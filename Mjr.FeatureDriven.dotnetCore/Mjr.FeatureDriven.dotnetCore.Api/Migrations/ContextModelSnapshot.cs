using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Mjr.FeatureDriven.dotnetCore.Api.Data;

namespace Mjr.FeatureDriven.dotnetCore.Api.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mjr.FeatureDriven.dotnetCore.Api.Data.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 500);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("FromId");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Mjr.FeatureDriven.dotnetCore.Api.Data.Entities.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Priority")
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 30);

                    b.HasKey("Id");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Mjr.FeatureDriven.dotnetCore.Api.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AvatarUrl")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("IconUrl")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("IsActive");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 17);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Mjr.FeatureDriven.dotnetCore.Api.Data.Entities.Post", b =>
                {
                    b.HasOne("Mjr.FeatureDriven.dotnetCore.Api.Data.Entities.User", "From")
                        .WithMany("Posts")
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
