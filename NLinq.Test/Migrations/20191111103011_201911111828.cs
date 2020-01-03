using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace NLinq.Test.Migrations
{
    public partial class _201911111828 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityTrackModel1s",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityTrackModel1s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityTrackModel2s",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Super = table.Column<Guid>(nullable: false),
                    GroupQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityTrackModel2s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityTrackModel2s_EntityTrackModel1s_Super",
                        column: x => x.Super,
                        principalTable: "EntityTrackModel1s",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityTrackModel3s",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Super = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityTrackModel3s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityTrackModel3s_EntityTrackModel2s_Super",
                        column: x => x.Super,
                        principalTable: "EntityTrackModel2s",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityTrackModel2s_Super",
                table: "EntityTrackModel2s",
                column: "Super");

            migrationBuilder.CreateIndex(
                name: "IX_EntityTrackModel3s_Super",
                table: "EntityTrackModel3s",
                column: "Super");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityTrackModel3s");

            migrationBuilder.DropTable(
                name: "EntityTrackModel2s");

            migrationBuilder.DropTable(
                name: "EntityTrackModel1s");
        }
    }
}
