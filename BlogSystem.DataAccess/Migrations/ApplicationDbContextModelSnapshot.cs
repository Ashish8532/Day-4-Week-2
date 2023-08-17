﻿// <auto-generated />
using System;
using BlogSystem.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogSystem.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BlogSystem.Models.Models.Blog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Blogs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "This is Blog 1.",
                            Title = "Blog 1"
                        },
                        new
                        {
                            Id = 2,
                            Content = "This is Blog 2.",
                            Title = "Blog 2"
                        },
                        new
                        {
                            Id = 3,
                            Content = "This is Blog 3.",
                            Title = "Blog 3"
                        });
                });

            modelBuilder.Entity("BlogSystem.Models.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BlogId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BlogId = 1,
                            Content = "This is Comment 1."
                        },
                        new
                        {
                            Id = 2,
                            BlogId = 1,
                            Content = "This is Comment 2."
                        },
                        new
                        {
                            Id = 3,
                            BlogId = 1,
                            Content = "This is Comment 3."
                        },
                        new
                        {
                            Id = 4,
                            BlogId = 2,
                            Content = "This is Comment 4."
                        },
                        new
                        {
                            Id = 5,
                            BlogId = 2,
                            Content = "This is Comment 5."
                        },
                        new
                        {
                            Id = 6,
                            BlogId = 3,
                            Content = "This is Comment 6."
                        },
                        new
                        {
                            Id = 7,
                            BlogId = 3,
                            Content = "This is Comment 7."
                        });
                });

            modelBuilder.Entity("BlogSystem.Models.Models.Comment", b =>
                {
                    b.HasOne("BlogSystem.Models.Models.Blog", "Blog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("BlogSystem.Models.Models.Blog", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
