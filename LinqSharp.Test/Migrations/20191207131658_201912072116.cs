using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LinqSharp.Test.Migrations
{
    public partial class _201912072116 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRegistries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Item = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRegistries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppRegistries_Item_Key",
                table: "AppRegistries",
                columns: new[] { "Item", "Key" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRegistries");
        }
    }
}
