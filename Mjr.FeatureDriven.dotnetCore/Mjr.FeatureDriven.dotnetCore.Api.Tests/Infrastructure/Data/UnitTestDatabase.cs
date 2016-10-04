using Mjr.FeatureDriven.dotnetCore.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mjr.FeatureDriven.dotnetCore.Api.Tests.Infrastructure.Data
{
    public class UnitTestContext : Context
    {
        public const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=Mjr.FeatureDriven.dotnetCore.Api_TestDb;Trusted_Connection=True;";
        private static DbContextOptions<Context> CreateNewContextOptions()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<Context>();
            builder.UseSqlServer(ConnectionString);

            return builder.Options;
        }

        public UnitTestContext():base(CreateNewContextOptions())
        {

        }
    }
}
