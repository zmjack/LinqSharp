using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbCreator.Migrations
{
    public partial class _202010151800 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "@Northwnd.Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(maxLength: 15, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Picture = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.CustomerDemographics",
                columns: table => new
                {
                    CustomerTypeID = table.Column<string>(maxLength: 10, nullable: false),
                    CustomerDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.CustomerDemographics", x => x.CustomerTypeID);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.Customers",
                columns: table => new
                {
                    CustomerID = table.Column<string>(maxLength: 5, nullable: false),
                    CompanyName = table.Column<string>(maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(maxLength: 30, nullable: true),
                    Address = table.Column<string>(maxLength: 60, nullable: true),
                    City = table.Column<string>(maxLength: 15, nullable: true),
                    Region = table.Column<string>(maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Country = table.Column<string>(maxLength: 15, nullable: true),
                    Phone = table.Column<string>(maxLength: 24, nullable: true),
                    Fax = table.Column<string>(maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(maxLength: 10, nullable: false),
                    Title = table.Column<string>(maxLength: 30, nullable: true),
                    TitleOfCourtesy = table.Column<string>(maxLength: 25, nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    HireDate = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(maxLength: 60, nullable: true),
                    City = table.Column<string>(maxLength: 15, nullable: true),
                    Region = table.Column<string>(maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Country = table.Column<string>(maxLength: 15, nullable: true),
                    HomePhone = table.Column<string>(maxLength: 24, nullable: true),
                    Extension = table.Column<string>(maxLength: 4, nullable: true),
                    Photo = table.Column<byte[]>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    ReportsTo = table.Column<int>(nullable: true),
                    PhotoPath = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Employees_@Northwnd.Employees_ReportsTo",
                        column: x => x.ReportsTo,
                        principalTable: "@Northwnd.Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.Regions",
                columns: table => new
                {
                    RegionID = table.Column<int>(nullable: false),
                    RegionDescription = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Regions", x => x.RegionID);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.Shippers",
                columns: table => new
                {
                    ShipperID = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 40, nullable: false),
                    Phone = table.Column<string>(maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Shippers", x => x.ShipperID);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(maxLength: 30, nullable: true),
                    Address = table.Column<string>(maxLength: 60, nullable: true),
                    City = table.Column<string>(maxLength: 15, nullable: true),
                    Region = table.Column<string>(maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Country = table.Column<string>(maxLength: 15, nullable: true),
                    Phone = table.Column<string>(maxLength: 24, nullable: true),
                    Fax = table.Column<string>(maxLength: 24, nullable: true),
                    HomePage = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "AppRegistries",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Item = table.Column<string>(maxLength: 127, nullable: true),
                    Key = table.Column<string>(maxLength: 127, nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRegistries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditRoots",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TotalQuantity = table.Column<int>(nullable: false),
                    LimitQuantity = table.Column<int>(nullable: false),
                    RowVersion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRoots", x => x.Id);
                });

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
                name: "ConcurrencyModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RandomNumber = table.Column<int>(nullable: false),
                    CheckDefault = table.Column<int>(nullable: false),
                    CheckThrow = table.Column<int>(nullable: false),
                    CheckStoreWin = table.Column<int>(nullable: false),
                    CheckClientWin = table.Column<int>(nullable: false),
                    CheckCombine = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcurrencyModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityMonitorModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Event = table.Column<string>(nullable: true),
                    TypeName = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    ChangeValues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityMonitorModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LS_Indices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Int0 = table.Column<int>(nullable: false),
                    Int1 = table.Column<int>(nullable: false),
                    Int2_G1 = table.Column<int>(nullable: false),
                    Int3_G1 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LS_Indices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LS_Names",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Note = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LS_Names", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LS_Providers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Password = table.Column<string>(maxLength: 127, nullable: true),
                    NameModel = table.Column<string>(maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LS_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false)
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
                    ForCondensed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YearMonthModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearMonthModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.CustomerCustomerDemos",
                columns: table => new
                {
                    CustomerID = table.Column<string>(maxLength: 5, nullable: false),
                    CustomerTypeID = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.CustomerCustomerDemos", x => new { x.CustomerTypeID, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_@Northwnd.CustomerCustomerDemos_@Northwnd.Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "@Northwnd.Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_@Northwnd.CustomerCustomerDemos_@Northwnd.CustomerDemographics_CustomerTypeID",
                        column: x => x.CustomerTypeID,
                        principalTable: "@Northwnd.CustomerDemographics",
                        principalColumn: "CustomerTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.Territories",
                columns: table => new
                {
                    TerritoryID = table.Column<string>(maxLength: 20, nullable: false),
                    TerritoryDescription = table.Column<string>(maxLength: 50, nullable: false),
                    RegionID = table.Column<int>(nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(nullable: false),
                    CustomerID = table.Column<string>(maxLength: 5, nullable: true),
                    EmployeeID = table.Column<int>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: true),
                    RequiredDate = table.Column<DateTime>(nullable: true),
                    ShippedDate = table.Column<DateTime>(nullable: true),
                    ShipVia = table.Column<int>(nullable: true),
                    Freight = table.Column<double>(nullable: true),
                    ShipName = table.Column<string>(maxLength: 40, nullable: true),
                    ShipAddress = table.Column<string>(maxLength: 60, nullable: true),
                    ShipCity = table.Column<string>(maxLength: 15, nullable: true),
                    ShipRegion = table.Column<string>(maxLength: 15, nullable: true),
                    ShipPostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    ShipCountry = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Orders_@Northwnd.Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "@Northwnd.Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Orders_@Northwnd.Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "@Northwnd.Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Orders_@Northwnd.Shippers_ShipVia",
                        column: x => x.ShipVia,
                        principalTable: "@Northwnd.Shippers",
                        principalColumn: "ShipperID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 40, nullable: false),
                    SupplierID = table.Column<int>(nullable: true),
                    CategoryID = table.Column<int>(nullable: true),
                    QuantityPerUnit = table.Column<string>(maxLength: 20, nullable: true),
                    UnitPrice = table.Column<double>(nullable: true),
                    UnitsInStock = table.Column<short>(nullable: true),
                    UnitsOnOrder = table.Column<short>(nullable: true),
                    ReorderLevel = table.Column<short>(nullable: true),
                    Discontinued = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@Northwnd.Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Products_@Northwnd.Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "@Northwnd.Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_@Northwnd.Products_@Northwnd.Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "@Northwnd.Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "@Northwnd.EmployeeTerritories",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(nullable: false),
                    TerritoryID = table.Column<string>(maxLength: 20, nullable: false)
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
                        name: "FK_@Northwnd.EmployeeTerritories_@Northwnd.Territories_TerritoryID",
                        column: x => x.TerritoryID,
                        principalTable: "@Northwnd.Territories",
                        principalColumn: "TerritoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "@Northwnd.OrderDetails",
                columns: table => new
                {
                    OrderID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<double>(nullable: false),
                    Quantity = table.Column<short>(nullable: false),
                    Discount = table.Column<float>(nullable: false)
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
                name: "IX_AppRegistries_Item_Key",
                table: "AppRegistries",
                columns: new[] { "Item", "Key" },
                unique: true,
                filter: "[Item] IS NOT NULL AND [Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLevels_Root",
                table: "AuditLevels",
                column: "Root");

            migrationBuilder.CreateIndex(
                name: "IX_AuditValues_Level",
                table: "AuditValues",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_ConcurrencyModels_RandomNumber",
                table: "ConcurrencyModels",
                column: "RandomNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LS_Indices_Int0",
                table: "LS_Indices",
                column: "Int0");

            migrationBuilder.CreateIndex(
                name: "IX_LS_Indices_Int1",
                table: "LS_Indices",
                column: "Int1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LS_Indices_Int2_G1_Int3_G1",
                table: "LS_Indices",
                columns: new[] { "Int2_G1", "Int3_G1" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LS_Names_Name_CreationTime",
                table: "LS_Names",
                columns: new[] { "Name", "CreationTime" },
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "@Northwnd.CustomerCustomerDemos");

            migrationBuilder.DropTable(
                name: "@Northwnd.EmployeeTerritories");

            migrationBuilder.DropTable(
                name: "@Northwnd.OrderDetails");

            migrationBuilder.DropTable(
                name: "AppRegistries");

            migrationBuilder.DropTable(
                name: "AuditValues");

            migrationBuilder.DropTable(
                name: "CompositeKeyModels");

            migrationBuilder.DropTable(
                name: "ConcurrencyModels");

            migrationBuilder.DropTable(
                name: "EntityMonitorModels");

            migrationBuilder.DropTable(
                name: "LS_Indices");

            migrationBuilder.DropTable(
                name: "LS_Names");

            migrationBuilder.DropTable(
                name: "LS_Providers");

            migrationBuilder.DropTable(
                name: "SimpleModels");

            migrationBuilder.DropTable(
                name: "TrackModels");

            migrationBuilder.DropTable(
                name: "YearMonthModels");

            migrationBuilder.DropTable(
                name: "@Northwnd.CustomerDemographics");

            migrationBuilder.DropTable(
                name: "@Northwnd.Territories");

            migrationBuilder.DropTable(
                name: "@Northwnd.Orders");

            migrationBuilder.DropTable(
                name: "@Northwnd.Products");

            migrationBuilder.DropTable(
                name: "AuditLevels");

            migrationBuilder.DropTable(
                name: "@Northwnd.Regions");

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
                name: "AuditRoots");
        }
    }
}
