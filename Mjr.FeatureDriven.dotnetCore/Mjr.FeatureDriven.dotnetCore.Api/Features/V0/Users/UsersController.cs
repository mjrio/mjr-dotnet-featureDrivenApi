using Mjr.FeatureDriven.dotnetCore.Api.Data;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Mjr.FeatureDriven.dotnetCore.Api.Features.V0.Users
{
    [Route("api/v0/users")]
    public class UsersController : Controller
    {
        ///api/v0/users
        [HttpPost]
        public int Post(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentNullException(nameof(user.Email));
            try
            {
                using (var context = new Context())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    return user.Id;
                }
            }
            catch (Exception)
            {
                //sth happened we will log it.
                return 0;
            }
        }



        public int Post2(User user)
        {
            return _mediatr.Send(new AddUser.Command { User = user });
        }






        private readonly MediatR.IMediator _mediatr;
        public UsersController(MediatR.IMediator mediatr)
        {
            _mediatr = mediatr;
        }
    }
}
