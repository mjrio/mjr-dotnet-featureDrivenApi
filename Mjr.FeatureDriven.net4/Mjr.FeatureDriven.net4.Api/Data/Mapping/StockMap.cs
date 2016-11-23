using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Mjr.FeatureDriven.net4.Api.Data.Entities;

namespace Mjr.FeatureDriven.net4.Api.Data.Mapping
{
    public class StockMap : EntityTypeConfiguration<Stock>
    {
        public StockMap()
        {
            ToTable("Stocks");
        }
    }
}