using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Mapping;
using System.Reflection;
using Mjr.FeatureDriven.dotnetCore.Infrastructure.Database;

namespace Mjr.FeatureDriven.dotnetCore.Api.Data
{
    public class Context : BaseContext<Context>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddEntityConfigurationsFromAssembly(Assembly.GetAssembly(typeof(UserMapping)));
        }
        public Context()
        {

        }
        public Context(DbContextOptions<Context> options)
        : base(options)
        {
        }
    }
}
