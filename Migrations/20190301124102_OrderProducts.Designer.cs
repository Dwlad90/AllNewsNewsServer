﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using AllNewsServer.Data;

namespace AllNewsServer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190301124102_OrderProducts")]
    partial class OrderProducts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:postgis", ",,")
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

                    b.HasIndex("Email")
                        .IsUnique();

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

            modelBuilder.Entity("AllNewsServer.Data.Models.Currency", b =>
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

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.DeliveryType", b =>
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

                    b.ToTable("DeliveryTypes");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.LocalizationResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("CreationUser");

                    b.Property<int>("CultureId");

                    b.Property<int?>("CurrencyId");

                    b.Property<int?>("DeliveryTypeId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Key");

                    b.Property<DateTime>("ModificationDateTime");

                    b.Property<string>("ModificationUser");

                    b.Property<int?>("OrderStatusId");

                    b.Property<int?>("ProductTypeId");

                    b.Property<int?>("SizeUnitId");

                    b.Property<string>("Value");

                    b.Property<int?>("VolumeUnitId");

                    b.Property<int?>("WeightUnitId");

                    b.HasKey("Id");

                    b.HasIndex("CultureId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("DeliveryTypeId");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("ProductTypeId");

                    b.HasIndex("SizeUnitId");

                    b.HasIndex("VolumeUnitId");

                    b.HasIndex("WeightUnitId");

                    b.ToTable("LocalizationResources");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("CreationUser");

                    b.Property<int>("CustomerId");

                    b.Property<int>("DeliveryTypeId");

                    b.Property<string>("Description");

                    b.Property<Point>("EndLocation")
                        .HasColumnType("geography")
                        .HasAnnotation("Sqlite:Srid", 4326);

                    b.Property<int?>("ExecutorId");

                    b.Property<DateTime>("ExpectingDateTime");

                    b.Property<DateTime?>("FinishingTime");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ModificationDateTime");

                    b.Property<string>("ModificationUser");

                    b.Property<string>("Name");

                    b.Property<int>("OrderStatusId");

                    b.Property<Point>("StartLocation")
                        .HasColumnType("geography")
                        .HasAnnotation("Sqlite:Srid", 4326);

                    b.Property<DateTime?>("TakingDateTime");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DeliveryTypeId");

                    b.HasIndex("ExecutorId");

                    b.HasIndex("OrderStatusId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.OrderProduct", b =>
                {
                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProducts");
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

            modelBuilder.Entity("AllNewsServer.Data.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<string>("CreationUser");

                    b.Property<int?>("CurrencyId");

                    b.Property<double?>("Depth");

                    b.Property<string>("Description");

                    b.Property<double?>("Height");

                    b.Property<string>("Image");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ModificationDateTime");

                    b.Property<string>("ModificationUser");

                    b.Property<string>("Name");

                    b.Property<double>("Price");

                    b.Property<int>("ProductTypeId");

                    b.Property<int?>("SizeUnitId");

                    b.Property<double?>("Volume");

                    b.Property<int?>("VolumeUnitId");

                    b.Property<double>("Weight");

                    b.Property<int>("WeightUnitId");

                    b.Property<double?>("Width");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ProductTypeId");

                    b.HasIndex("SizeUnitId");

                    b.HasIndex("VolumeUnitId");

                    b.HasIndex("WeightUnitId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.ProductType", b =>
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

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.SizeUnit", b =>
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

                    b.ToTable("SizeUnits");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.UserRole", b =>
                {
                    b.Property<int>("ApplicationRoleId");

                    b.Property<int>("ApplicationUserId");

                    b.HasKey("ApplicationRoleId", "ApplicationUserId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.VolumeUnit", b =>
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

                    b.ToTable("VolumeUnits");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.WeightUnit", b =>
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

                    b.ToTable("WeightUnits");
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
                        .WithMany()
                        .HasForeignKey("CultureId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AllNewsServer.Data.Models.Currency")
                        .WithMany("Names")
                        .HasForeignKey("CurrencyId");

                    b.HasOne("AllNewsServer.Data.Models.DeliveryType", "DeliveryType")
                        .WithMany("Names")
                        .HasForeignKey("DeliveryTypeId");

                    b.HasOne("AllNewsServer.Data.Models.OrderStatus", "OrederStatus")
                        .WithMany("Names")
                        .HasForeignKey("OrderStatusId");

                    b.HasOne("AllNewsServer.Data.Models.ProductType", "ProductType")
                        .WithMany("Names")
                        .HasForeignKey("ProductTypeId");

                    b.HasOne("AllNewsServer.Data.Models.SizeUnit")
                        .WithMany("Descriptions")
                        .HasForeignKey("SizeUnitId");

                    b.HasOne("AllNewsServer.Data.Models.VolumeUnit")
                        .WithMany("Descriptions")
                        .HasForeignKey("VolumeUnitId");

                    b.HasOne("AllNewsServer.Data.Models.WeightUnit")
                        .WithMany("Descriptions")
                        .HasForeignKey("WeightUnitId");
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.Order", b =>
                {
                    b.HasOne("AllNewsServer.Data.Models.ApplicationUser", "Customer")
                        .WithMany("ReceivedOrders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AllNewsServer.Data.Models.DeliveryType", "DeliveryType")
                        .WithMany("Orders")
                        .HasForeignKey("DeliveryTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AllNewsServer.Data.Models.ApplicationUser", "Executor")
                        .WithMany("DeliveredOrders")
                        .HasForeignKey("ExecutorId");

                    b.HasOne("AllNewsServer.Data.Models.OrderStatus", "OrderStatus")
                        .WithMany("Orders")
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.OrderProduct", b =>
                {
                    b.HasOne("AllNewsServer.Data.Models.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AllNewsServer.Data.Models.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AllNewsServer.Data.Models.Product", b =>
                {
                    b.HasOne("AllNewsServer.Data.Models.Currency", "Currency")
                        .WithMany("Products")
                        .HasForeignKey("CurrencyId");

                    b.HasOne("AllNewsServer.Data.Models.ProductType", "ProductType")
                        .WithMany("Products")
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AllNewsServer.Data.Models.SizeUnit", "SizeUnit")
                        .WithMany("Products")
                        .HasForeignKey("SizeUnitId");

                    b.HasOne("AllNewsServer.Data.Models.VolumeUnit", "VolumeUnit")
                        .WithMany("Products")
                        .HasForeignKey("VolumeUnitId");

                    b.HasOne("AllNewsServer.Data.Models.WeightUnit", "WeightUnit")
                        .WithMany("Products")
                        .HasForeignKey("WeightUnitId")
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
