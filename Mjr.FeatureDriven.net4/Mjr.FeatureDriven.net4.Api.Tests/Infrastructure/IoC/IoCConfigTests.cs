using System.Diagnostics;
using AutoMapper;
using MediatR;
using Mjr.FeatureDriven.net4.Api.Features.V1.Stocks;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.IoC
{
    public class IoCConfigTests
    {
        public void ShouldNotThrowException()
        {
            var container = Api.Infrastructure.IoC.IoCConfig.Container;
            Debug.WriteLine(container.WhatDoIHave());

            container.GetInstance<IMediator>();
            container.GetInstance<IMapper>();
            container.GetInstance<IAsyncRequestHandler<GetStock.Query, GetStock.Result>>();
            container.AssertConfigurationIsValid();
        }
    }
}
