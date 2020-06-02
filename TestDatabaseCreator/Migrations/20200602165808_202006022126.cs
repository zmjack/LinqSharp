using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestDatabaseCreator.Migrations
{
    public partial class _202006022126 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityTrackModel3s");

            migrationBuilder.DropTable(
                name: "EntityTrackModel2s");

            migrationBuilder.DropTable(
                name: "EntityTrackModel1s");

            migrationBuilder.CreateTable(
                name: "AuditRoots",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false),
                    LimitQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRoots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Root = table.Column<Guid>(nullable: false),
                    ValueCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLevels_AuditRoots_Root",
                        column: x => x.Root,
                        principalTable: "AuditRoots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Level = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditValues_AuditLevels_Level",
                        column: x => x.Level,
                        principalTable: "AuditLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLevels_Root",
                table: "AuditLevels",
                column: "Root");

            migrationBuilder.CreateIndex(
                name: "IX_AuditValues_Level",
                table: "AuditValues",
                column: "Level");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditValues");

            migrationBuilder.DropTable(
                name: "AuditLevels");

            migrationBuilder.DropTable(
                name: "AuditRoots");

            migrationBuilder.CreateTable(
                name: "EntityTrackModel1s",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityTrackModel1s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityTrackModel2s",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    GroupQuantity = table.Column<int>(type: "int", nullable: false),
                    Super = table.Column<Guid>(type: "char(36)", nullable: false)
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
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Super = table.Column<Guid>(type: "char(36)", nullable: false)
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
    }
}
