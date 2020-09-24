using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestDatabaseCreator.Migrations
{
    public partial class _202009241440 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LS_Names_Name",
                table: "LS_Names");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "LS_Names",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_LS_Names_Name_CreationTime",
                table: "LS_Names",
                columns: new[] { "Name", "CreationTime" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LS_Names_Name_CreationTime",
                table: "LS_Names");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "LS_Names");

            migrationBuilder.CreateIndex(
                name: "IX_LS_Names_Name",
                table: "LS_Names",
                column: "Name",
                unique: true);
        }
    }
}
