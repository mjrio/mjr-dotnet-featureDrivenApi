using Mjr.FeatureDriven.dotnetCore.Api.Data;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Mjr.FeatureDriven.dotnetCore.Api.Features.V1.Users
{
    public class SearchUsers
    {
        public class Query : IRequest<List<User>>
        {
            [Required]
            public string Name { get; set; }
        }
        public class Handler : IRequestHandler<Query, List<User>>
        {
            private readonly Context _context;
            public Handler(Context context)
            {
                _context = context;
            }
            public List<User> Handle(Query message)
            {
                return _context.Users.Where(s=>s.FirstName.Contains(message.Name)).ToList();
            }
        }
    }
}
