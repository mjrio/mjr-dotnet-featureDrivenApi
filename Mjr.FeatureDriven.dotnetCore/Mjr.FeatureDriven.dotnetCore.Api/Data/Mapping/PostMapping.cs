using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Mjr.FeatureDriven.dotnetCore.Api.Data.Entities;

namespace Mjr.FeatureDriven.dotnetCore.Api.Data.Mapping
{
    public class PostMapping : EntityMappingConfiguration<Post>
    {
        public override void Map(EntityTypeBuilder<Post> b)
        {
            b.ToTable("Posts");
            b.Property(c => c.CreatedAt).HasColumnType("datetime2");
            b.HasOne(p => p.From)
                .WithMany(p => p.Posts)
                .HasForeignKey(pt => pt.FromId);
        }
    }
}
