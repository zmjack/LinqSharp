using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestDatabaseCreator.Migrations
{
    public partial class _202008281733 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LS_Indices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Int0 = table.Column<int>(nullable: false),
                    Int1 = table.Column<int>(nullable: false),
                    Int2_G1 = table.Column<int>(nullable: false),
                    Int3_G1 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LS_Indices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LS_Indices_Int0",
                table: "LS_Indices",
                column: "Int0");

            migrationBuilder.CreateIndex(
                name: "IX_LS_Indices_Int1",
                table: "LS_Indices",
                column: "Int1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LS_Indices_Int2_G1_Int3_G1",
                table: "LS_Indices",
                columns: new[] { "Int2_G1", "Int3_G1" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LS_Indices");
        }
    }
}
