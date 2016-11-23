using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Authentication
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            //HttpContext.Current.User = principal; // For Mvc
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("user"), null);

            return await base.SendAsync(request, cancellationToken);
        }

    }
}