using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Extensions;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Tracing;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling
{
    /// <summary>
    /// Represents the an attribute that provides a filter for unhandled exceptions.
    /// </summary>
    public class ExceptionHandlingFilter : ExceptionFilterAttribute
    {
        #region ExceptionHandlingFilter()
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingFilter"/> class.
        /// </summary>
        public ExceptionHandlingFilter()
            : base()
        {

        }
        #endregion

        #region DefaultHandler
        /// <summary>
        /// Gets a delegate method that returns an <see cref="HttpResponseMessage"/> 
        /// that describes the supplied exception.
        /// </summary>
        /// <value>
        /// A <see cref="Func{Exception, HttpRequestMessage, HttpResponseMessage}"/> delegate method that returns 
        /// an <see cref="HttpResponseMessage"/> that describes the supplied exception.
        /// </value>
        private static Func<Exception, HttpRequestMessage, HttpResponseMessage> DefaultHandler = (exception, request) =>
        {
            if (exception == null)
            {
                return null;
            }
            var stackTrace = exception.GetContentOf();
            var response = request.CreateResponse(HttpStatusCode.InternalServerError, stackTrace);
            response.ReasonPhrase = exception.Message.Replace(Environment.NewLine, String.Empty);

            ApiEventSource.Log.WebApiError(request.RequestUri.AbsoluteUri, response.ReasonPhrase, stackTrace);

            return response;
        };
        #endregion



        #region Handlers
        /// <summary>
        /// Gets the exception handlers registered with this filter.
        /// </summary>
        /// <value>
        /// A <see cref="ConcurrentDictionary{Type, Tuple}"/> collection that contains 
        /// the exception handlers registered with this filter.
        /// </value>
        protected ConcurrentDictionary<Type, Tuple<HttpStatusCode?, Func<Exception, HttpRequestMessage, HttpResponseMessage>>> Handlers
        {
            get
            {
                return _filterHandlers;
            }
        }
        private readonly ConcurrentDictionary<Type, Tuple<HttpStatusCode?, Func<Exception, HttpRequestMessage, HttpResponseMessage>>> _filterHandlers = new ConcurrentDictionary<Type, Tuple<HttpStatusCode?, Func<Exception, HttpRequestMessage, HttpResponseMessage>>>();
        #endregion

        #region OnException(HttpActionExecutedContext actionExecutedContext)
        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext == null || actionExecutedContext.Exception == null)
            {
                return;
            }

            var type = actionExecutedContext.Exception.GetType();

            var registration = GetRegistration(type);

            if (registration != null)
            {
                var statusCode = registration.Item1;
                var handler = registration.Item2;

                var response = handler(
                     actionExecutedContext.Exception.GetBaseException(),
                     actionExecutedContext.Request
                     );
                // Use registered status code if available
                if (statusCode.HasValue)
                {
                    response.StatusCode = statusCode.Value;
                }

                actionExecutedContext.Response = response;
            }
            else
            {
                // If no exception handler registered for the exception type, fallback to default handler
                actionExecutedContext.Response = DefaultHandler(
                    actionExecutedContext.Exception.GetBaseException(), actionExecutedContext.Request
                    );
            }
        }

        private Tuple<HttpStatusCode?, Func<Exception, HttpRequestMessage, HttpResponseMessage>> GetRegistration(Type type)
        {
            return (from handler in Handlers where handler.Key == type || type.IsSubclassOf(handler.Key) select handler.Value).FirstOrDefault();
        }

        #endregion

        #region Register<TException>(HttpStatusCode statusCode)
        /// <summary>
        /// Registers an exception handler that returns the specified status code for exceptions of type <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of exception to register a handler for.</typeparam>
        /// <param name="statusCode">The HTTP status code to return for exceptions of type <typeparamref name="TException"/>.</param>
        /// <returns>
        /// This <see cref="ExceptionHandlingFilter"/> after the exception handler has been added.
        /// </returns>
        public ExceptionHandlingFilter Register<TException>(HttpStatusCode statusCode)
            where TException : Exception
        {

            var type = typeof(TException);
            var item = new Tuple<HttpStatusCode?, Func<Exception, HttpRequestMessage, HttpResponseMessage>>(
                statusCode, DefaultHandler
                );

            if (!this.Handlers.TryAdd(type, item))
            {
                Tuple<HttpStatusCode?, Func<Exception, HttpRequestMessage, HttpResponseMessage>> oldItem = null;

                if (this.Handlers.TryRemove(type, out oldItem))
                {
                    this.Handlers.TryAdd(type, item);
                }
            }

            return this;
        }
        #endregion

        #region Register<TException>(Func<Exception, HttpRequestMessage, HttpResponseMessage> handler)
        /// <summary>
        /// Registers the specified exception <paramref name="handler"/> for exceptions of type <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of exception to register the <paramref name="handler"/> for.</typeparam>
        /// <param name="handler">The exception handler responsible for exceptions of type <typeparamref name="TException"/>.</param>
        /// <returns>
        /// This <see cref="ExceptionHandlingFilter"/> after the exception <paramref name="handler"/> 
        /// has been added.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref name="handler"/> is <see langword="null"/>.</exception>
        public ExceptionHandlingFilter Register<TException>(Func<Exception, HttpRequestMessage, HttpResponseMessage> handler)
            where TException : Exception
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            var type = typeof(TException);
            var item = new Tuple<HttpStatusCode?, Func<Exception, HttpRequestMessage, HttpResponseMessage>>(
                null, handler
                );

            if (!this.Handlers.TryAdd(type, item))
            {
                Tuple<HttpStatusCode?, Func<Exception, HttpRequestMessage, HttpResponseMessage>> oldItem = null;

                if (this.Handlers.TryRemove(type, out oldItem))
                {
                    this.Handlers.TryAdd(type, item);
                }
            }

            return this;
        }
        #endregion

        #region Unregister<TException>()
        /// <summary>
        /// Unregisters the exception handler for exceptions of type <typeparamref name="TException"/>.
        /// </summary>
        /// <typeparam name="TException">The type of exception to unregister handlers for.</typeparam>
        /// <returns>
        /// This <see cref="ExceptionHandlingFilter"/> after the exception handler 
        /// for exceptions of type <typeparamref name="TException"/> has been removed.
        /// </returns>
        public ExceptionHandlingFilter Unregister<TException>()
            where TException : Exception
        {
            Tuple<HttpStatusCode?, Func<Exception, HttpRequestMessage, HttpResponseMessage>> item = null;

            this.Handlers.TryRemove(typeof(TException), out item);

            return this;
        }
        #endregion
    }
}