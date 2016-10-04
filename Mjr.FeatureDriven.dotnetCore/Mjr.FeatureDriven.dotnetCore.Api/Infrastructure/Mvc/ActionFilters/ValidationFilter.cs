using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Mvc.ActionFilters
{
    /// <summary>
    ///     Action filter to check the model state before the controller action is invoked.
    /// </summary>
    /// <remarks>
    ///     From http://www.asp.net/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api
    /// </remarks>
    public sealed class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid == false)
            {
                var message =
                    context.ModelState.Values.FirstOrDefault()?
                        .Errors.FirstOrDefault()?.Exception?.InnerException.Message;

                if (!string.IsNullOrWhiteSpace(message))
                {
                    context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(message)
                    {
                        StatusCode = 400,
                    };
                }
                else
                {
                    context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(context.ModelState)
                    {
                        StatusCode = 400,
                    };
                }
            }
            else
                await next();
        }
    }
}
