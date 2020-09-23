using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestDatabaseCreator.Migrations
{
    public partial class _202009231358 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LS_Names",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LS_Names", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.EmployeeTerritories_EmployeeID",
                table: "@Northwnd.EmployeeTerritories",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.CustomerCustomerDemos_CustomerTypeID",
                table: "@Northwnd.CustomerCustomerDemos",
                column: "CustomerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_LS_Names_Name",
                table: "LS_Names",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LS_Names");

            migrationBuilder.DropIndex(
                name: "IX_@Northwnd.EmployeeTerritories_EmployeeID",
                table: "@Northwnd.EmployeeTerritories");

            migrationBuilder.DropIndex(
                name: "IX_@Northwnd.CustomerCustomerDemos_CustomerTypeID",
                table: "@Northwnd.CustomerCustomerDemos");
        }
    }
}
