using System;
using System.Collections.Generic;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Authentication;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.Security
{
    public class TestUserSession : IUserSession
    {
        public string UserCode => Environment.UserName;

        public List<string> Permissions { get; set; }

        public void AddPermission(params string[] permissions)
        {
            Permissions.AddRange(permissions);
        }

        public TestUserSession()
        {
            Permissions = new List<string>();
           
        }
        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission);
        }
    }
}
