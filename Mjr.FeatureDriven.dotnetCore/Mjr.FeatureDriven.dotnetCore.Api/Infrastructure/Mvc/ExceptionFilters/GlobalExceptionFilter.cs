using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Mvc.ExceptionFilters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        public GlobalExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GlobalExceptionFilter>();
        }
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                _logger.LogError("{0} failed", context.ActionDescriptor.DisplayName);
                var message = new
                {
                    Message = context.Exception.Message,
                    Description = $"No idea what went wrong, contact your admin.",
                };
                context.Result = new JsonResult(message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
