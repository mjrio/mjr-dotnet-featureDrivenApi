using Mjr.FeatureDriven.dotnetCore.Api.Data;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace Mjr.FeatureDriven.dotnetCore.Api.Features.V1.Users
{
    public class GetUsers
    {
        public class Query: IRequest<List<User>>
        {
            

        }

        public class Handler : IRequestHandler<Query, List<User>>
        {
            private readonly Context _context;
            private readonly IMapper _mapper;
            public Handler(Context context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public List<User> Handle(Query message)
            {
                return _context.Users.ToList();
               
            }
        }
    }
}
