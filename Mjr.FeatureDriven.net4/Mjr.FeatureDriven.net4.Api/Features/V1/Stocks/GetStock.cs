using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using MediatR;
using Mjr.FeatureDriven.net4.Api.Data;
using Mjr.FeatureDriven.net4.Api.Data.Entities;
using Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling.Exceptions;

namespace Mjr.FeatureDriven.net4.Api.Features.V1.Stocks
{
    public class GetStock
    {
        public class Query : IAsyncRequest<Result>
        {
            [Required]
            [StringLength(3)]
            public string CurrencyCode { get; set; }
            [Required]
            [StringLength(16)]
            public string IsinCode { get; set; }

            public Query(string currencyCode, string isincode)
            {
                CurrencyCode = currencyCode;
                IsinCode = isincode;
            }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly ApiContext _apiContext;
            private readonly IMapper _mapper;

            public Handler(ApiContext apiContext, IMapper mapper)
            {
                _apiContext = apiContext;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Query message)
            {
                var stock = await
                    _apiContext.Stocks.SingleOrDefaultAsync(
                        s => s.IsinCode == message.IsinCode && s.CurrencyCode == message.CurrencyCode);
                if (stock == null)
                    throw new RootObjectNotFoundException("Stock not found.");
                return _mapper.Map<Result>(stock);

            }
        }
        public class Result
        {
            [StringLength(400)]
            public string Name { get; set; }
        }

        public class MappingProfile : Profile
        {
            protected override void Configure()
            {
                CreateMap<Stock, Result>();
            }
        }
    }
}