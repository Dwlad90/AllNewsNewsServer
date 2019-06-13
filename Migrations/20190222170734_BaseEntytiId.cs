using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AllNewsServer.Migrations
{
    public partial class BaseEntytiId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateTime",
                table: "OrderStatuses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreationUser",
                table: "OrderStatuses",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OrderStatuses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDateTime",
                table: "OrderStatuses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModificationUser",
                table: "OrderStatuses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateTime",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "CreationUser",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "ModificationDateTime",
                table: "OrderStatuses");

            migrationBuilder.DropColumn(
                name: "ModificationUser",
                table: "OrderStatuses");
        }
    }
}
