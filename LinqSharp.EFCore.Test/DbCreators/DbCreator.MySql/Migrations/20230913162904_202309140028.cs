using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.Migrations
{
    /// <inheritdoc />
    public partial class _202309140028 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "@Northwnd.CustomerCustomerDemos");

            migrationBuilder.DropTable(
                name: "@Northwnd.EmployeeTerritories");

            migrationBuilder.DropTable(
                name: "@Northwnd.OrderDetails");

            migrationBuilder.DropTable(
                name: "CustomerCustomerDemographic");

            migrationBuilder.DropTable(
                name: "EmployeeTerritory");

            migrationBuilder.DropTable(
                name: "@Northwnd.Orders");

            migrationBuilder.DropTable(
                name: "@Northwnd.Products");

            migrationBuilder.DropTable(
                name: "@Northwnd.CustomerDemographics");

            migrationBuilder.DropTable(
                name: "@Northwnd.Territories");

            migrationBuilder.DropTable(
                name: "@Northwnd.Customers");

            migrationBuilder.DropTable(
                name: "@Northwnd.Employees");

            migrationBuilder.DropTable(
                name: "@Northwnd.Shippers");

            migrationBuilder.DropTable(
                name: "@Northwnd.Categories");

            migrationBuilder.DropTable(
                name: "@Northwnd.Suppliers");

            migrationBuilder.DropTable(
                name: "@Northwnd.Regions");

            migrationBuilder.CreateTable(
                name: "@n.Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Picture = table.Column<byte[]>(type: "longblob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Categories", x => x.CategoryID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.CustomerDemographics",
                columns: table => new
                {
                    CustomerTypeID = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerDesc = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.CustomerDemographics", x => x.CustomerTypeID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.Customers",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactTitle = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Region = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fax = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Customers", x => x.CustomerID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    LastName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TitleOfCourtesy = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BirthDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Address = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Region = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HomePhone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Extension = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Photo = table.Column<byte[]>(type: "longblob", nullable: true),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReportsTo = table.Column<int>(type: "int", nullable: true),
                    PhotoPath = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_@n.Employees_@n.Employees_ReportsTo",
                        column: x => x.ReportsTo,
                        principalTable: "@n.Employees",
                        principalColumn: "EmployeeID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.Regions",
                columns: table => new
                {
                    RegionID = table.Column<int>(type: "int", nullable: false),
                    RegionDescription = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Regions", x => x.RegionID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.Shippers",
                columns: table => new
                {
                    ShipperID = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Shippers", x => x.ShipperID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactTitle = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Region = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fax = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HomePage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Suppliers", x => x.SupplierID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.CustomerCustomerDemos",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerTypeID = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.CustomerCustomerDemos", x => new { x.CustomerTypeID, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_@n.CustomerCustomerDemos_@n.CustomerDemographics_CustomerTyp~",
                        column: x => x.CustomerTypeID,
                        principalTable: "@n.CustomerDemographics",
                        principalColumn: "CustomerTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_@n.CustomerCustomerDemos_@n.Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "@n.Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.Territories",
                columns: table => new
                {
                    TerritoryID = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TerritoryDescription = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Territories", x => x.TerritoryID);
                    table.ForeignKey(
                        name: "FK_@n.Territories_@n.Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "@n.Regions",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeID = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RequiredDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ShipVia = table.Column<int>(type: "int", nullable: true),
                    Freight = table.Column<double>(type: "double", nullable: true),
                    ShipName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipAddress = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipCity = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipRegion = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipPostalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipCountry = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_@n.Orders_@n.Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "@n.Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_@n.Orders_@n.Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "@n.Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_@n.Orders_@n.Shippers_ShipVia",
                        column: x => x.ShipVia,
                        principalTable: "@n.Shippers",
                        principalColumn: "ShipperID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupplierID = table.Column<int>(type: "int", nullable: true),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    QuantityPerUnit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnitPrice = table.Column<double>(type: "double", nullable: true),
                    UnitsInStock = table.Column<short>(type: "smallint", nullable: true),
                    UnitsOnOrder = table.Column<short>(type: "smallint", nullable: true),
                    ReorderLevel = table.Column<short>(type: "smallint", nullable: true),
                    Discontinued = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_@n.Products_@n.Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "@n.Categories",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_@n.Products_@n.Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "@n.Suppliers",
                        principalColumn: "SupplierID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.EmployeeTerritories",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    TerritoryID = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.EmployeeTerritories", x => new { x.EmployeeID, x.TerritoryID });
                    table.ForeignKey(
                        name: "FK_@n.EmployeeTerritories_@n.Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "@n.Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_@n.EmployeeTerritories_@n.Territories_TerritoryID",
                        column: x => x.TerritoryID,
                        principalTable: "@n.Territories",
                        principalColumn: "TerritoryID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@n.OrderDetails",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "double", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    Discount = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.OrderDetails", x => new { x.OrderID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_@n.OrderDetails_@n.Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "@n.Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_@n.OrderDetails_@n.Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "@n.Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_@n.CustomerCustomerDemos_CustomerID",
                table: "@n.CustomerCustomerDemos",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.CustomerCustomerDemos_CustomerTypeID",
                table: "@n.CustomerCustomerDemos",
                column: "CustomerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.Employees_ReportsTo",
                table: "@n.Employees",
                column: "ReportsTo");

            migrationBuilder.CreateIndex(
                name: "IX_@n.EmployeeTerritories_EmployeeID",
                table: "@n.EmployeeTerritories",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.EmployeeTerritories_TerritoryID",
                table: "@n.EmployeeTerritories",
                column: "TerritoryID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.OrderDetails_OrderID",
                table: "@n.OrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.OrderDetails_ProductID",
                table: "@n.OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.Orders_CustomerID",
                table: "@n.Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.Orders_EmployeeID",
                table: "@n.Orders",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.Orders_ShipVia",
                table: "@n.Orders",
                column: "ShipVia");

            migrationBuilder.CreateIndex(
                name: "IX_@n.Products_CategoryID",
                table: "@n.Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.Products_SupplierID",
                table: "@n.Products",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_@n.Territories_RegionID",
                table: "@n.Territories",
                column: "RegionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "@n.CustomerCustomerDemos");

            migrationBuilder.DropTable(
                name: "@n.EmployeeTerritories");

            migrationBuilder.DropTable(
                name: "@n.OrderDetails");

            migrationBuilder.DropTable(
                name: "@n.CustomerDemographics");

            migrationBuilder.DropTable(
                name: "@n.Territories");

            migrationBuilder.DropTable(
                name: "@n.Orders");

            migrationBuilder.DropTable(
                name: "@n.Products");

            migrationBuilder.DropTable(
                name: "@n.Regions");

            migrationBuilder.DropTable(
                name: "@n.Customers");

            migrationBuilder.DropTable(
                name: "@n.Employees");

            migrationBuilder.DropTable(
                name: "@n.Shippers");

            migrationBuilder.DropTable(
                name: "@n.Categories");

            migrationBuilder.DropTable(
                name: "@n.Suppliers");

            migrationBuilder.CreateTable(
                name: "@Northwnd.Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Picture = table.Column<byte[]>(type: "longblob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Categories", x => x.CategoryID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.CustomerDemographics",
                columns: table => new
                {
                    CustomerTypeID = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerDesc = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.CustomerDemographics", x => x.CustomerTypeID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.Customers",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactTitle = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fax = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Region = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Customers", x => x.CustomerID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    ReportsTo = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BirthDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    City = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Extension = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HireDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HomePhone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Photo = table.Column<byte[]>(type: "longblob", nullable: true),
                    PhotoPath = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Region = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TitleOfCourtesy = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Employees_@Northwnd.Employees_ReportsTo",
                        column: x => x.ReportsTo,
                        principalTable: "@Northwnd.Employees",
                        principalColumn: "EmployeeID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.Regions",
                columns: table => new
                {
                    RegionID = table.Column<int>(type: "int", nullable: false),
                    RegionDescription = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Regions", x => x.RegionID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.Shippers",
                columns: table => new
                {
                    ShipperID = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Shippers", x => x.ShipperID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactTitle = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fax = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HomePage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Region = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Suppliers", x => x.SupplierID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.CustomerCustomerDemos",
                columns: table => new
                {
                    CustomerTypeID = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerID = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.CustomerCustomerDemos", x => new { x.CustomerTypeID, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_@Northwnd.CustomerCustomerDemos_@Northwnd.CustomerDemographi~",
                        column: x => x.CustomerTypeID,
                        principalTable: "@Northwnd.CustomerDemographics",
                        principalColumn: "CustomerTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_@Northwnd.CustomerCustomerDemos_@Northwnd.Customers_Customer~",
                        column: x => x.CustomerID,
                        principalTable: "@Northwnd.Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomerCustomerDemographic",
                columns: table => new
                {
                    CustomerDemographicsCustomerTypeID = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomersCustomerID = table.Column<string>(type: "varchar(5)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCustomerDemographic", x => new { x.CustomerDemographicsCustomerTypeID, x.CustomersCustomerID });
                    table.ForeignKey(
                        name: "FK_CustomerCustomerDemographic_@Northwnd.CustomerDemographics_C~",
                        column: x => x.CustomerDemographicsCustomerTypeID,
                        principalTable: "@Northwnd.CustomerDemographics",
                        principalColumn: "CustomerTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerCustomerDemographic_@Northwnd.Customers_CustomersCus~",
                        column: x => x.CustomersCustomerID,
                        principalTable: "@Northwnd.Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.Territories",
                columns: table => new
                {
                    TerritoryID = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegionID = table.Column<int>(type: "int", nullable: false),
                    TerritoryDescription = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Territories", x => x.TerritoryID);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Territories_@Northwnd.Regions_RegionID",
                        column: x => x.RegionID,
                        principalTable: "@Northwnd.Regions",
                        principalColumn: "RegionID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeID = table.Column<int>(type: "int", nullable: true),
                    ShipVia = table.Column<int>(type: "int", nullable: true),
                    Freight = table.Column<double>(type: "double", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RequiredDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ShipAddress = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipCity = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipCountry = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipPostalCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipRegion = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShippedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Orders_@Northwnd.Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "@Northwnd.Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_@Northwnd.Orders_@Northwnd.Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "@Northwnd.Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_@Northwnd.Orders_@Northwnd.Shippers_ShipVia",
                        column: x => x.ShipVia,
                        principalTable: "@Northwnd.Shippers",
                        principalColumn: "ShipperID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    SupplierID = table.Column<int>(type: "int", nullable: true),
                    Discontinued = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ProductName = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QuantityPerUnit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReorderLevel = table.Column<short>(type: "smallint", nullable: true),
                    UnitPrice = table.Column<double>(type: "double", nullable: true),
                    UnitsInStock = table.Column<short>(type: "smallint", nullable: true),
                    UnitsOnOrder = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Products_@Northwnd.Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "@Northwnd.Categories",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_@Northwnd.Products_@Northwnd.Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "@Northwnd.Suppliers",
                        principalColumn: "SupplierID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.EmployeeTerritories",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    TerritoryID = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.EmployeeTerritories", x => new { x.EmployeeID, x.TerritoryID });
                    table.ForeignKey(
                        name: "FK_@Northwnd.EmployeeTerritories_@Northwnd.Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "@Northwnd.Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_@Northwnd.EmployeeTerritories_@Northwnd.Territories_Territor~",
                        column: x => x.TerritoryID,
                        principalTable: "@Northwnd.Territories",
                        principalColumn: "TerritoryID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmployeeTerritory",
                columns: table => new
                {
                    EmployeesEmployeeID = table.Column<int>(type: "int", nullable: false),
                    TerritoriesTerritoryID = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTerritory", x => new { x.EmployeesEmployeeID, x.TerritoriesTerritoryID });
                    table.ForeignKey(
                        name: "FK_EmployeeTerritory_@Northwnd.Employees_EmployeesEmployeeID",
                        column: x => x.EmployeesEmployeeID,
                        principalTable: "@Northwnd.Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeTerritory_@Northwnd.Territories_TerritoriesTerritory~",
                        column: x => x.TerritoriesTerritoryID,
                        principalTable: "@Northwnd.Territories",
                        principalColumn: "TerritoryID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "@Northwnd.OrderDetails",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<float>(type: "float", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    UnitPrice = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.OrderDetails", x => new { x.OrderID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_@Northwnd.OrderDetails_@Northwnd.Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "@Northwnd.Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_@Northwnd.OrderDetails_@Northwnd.Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "@Northwnd.Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.CustomerCustomerDemos_CustomerID",
                table: "@Northwnd.CustomerCustomerDemos",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.CustomerCustomerDemos_CustomerTypeID",
                table: "@Northwnd.CustomerCustomerDemos",
                column: "CustomerTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.Employees_ReportsTo",
                table: "@Northwnd.Employees",
                column: "ReportsTo");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.EmployeeTerritories_EmployeeID",
                table: "@Northwnd.EmployeeTerritories",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.EmployeeTerritories_TerritoryID",
                table: "@Northwnd.EmployeeTerritories",
                column: "TerritoryID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.OrderDetails_OrderID",
                table: "@Northwnd.OrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.OrderDetails_ProductID",
                table: "@Northwnd.OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.Orders_CustomerID",
                table: "@Northwnd.Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.Orders_EmployeeID",
                table: "@Northwnd.Orders",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.Orders_ShipVia",
                table: "@Northwnd.Orders",
                column: "ShipVia");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.Products_CategoryID",
                table: "@Northwnd.Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.Products_SupplierID",
                table: "@Northwnd.Products",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_@Northwnd.Territories_RegionID",
                table: "@Northwnd.Territories",
                column: "RegionID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCustomerDemographic_CustomersCustomerID",
                table: "CustomerCustomerDemographic",
                column: "CustomersCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTerritory_TerritoriesTerritoryID",
                table: "EmployeeTerritory",
                column: "TerritoriesTerritoryID");
        }
    }
}
