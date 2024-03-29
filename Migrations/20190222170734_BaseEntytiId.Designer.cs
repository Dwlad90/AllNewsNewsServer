﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using AllNewsServer.Data;

namespace AllNewsServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190222170734_BaseEntytiId")]
    partial class BaseEntytiId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("AllNewsServer.Data.Models.ActiveSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ApplicationUserId");

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("IPAddress");

                    b.Property<bool>("IsTerminated");

                    b.Property<DateTime>("LastRefreshDateTime");

                    b.Property<string>("Location");

                    b.Property<string>("RefreshToken");

                    b.Property<string>("UserAgent");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("ActiveSessions");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("CreationUser");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ModificationDateTime");

                    b.Property<string>("ModificationUser");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("ApplicationRoles");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("CreationUser");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Hash");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsBanned");

                    b.Property<string>("LastName");

                    b.Property<int>("LoginAttempts");

                    b.Property<DateTime?>("LoginBannedUntilDateTime");

                    b.Property<string>("MiddleName");

                    b.Property<DateTime>("ModificationDateTime");

                    b.Property<string>("ModificationUser");

                    b.Property<string>("Phone");

                    b.Property<string>("PushNotificationsToken");

                    b.Property<byte[]>("Salt");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.Culture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("CreationUser");

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Key");

                    b.Property<DateTime>("ModificationDateTime");

                    b.Property<string>("ModificationUser");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Cultures");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.LocalizationResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("CreationUser");

                    b.Property<int>("CultureId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Key");

                    b.Property<DateTime>("ModificationDateTime");

                    b.Property<string>("ModificationUser");

                    b.Property<int?>("OrderStatusId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CultureId");

                    b.HasIndex("OrderStatusId");

                    b.ToTable("LocalizationResources");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("CreationUser");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ModificationDateTime");

                    b.Property<string>("ModificationUser");

                    b.Property<int>("OrderStatusId");

                    b.HasKey("Id");

                    b.HasIndex("OrderStatusId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.OrderStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("CreationUser");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Key");

                    b.Property<DateTime>("ModificationDateTime");

                    b.Property<string>("ModificationUser");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("OrderStatuses");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.UserRole", b =>
                {
                    b.Property<int>("ApplicationRoleId");

                    b.Property<int>("ApplicationUserId");

                    b.HasKey("ApplicationRoleId", "ApplicationUserId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.ActiveSession", b =>
                {
                    b.HasOne("AllNewsServer.Data.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("ActiveSessions")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.LocalizationResource", b =>
                {
                    b.HasOne("AllNewsServer.Data.Models.Culture", "Culture")
                        .WithMany("LocalizationResources")
                        .HasForeignKey("CultureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AllNewsServer.Data.Models.OrderStatus", "OrederStatus")
                        .WithMany("Names")
                        .HasForeignKey("OrderStatusId");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.Order", b =>
                {
                    b.HasOne("AllNewsServer.Data.Models.OrderStatus", "OrderStatus")
                        .WithMany("Orders")
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.UserRole", b =>
                {
                    b.HasOne("AllNewsServer.Data.Models.ApplicationRole", "ApplicationRole")
                        .WithMany("UserRoles")
                        .HasForeignKey("ApplicationRoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AllNewsServer.Data.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("UserRoles")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
