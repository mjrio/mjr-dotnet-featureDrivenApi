using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Mjr.FeatureDriven.dotnetCore.Api.Features.V1.Users
{
    /// <summary>
    /// Version 1 of the UsersController
    /// </summary>
    [Route("api/v1/[Controller]")]
    public class UsersController

    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //api/v1/users
        /// <summary>
        /// Add user
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Some result</returns>
        [HttpPost(Name = "AddUserv1")]
        public AddUser.Result AddUser([FromBody]AddUser.Command command)
        {
            return _mediator.Send(command);
        }

        //api/v1/users
        [HttpGet(Name = "GetUsersv1")]
        public List<User> GetUsers()
        {
            return _mediator.Send(new GetUsers.Query());
        }
        
        //api/v1/users/search
        [HttpGet("[action]", Name = "SearchUsersv1")]
        public List<User> Search(SearchUsers.Query query)
        {
            return _mediator.Send(query);
        }
    }
}
