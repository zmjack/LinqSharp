using Microsoft.EntityFrameworkCore.Migrations;

namespace TestDatabaseCreator.Migrations
{
    public partial class _202006302300 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Day",
                table: "YearMonthModels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "YearMonthModels");
        }
    }
}
