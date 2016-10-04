using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text.Encodings.Web;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.ExceptionHandling
{
    public static class ExceptionHandlingConfig
    {
        public static ConcurrentDictionary<Type, HttpStatusCode> Codes = new ConcurrentDictionary<Type, HttpStatusCode>();
        public static IApplicationBuilder ConfigureExceptionHandling(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                /*
                 * Captures synchronous and asynchronous database related exceptions from the pipeline 
                 * that may be resolved using Entity Framework migrations. 
                 * When these exceptions occur an HTML response with details of possible actions 
                 * to resolve the issue is generated.
                 * */
                app.UseDatabaseErrorPage();
            }
            else
            {
                //Configure the error handler to show an error page.
                app.UseExceptionHandler(errorApp =>
                {
                    // Normally you'd use MVC or similar to render a nice page.
                    errorApp.Run(async context =>
                   {
                       context.Response.StatusCode = 500;
                       context.Response.ContentType = "text/html";
                       await context.Response.WriteAsync("<html><body>\r\n");
                       await context.Response.WriteAsync("We're sorry, we encountered an un-expected issue with your application.<br>\r\n");

                       var error = context.Features.Get<IExceptionHandlerFeature>();
                       if (error != null)
                       {
                      // This error would not normally be exposed to the client
                      await context.Response.WriteAsync("<br>Error: " + HtmlEncoder.Default.Encode(error.Error.Message) + "<br>\r\n");
                       }
                       await context.Response.WriteAsync("<br><a href=\"/\">Home</a><br>\r\n");
                       await context.Response.WriteAsync("</body></html>\r\n");
                       await context.Response.WriteAsync(new string(' ', 512)); // Padding for IE
              });
                });
            }
            return app;
        }
    }
}

