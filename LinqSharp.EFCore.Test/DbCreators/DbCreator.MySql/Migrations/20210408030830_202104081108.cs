using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202104081108 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueCode",
                table: "BulkTestModels",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BulkTestModels_UniqueCode",
                table: "BulkTestModels",
                column: "UniqueCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BulkTestModels_UniqueCode",
                table: "BulkTestModels");

            migrationBuilder.DropColumn(
                name: "UniqueCode",
                table: "BulkTestModels");
        }
    }
}
