using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202205070300 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConcurrencyModels_RandomNumber",
                table: "ConcurrencyModels");

            migrationBuilder.DropColumn(
                name: "CheckClientWin",
                table: "ConcurrencyModels");

            migrationBuilder.DropColumn(
                name: "CheckCombine",
                table: "ConcurrencyModels");

            migrationBuilder.DropColumn(
                name: "CheckDefault",
                table: "ConcurrencyModels");

            migrationBuilder.DropColumn(
                name: "CheckStoreWin",
                table: "ConcurrencyModels");

            migrationBuilder.DropColumn(
                name: "CheckThrow",
                table: "ConcurrencyModels");

            migrationBuilder.DropColumn(
                name: "RandomNumber",
                table: "ConcurrencyModels");

            migrationBuilder.AddColumn<int>(
                name: "ClientWinValue",
                table: "ConcurrencyModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConcurrencyCheck",
                table: "ConcurrencyModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DatabaseWinValue",
                table: "ConcurrencyModels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "ConcurrencyModels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientWinValue",
                table: "ConcurrencyModels");

            migrationBuilder.DropColumn(
                name: "ConcurrencyCheck",
                table: "ConcurrencyModels");

            migrationBuilder.DropColumn(
                name: "DatabaseWinValue",
                table: "ConcurrencyModels");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "ConcurrencyModels");

            migrationBuilder.AddColumn<int>(
                name: "CheckClientWin",
                table: "ConcurrencyModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CheckCombine",
                table: "ConcurrencyModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CheckDefault",
                table: "ConcurrencyModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CheckStoreWin",
                table: "ConcurrencyModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CheckThrow",
                table: "ConcurrencyModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RandomNumber",
                table: "ConcurrencyModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ConcurrencyModels_RandomNumber",
                table: "ConcurrencyModels",
                column: "RandomNumber",
                unique: true);
        }
    }
}
