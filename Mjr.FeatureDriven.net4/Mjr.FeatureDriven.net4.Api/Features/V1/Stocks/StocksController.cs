using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using MediatR;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Routing;

namespace Mjr.FeatureDriven.net4.Api.Features.V1.Stocks
{
    [ApiVersion1RoutePrefix("Stock")]
    public class StocksController : ApiController
    {
        private readonly IMediator _mediator;

        public StocksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("{currencyCode}/{isinCode}",Name="get")]
        [HttpGet]
        public async Task<GetStock.Result> Get(string currencyCode, string isinCode)
        {
            return await _mediator.SendAsync(new GetStock.Query(currencyCode, isinCode));
        }
    }
}