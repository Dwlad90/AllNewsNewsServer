using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AllNewsServer.Migrations
{
    public partial class ProductSizeUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Depth",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeUnitId",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SizeUnitId",
                table: "LocalizationResources",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SizeUnits",
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
                    table.PrimaryKey("PK_SizeUnits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SizeUnitId",
                table: "Products",
                column: "SizeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationResources_SizeUnitId",
                table: "LocalizationResources",
                column: "SizeUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizationResources_SizeUnits_SizeUnitId",
                table: "LocalizationResources",
                column: "SizeUnitId",
                principalTable: "SizeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SizeUnits_SizeUnitId",
                table: "Products",
                column: "SizeUnitId",
                principalTable: "SizeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalizationResources_SizeUnits_SizeUnitId",
                table: "LocalizationResources");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_SizeUnits_SizeUnitId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "SizeUnits");

            migrationBuilder.DropIndex(
                name: "IX_Products_SizeUnitId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_LocalizationResources_SizeUnitId",
                table: "LocalizationResources");

            migrationBuilder.DropColumn(
                name: "Depth",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SizeUnitId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SizeUnitId",
                table: "LocalizationResources");
        }
    }
}
