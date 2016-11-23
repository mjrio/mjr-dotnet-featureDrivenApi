using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling.Exceptions;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Extensions;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Tracing;
using NLog;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling
{
    public static class ExceptionHandlingConfig
    {
        internal static void Configure(HttpConfiguration config)
        {
            config.Filters.Add(
            new ExceptionHandlingFilter()
                .Register<KeyNotFoundException>(HttpStatusCode.NotFound)
                .Register<SecurityException>(HttpStatusCode.Forbidden)
                .Register<InvalidPanException>(HttpStatusCode.BadRequest)
                .Register<RootObjectNotFoundException>(UserFriendlyNotFoundResponseMessage)
                .Register<ChildObjectNotFoundException>(UserFriendlyNotFoundResponseMessage)
                .Register<DbEntityValidationException>(DbEntityValidationException)
                .Register<BusinessRuleViolationException>((exception, request) =>
                {
                    ApiEventSource.Log.FunctionalWarning(request.RequestUri.AbsoluteUri, exception.Message, exception.GetContentOf());
                    return request
                         .CreateErrorResponse(
                             HttpStatusCode
                                 .PaymentRequired,
                             exception.Message);
                })
                .Register<ValidationException>((exception, request) =>
                {
                    ApiEventSource.Log.FunctionalWarning(request.RequestUri.AbsoluteUri, exception.Message,
                        exception.GetContentOf());
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, exception.Message);
                }
                )

                .Register<SqlException>(
                    (exception, request) =>
                    {
                        var sqlException = exception as SqlException;
                        if (sqlException != null)
                        {
                            ApiEventSource.Log.WebApiError(request.RequestUri.AbsoluteUri, exception.Message,
                           exception.GetContentOf());
                            if (sqlException.Number == 4060)
                            {
                                //Provider failed on open, so could be permission issue or there is no database.
                                //If you get this error locally, you have to run update-database in the package-manager console, so the database is created or put in the correct state.
                            }
                            else
                            if (sqlException.Number > 50000)
                            {
                                var badResponse = request.CreateResponse(HttpStatusCode.BadRequest);
                                badResponse.ReasonPhrase = sqlException.Message.Replace(Environment.NewLine, String.Empty);
                                return badResponse;
                            }
                        }
                        var response = request.CreateResponse(HttpStatusCode.InternalServerError);
                        response.ReasonPhrase = exception.Message.Replace(Environment.NewLine, String.Empty);
                        LogManager.GetCurrentClassLogger().Log(LogLevel.Error, exception.Message);
                        return response;
                    }
                )
            );
        }

        private static HttpResponseMessage DbEntityValidationException(Exception exception, HttpRequestMessage request)
        {
            var dbEx = (DbEntityValidationException)exception;
            var validationErrorMessage = dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors).Aggregate("", (current, validationError) => current + validationError.ErrorMessage);
            ApiEventSource.Log.WebApiError(request.RequestUri.AbsoluteUri, validationErrorMessage, exception.GetContentOf());
            return request.CreateErrorResponse(HttpStatusCode.InternalServerError, validationErrorMessage);
        }
        private static HttpResponseMessage UserFriendlyNotFoundResponseMessage(Exception exception, HttpRequestMessage request)
        {
            ApiEventSource.Log.FunctionalWarning(request.RequestUri.AbsoluteUri, exception.Message, exception.GetContentOf());
            return request.CreateErrorResponse(HttpStatusCode.NotFound, exception.Message);
        }
    }
}