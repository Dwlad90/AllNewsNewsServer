using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AllNewsServer.Migrations
{
    public partial class Product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductTypeId",
                table: "LocalizationResources",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VolumeUnitId",
                table: "LocalizationResources",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeightUnitId",
                table: "LocalizationResources",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    CreationUser = table.Column<string>(nullable: true),
                    ModificationDateTime = table.Column<DateTime>(nullable: false),
                    ModificationUser = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Key = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VolumeUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    CreationUser = table.Column<string>(nullable: true),
                    ModificationDateTime = table.Column<DateTime>(nullable: false),
                    ModificationUser = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Key = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolumeUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeightUnits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    CreationUser = table.Column<string>(nullable: true),
                    ModificationDateTime = table.Column<DateTime>(nullable: false),
                    ModificationUser = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Key = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreationDateTime = table.Column<DateTime>(nullable: false),
                    CreationUser = table.Column<string>(nullable: true),
                    ModificationDateTime = table.Column<DateTime>(nullable: false),
                    ModificationUser = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ProductTypeId = table.Column<int>(nullable: false),
                    Weight = table.Column<int>(nullable: false),
                    WeightUnitId = table.Column<int>(nullable: false),
                    Volume = table.Column<int>(nullable: true),
                    VolumeUnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_VolumeUnits_VolumeUnitId",
                        column: x => x.VolumeUnitId,
                        principalTable: "VolumeUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_WeightUnits_WeightUnitId",
                        column: x => x.WeightUnitId,
                        principalTable: "WeightUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_ProductTypeId",
                table: "LocalizationResources",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_VolumeUnitId",
                table: "LocalizationResources",
                column: "VolumeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_WeightUnitId",
                table: "LocalizationResources",
                column: "WeightUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VolumeUnitId",
                table: "Products",
                column: "VolumeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_WeightUnitId",
                table: "Products",
                column: "WeightUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_Key",
                table: "ProductTypes",
                column: "Key",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizationResources_ProductTypes_ProductTypeId",
                table: "LocalizationResources",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizationResources_VolumeUnits_VolumeUnitId",
                table: "LocalizationResources",
                column: "VolumeUnitId",
                principalTable: "VolumeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizationResources_WeightUnits_WeightUnitId",
                table: "LocalizationResources",
                column: "WeightUnitId",
                principalTable: "WeightUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalizationResources_ProductTypes_ProductTypeId",
                table: "LocalizationResources");

            migrationBuilder.DropForeignKey(
                name: "FK_LocalizationResources_VolumeUnits_VolumeUnitId",
                table: "LocalizationResources");

            migrationBuilder.DropForeignKey(
                name: "FK_LocalizationResources_WeightUnits_WeightUnitId",
                table: "LocalizationResources");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "VolumeUnits");

            migrationBuilder.DropTable(
                name: "WeightUnits");

            migrationBuilder.DropIndex(
                name: "IX_LocalizationResources_ProductTypeId",
                table: "LocalizationResources");

            migrationBuilder.DropIndex(
                name: "IX_LocalizationResources_VolumeUnitId",
                table: "LocalizationResources");

            migrationBuilder.DropIndex(
                name: "IX_LocalizationResources_WeightUnitId",
                table: "LocalizationResources");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "LocalizationResources");

            migrationBuilder.DropColumn(
                name: "VolumeUnitId",
                table: "LocalizationResources");

            migrationBuilder.DropColumn(
                name: "WeightUnitId",
                table: "LocalizationResources");
        }
    }
}
