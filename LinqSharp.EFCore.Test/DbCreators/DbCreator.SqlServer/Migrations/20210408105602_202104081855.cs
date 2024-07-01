using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202104081855 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BulkTestModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UniqueCode = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkTestModels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BulkTestModels_UniqueCode",
                table: "BulkTestModels",
                column: "UniqueCode",
                unique: true,
                filter: "[UniqueCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulkTestModels");
        }
    }
}
