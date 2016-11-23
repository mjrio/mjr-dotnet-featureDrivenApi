using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Routing;

namespace Mjr.FeatureDriven.net4.Api.Features.V1.Stocks
{
    [ApiVersion1RoutePrefix("Stocks")]
    public class StocksController : ApiController
    {
        [Route("",Name="get")]
        [HttpGet]
        public string Get()
        {
            return "test";
        }
    }
}