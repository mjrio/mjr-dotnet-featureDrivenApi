using Mjr.FeatureDriven.dotnetCore.Api.Data;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Mjr.FeatureDriven.dotnetCore.Api.Features.V1.Users
{
    public class AddUser
    {
        public class Command : IRequest<Result>
        {
            [Required]
            [StringLength(17)]
            public string Username { get; set; }
            [Required]
            [StringLength(50)]
            public string Email { get; set; }
            [StringLength(20)]
            public string FirstName { get; set; }
            [StringLength(50)]
            public string LastName { get; set; }
            [StringLength(100)]
            public string IconUrl { get; set; }
            [StringLength(100)]
            public string AvatarUrl { get; set; }

        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly Context _context;
            private readonly IMapper _mapper;
            public Handler(Context context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public Result Handle(Command message)
            {
                var user = _mapper.Map<User>(message);
                _context.Users.Add(user);
                _context.SaveChanges();
                return _mapper.Map<Result>(user);
            }
        }

        public class Result
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, User>();
                CreateMap<User, Result>();
            }
        }
    }

}
