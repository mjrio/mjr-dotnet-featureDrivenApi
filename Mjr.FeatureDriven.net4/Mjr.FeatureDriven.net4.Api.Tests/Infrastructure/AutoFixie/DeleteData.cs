using System;
using System.Linq;
using Fixie;
using Mjr.FeatureDriven.net4.Api.Data;
using Respawn;

namespace Mjr.FeatureDriven.net4.Api.Tests.Infrastructure.AutoFixie
{
    public class ClearPermissions : FixtureBehavior
    {
        public void Execute(Fixture context, Action next)
        {
            //clears the permissions, hacky...
            if (context.Cases.Any())
            {
                if (context.Cases[0]?.Parameters != null )
                    if (context.Cases[0].Parameters.Any())
                    {
                        var testContextFixture = context.Cases[0].Parameters[0] as TestContextFixture;
                        testContextFixture?.UserSession.Permissions.Clear();
                    }
            }
            next();
        }
    }
    public class DeleteData : FixtureBehavior, ClassBehavior
    {
        private static Checkpoint checkpoint = new Checkpoint
        {
            TablesToIgnore = new[]
            {
                "sysdiagrams",
                "__MigrationHistory"
            }
        };

        public void Execute(Fixture context, Action next)
        {
            DeleteAllData();
            next();
        }

        public void Execute(Class context, Action next)
        {
            DeleteAllData();
            next();
        }

        private static void DeleteAllData()
        {
            checkpoint.Reset(new ApiContext().Database.Connection.ConnectionString);
        }
    }
}