using Mjr.FeatureDriven.dotnetCore.Api.Data;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using AutoMapper;
using MediatR;

namespace Mjr.FeatureDriven.dotnetCore.Api.Features.V0.Users
{
    public class AddUserV0
    {
        public class Command : IRequest<int>
        {
            public User User { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly Context _context;
            public Handler(Context context)
            {
                _context = context;
            }
            public int Handle(Command message)
            {
                _context.Users.Add(message.User);
                _context.SaveChanges();
                return message.User.Id;
            }
        }
    }
}
