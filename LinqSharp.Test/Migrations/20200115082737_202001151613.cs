using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LinqSharp.Test.Migrations
{
    public partial class _202001151613 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProviderTestModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Password = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderTestModels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderTestModels");
        }
    }
}
