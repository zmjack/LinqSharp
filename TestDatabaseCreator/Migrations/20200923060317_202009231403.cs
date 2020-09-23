using Microsoft.EntityFrameworkCore.Migrations;

namespace TestDatabaseCreator.Migrations
{
    public partial class _202009231403 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "LS_Names",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "LS_Names");
        }
    }
}
