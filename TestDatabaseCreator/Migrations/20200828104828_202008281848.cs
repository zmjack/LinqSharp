using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestDatabaseCreator.Migrations
{
    public partial class _202008281848 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SimpleModel",
                table: "LS_Providers");

            migrationBuilder.AddColumn<string>(
                name: "NameModel",
                table: "LS_Providers",
                maxLength: 127,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "M_Names",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_M_Names", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "M_Names");

            migrationBuilder.DropColumn(
                name: "NameModel",
                table: "LS_Providers");

            migrationBuilder.AddColumn<string>(
                name: "SimpleModel",
                table: "LS_Providers",
                type: "varchar(127) CHARACTER SET utf8mb4",
                maxLength: 127,
                nullable: true);
        }
    }
}
