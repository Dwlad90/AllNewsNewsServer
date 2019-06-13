using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace AllNewsServer.Migrations
{
    public partial class OrderLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AddColumn<Point>(
                name: "EndLocation",
                table: "Orders",
                type: "geography",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectingDateTime",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishingTime",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Point>(
                name: "StartLocation",
                table: "Orders",
                type: "geography",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TakingDateTime",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndLocation",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExpectingDateTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FinishingTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StartLocation",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TakingDateTime",
                table: "Orders");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");
        }
    }
}
