using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DbCreator.Migrations
{
    public partial class _202105032121 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SimpleRows",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Group_Name = table.Column<string>(maxLength: 255, nullable: true),
                    Group_Age = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleRows", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimpleRows");
        }
    }
}
