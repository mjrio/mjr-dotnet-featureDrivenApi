using Mjr.FeatureDriven.dotnetCore.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Database
{
    public static class DatabaseConfig
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            return services.
                AddDbContext<Context>(options =>
                {
                   // options.EnableSensitiveDataLogging();
                    options.UseSqlServer(connectionString);
                }
            );
        }
        public static IApplicationBuilder ConfigureDatabase(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
