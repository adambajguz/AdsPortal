﻿// <auto-generated />
using System;
using AdsPortal.WebApi.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdsPortal.WebApi.Persistence.Migrations
{
    [DbContext(typeof(RelationalDbContext))]
    partial class RelationalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.HasSequence("Job_JobNo_Sequence");

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.Advertisement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CoverImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastExpirationNotification")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastSavedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastSavedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("VisibleTo")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CoverImageId");

                    b.ToTable("Advertisements");
                });

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LastSavedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastSavedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.EntityAuditLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Action")
                        .HasColumnType("int");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Key")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Values")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Key");

                    b.ToTable("EntityAuditLogs");
                });

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Arguments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FinishedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("Instance")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("JobNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)")
                        .HasDefaultValueSql("NEXT VALUE FOR Job_JobNo_Sequence");

                    b.Property<string>("Operation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PostponeTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<Guid?>("RunAfterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("StartedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("TimeoutAfter")
                        .HasColumnType("time");

                    b.Property<int>("Tries")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("JobNo")
                        .IsUnique();

                    b.HasIndex("Priority");

                    b.HasIndex("RunAfterId")
                        .IsUnique()
                        .HasFilter("[RunAfterId] IS NOT NULL");

                    b.HasIndex("Status");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.MediaItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ByteSize")
                        .HasColumnType("bigint");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Data")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LastSavedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastSavedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("PathHashCode")
                        .HasColumnType("bigint");

                    b.Property<long>("Role")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VirtualDirectory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PathHashCode");

                    b.ToTable("MediaItems");
                });

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid?>("LastSavedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastSavedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Role")
                        .HasColumnType("bigint");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a76b1435-1fe5-4cec-b1fc-083190fa7ec5"),
                            CreatedOn = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "admin@adsportal.com",
                            IsActive = true,
                            LastSavedOn = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Admin",
                            Password = "00K7UWlOrhhboShjjWT4jSZafPuFHaJDlZxqsxnxfJI5FN4+HPA7r9wlGHQKhIoM2/mfXXWzjVX0aaVo1Uo0JlTQ==co8P2fSN/QYvZSkYBAVanudgRvtXYG3rXNb04AYzR7gbcY4SbWrFFNea9qsaOa+PGDCpW7+fq8rB2uey83mdyQ==",
                            Role = 6L,
                            Surname = "AdsPortal"
                        });
                });

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.Advertisement", b =>
                {
                    b.HasOne("AdsPortal.WebApi.Domain.Entities.User", "Author")
                        .WithMany("Advertisements")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdsPortal.WebApi.Domain.Entities.Category", "Category")
                        .WithMany("Advertisements")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdsPortal.WebApi.Domain.Entities.MediaItem", "CoverImage")
                        .WithMany()
                        .HasForeignKey("CoverImageId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Author");

                    b.Navigation("Category");

                    b.Navigation("CoverImage");
                });

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.Job", b =>
                {
                    b.HasOne("AdsPortal.WebApi.Domain.Entities.Job", "RunAfter")
                        .WithOne()
                        .HasForeignKey("AdsPortal.WebApi.Domain.Entities.Job", "RunAfterId");

                    b.Navigation("RunAfter");
                });

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.Category", b =>
                {
                    b.Navigation("Advertisements");
                });

            modelBuilder.Entity("AdsPortal.WebApi.Domain.Entities.User", b =>
                {
                    b.Navigation("Advertisements");
                });
#pragma warning restore 612, 618
        }
    }
}
