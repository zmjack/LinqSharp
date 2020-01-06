using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LinqSharp.Test.Migrations
{
    public partial class _201909120607 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompositeKeyModels",
                columns: table => new
                {
                    Id1 = table.Column<Guid>(nullable: false),
                    Id2 = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompositeKeyModels", x => new { x.Id1, x.Id2 });
                });

            migrationBuilder.CreateTable(
                name: "EntityMonitorModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityMonitorModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NickName = table.Column<string>(nullable: true),
                    RealName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastWriteTime = table.Column<DateTime>(nullable: false),
                    ForTrim = table.Column<string>(nullable: true),
                    ForUpper = table.Column<string>(nullable: true),
                    ForLower = table.Column<string>(nullable: true),
                    ForCondensed = table.Column<string>(nullable: true),
                    Automatic = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackModels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompositeKeyModels");

            migrationBuilder.DropTable(
                name: "EntityMonitorModels");

            migrationBuilder.DropTable(
                name: "SimpleModels");

            migrationBuilder.DropTable(
                name: "TrackModels");
        }
    }
}
