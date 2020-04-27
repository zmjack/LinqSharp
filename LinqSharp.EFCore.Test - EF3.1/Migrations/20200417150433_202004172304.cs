using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinqSharp.Test.Migrations
{
    public partial class _202004172304 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "EntityMonitorModels");

            migrationBuilder.AddColumn<string>(
                name: "ChangeValues",
                table: "EntityMonitorModels",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "EntityMonitorModels",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Event",
                table: "EntityMonitorModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "EntityMonitorModels",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "EntityMonitorModels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeValues",
                table: "EntityMonitorModels");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "EntityMonitorModels");

            migrationBuilder.DropColumn(
                name: "Event",
                table: "EntityMonitorModels");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "EntityMonitorModels");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "EntityMonitorModels");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "EntityMonitorModels",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
