using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Mjr.FeatureDriven.net4.Api.Data.Entities;
using Mjr.FeatureDriven.net4.Api.Data.Mapping;

namespace Mjr.FeatureDriven.net4.Api.Data
{
    public class ApiContext : Infrastructure.Data.BaseDbContext
    {

        public DbSet<Stock> Stocks { get; set; }

        public ApiContext() : base("StockConnectionstring")
        {
            Database.SetInitializer<ApiContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new StockMap());
            modelBuilder.Configurations.Add(new StockHistoryMap());

        }
    }
}