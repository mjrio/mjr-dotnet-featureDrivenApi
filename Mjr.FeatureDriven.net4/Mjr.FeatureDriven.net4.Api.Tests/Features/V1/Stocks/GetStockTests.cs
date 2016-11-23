using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mjr.FeatureDriven.net4.Api.Data.Entities;
using Mjr.FeatureDriven.net4.Api.Features.V1.Stocks;
using Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie;
using Shouldly;

namespace Mjr.FeatureDriven.net4.Api.Tests.Features.V1.Stocks
{
    public class GetStockTests
    {
        public async Task GetStock(TestContextFixture testContextFixture, Stock stock, GetStock.Query getStock)
        {
            //arrange
            testContextFixture.SaveAll(stock);
            getStock.CurrencyCode = stock.CurrencyCode;
            getStock.IsinCode = stock.IsinCode;

            //act
            var result = await testContextFixture.SendAsync(getStock);

            //assert
            result.Name.ShouldBe(stock.Name);
        }
    }
}
