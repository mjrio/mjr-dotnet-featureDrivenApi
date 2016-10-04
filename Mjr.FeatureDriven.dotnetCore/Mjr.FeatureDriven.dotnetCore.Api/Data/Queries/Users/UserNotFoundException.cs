using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.dotnetCore.Api.Data.Queries.Users
{
    public class UserNotFoundException: Exception
    {
        public UserNotFoundException(int userId) 
            : base($"User with id: {userId} not found.")
        {

        }
    }
}
