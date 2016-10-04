using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.dotnetCore.Api.Features.V2.Users
{
    [Route("api/v2/[Controller]")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //api/v2/users/{id} 
        [HttpPut("{id:int}", Name = "UpdateUserv2")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UpdateUser.Command command)
        {
            if (id != command.Id)
                return NotFound();
            await _mediator.SendAsync(command);
            return NoContent();
        }
    }
}
