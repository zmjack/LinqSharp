using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202208301442 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DictionaryModel",
                table: "LS_Providers",
                maxLength: 127,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DictionaryModel",
                table: "LS_Providers");
        }
    }
}
