using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Mvc.ExceptionFilters
{
    public class DbUpdateConcurrencyExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;
        public DbUpdateConcurrencyExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DbUpdateConcurrencyException>();
        }
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled && context.Exception.GetType() == typeof(DbUpdateConcurrencyException))
            {
                _logger.LogError("{0} failed with DbUpdateConcurrencyException", context.ActionDescriptor.DisplayName);
                var message = new
                {
                    Message = context.Exception.Message,
                    Description = $"{context.ActionDescriptor.DisplayName} could not be saved to the db because a concurrency violation. Actualize your data and try again.",
                };
                context.Result = new JsonResult(message)
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
