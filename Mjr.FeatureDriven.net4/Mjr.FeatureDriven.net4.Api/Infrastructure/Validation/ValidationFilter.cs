using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Validation
{

    /// <summary>
    ///     Action filter to check the model state before the controller action is invoked.
    /// </summary>
    /// <remarks>
    ///     From http://www.asp.net/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public sealed class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                var message =
                    actionContext.ModelState.Values.FirstOrDefault()?
                        .Errors.FirstOrDefault()?.Exception?.InnerException.Message;
                actionContext.Response = !string.IsNullOrWhiteSpace(message) ? actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, message) : actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }

        /// <summary>
        /// To prevent filter from executing twice on same call. Problem solved by:
        /// http://stackoverflow.com/questions/18485479/webapi-filter-is-calling-twice?rq=1
        /// </summary>
        public override bool AllowMultiple
        {
            get { return false; }
        }
    }

}