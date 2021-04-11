using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202104081105 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BulkTestModels_Code",
                table: "BulkTestModels");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "BulkTestModels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "BulkTestModels",
                type: "varchar(255) CHARACTER SET utf8mb4",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BulkTestModels_Code",
                table: "BulkTestModels",
                column: "Code",
                unique: true);
        }
    }
}
