using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using System.Linq;

namespace Mjr.FeatureDriven.dotnetCore.Api.Data.Queries.Users
{
    public class GetUserById
    {
        private readonly Context _context;
        public GetUserById(Context context)
        {
            _context = context;
        }

        public User Execute(int userId, bool throwOnNotFound=true)
        {
            var user = _context.Users.SingleOrDefault(s => s.Id == userId);
            if (user == null && throwOnNotFound)
                throw new UserNotFoundException(userId);
            return user;
        }
    }
}
