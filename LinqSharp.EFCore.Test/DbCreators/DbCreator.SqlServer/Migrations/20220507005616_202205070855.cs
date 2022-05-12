using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202205070855 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyCheck",
                table: "ConcurrencyModels");

            migrationBuilder.AddColumn<int>(
                name: "RowVersion",
                table: "ConcurrencyModels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ConcurrencyModels");

            migrationBuilder.AddColumn<int>(
                name: "ConcurrencyCheck",
                table: "ConcurrencyModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
