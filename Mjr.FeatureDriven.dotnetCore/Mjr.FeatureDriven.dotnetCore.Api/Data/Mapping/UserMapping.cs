using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;

namespace Mjr.FeatureDriven.dotnetCore.Api.Data.Mapping
{

    public class UserMapping : EntityMappingConfiguration<User>
    {
        public override void Map(EntityTypeBuilder<User> b)
        {
            b.ToTable("Users");
            
        }
    }
}
