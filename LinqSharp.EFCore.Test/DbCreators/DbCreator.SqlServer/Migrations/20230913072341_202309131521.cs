using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.Migrations
{
    /// <inheritdoc />
    public partial class _202309131521 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TerritoryID",
                table: "@Northwnd.Employees",
                type: "nvarchar(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerDemographicCustomerTypeID",
                table: "@Northwnd.Customers",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.Employees_TerritoryID",
                table: "@Northwnd.Employees",
                column: "TerritoryID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.Customers_CustomerDemographicCustomerTypeID",
                table: "@Northwnd.Customers",
                column: "CustomerDemographicCustomerTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_@Northwnd.Customers_@Northwnd.CustomerDemographics_CustomerDemographicCustomerTypeID",
                table: "@Northwnd.Customers",
                column: "CustomerDemographicCustomerTypeID",
                principalTable: "@Northwnd.CustomerDemographics",
                principalColumn: "CustomerTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_@Northwnd.Employees_@Northwnd.Territories_TerritoryID",
                table: "@Northwnd.Employees",
                column: "TerritoryID",
                principalTable: "@Northwnd.Territories",
                principalColumn: "TerritoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_@Northwnd.Customers_@Northwnd.CustomerDemographics_CustomerDemographicCustomerTypeID",
                table: "@Northwnd.Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_@Northwnd.Employees_@Northwnd.Territories_TerritoryID",
                table: "@Northwnd.Employees");

            migrationBuilder.DropIndex(
                name: "IX_@Northwnd.Employees_TerritoryID",
                table: "@Northwnd.Employees");

            migrationBuilder.DropIndex(
                name: "IX_@Northwnd.Customers_CustomerDemographicCustomerTypeID",
                table: "@Northwnd.Customers");

            migrationBuilder.DropColumn(
                name: "TerritoryID",
                table: "@Northwnd.Employees");

            migrationBuilder.DropColumn(
                name: "CustomerDemographicCustomerTypeID",
                table: "@Northwnd.Customers");
        }
    }
}
