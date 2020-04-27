using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinqSharp.Test.Migrations
{
    public partial class _202004241422 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreeModels");

            migrationBuilder.DropColumn(
                name: "FreeModel",
                table: "ProviderTestModels");

            migrationBuilder.AddColumn<bool>(
                name: "CheckClientWin",
                table: "SimpleModels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckCombine",
                table: "SimpleModels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckDefault",
                table: "SimpleModels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckStoreWin",
                table: "SimpleModels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CheckThrow",
                table: "SimpleModels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "SimpleModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SimpleModel",
                table: "ProviderTestModels",
                maxLength: 127,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckClientWin",
                table: "SimpleModels");

            migrationBuilder.DropColumn(
                name: "CheckCombine",
                table: "SimpleModels");

            migrationBuilder.DropColumn(
                name: "CheckDefault",
                table: "SimpleModels");

            migrationBuilder.DropColumn(
                name: "CheckStoreWin",
                table: "SimpleModels");

            migrationBuilder.DropColumn(
                name: "CheckThrow",
                table: "SimpleModels");

            migrationBuilder.DropColumn(
                name: "State",
                table: "SimpleModels");

            migrationBuilder.DropColumn(
                name: "SimpleModel",
                table: "ProviderTestModels");

            migrationBuilder.AddColumn<string>(
                name: "FreeModel",
                table: "ProviderTestModels",
                type: "varchar(127) CHARACTER SET utf8mb4",
                maxLength: 127,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FreeModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeModels", x => x.Id);
                });
        }
    }
}
