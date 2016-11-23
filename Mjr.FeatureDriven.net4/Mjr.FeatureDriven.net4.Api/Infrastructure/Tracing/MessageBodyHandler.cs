using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Tracing
{
    public class MessageBodyHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post)
            {
                var requestMessage = await request.Content.ReadAsByteArrayAsync();
                var body = Encoding.UTF8.GetString(requestMessage);
                ApiEventSource.Log.Body(body);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}