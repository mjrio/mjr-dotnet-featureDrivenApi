using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Authentication
{
    public interface IUserSession
    {
        string UserCode { get; }

        List<string> Permissions { get; }
        bool HasPermission(string permission);

    }
    /// <summary>
    /// fetch all info from Thread principle
    /// </summary>
    public class UserSession : IUserSession
    {
        public string UserCode
        {
            get
            {

                return Thread.CurrentPrincipal.Identity.Name;
            }
        }


        public List<string> Permissions
        {
            get
            {
                var rolePermissions = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).FindAll("roleType");
                var permissions = rolePermissions.Select(s => s.Value);
                return permissions.ToList(); ;
            }
        }

        public bool HasPermission(string permission)
        {
            return Thread.CurrentPrincipal.IsInRole(permission);
        }

    }
}