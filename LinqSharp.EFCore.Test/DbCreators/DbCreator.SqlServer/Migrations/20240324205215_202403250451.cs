using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.Migrations
{
    /// <inheritdoc />
    public partial class _202403250451 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateLock",
                table: "UpdateLockModels");

            migrationBuilder.AddColumn<DateTime>(
                name: "LockDate",
                table: "UpdateLockModels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "UpdateLockModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockDate",
                table: "UpdateLockModels");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "UpdateLockModels");

            migrationBuilder.AddColumn<bool>(
                name: "UpdateLock",
                table: "UpdateLockModels",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
