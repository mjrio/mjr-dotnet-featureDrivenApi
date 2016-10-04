using Mjr.FeatureDriven.dotnetCore.Api.Data;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.dotnetCore.Api.Features.V2.Users
{
    public class UpdateUser
    {
        /// <summary>
        /// Fill in these fields to update the user
        /// </summary>
        public class Command : IAsyncRequest
        {
            public int Id { get; set; }
            [Required]
            [StringLength(17)]
            public string Username { get; set; }
            [Required]
            [StringLength(50)]
            public string Email { get; set; }
            [Required]
            [StringLength(20)]
            public string FirstName { get; set; }
            [Required]
            [StringLength(50)]
            public string LastName { get; set; }
            [StringLength(100)]
            public string IconUrl { get; set; }
            [StringLength(100)]
            public string AvatarUrl { get; set; }

        }
        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly Context _context;
            private readonly IMapper _mapper;
            public Handler(Context context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            protected override async Task HandleCore(Command message)
            {
                var user = _mapper.Map<User>(message);
                _context.Update(user);
                //EF6: _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, User>();
            }
        }
    }
}
