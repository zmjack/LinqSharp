using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbCreator.Migrations
{
    /// <inheritdoc />
    public partial class _202410161747 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "@n.Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "integer", nullable: false),
                    CategoryName = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Picture = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "@n.CustomerDemographics",
                columns: table => new
                {
                    CustomerTypeID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CustomerDesc = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.CustomerDemographics", x => x.CustomerTypeID);
                });

            migrationBuilder.CreateTable(
                name: "@n.Customers",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Phone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    Fax = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "@n.Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "integer", nullable: false),
                    LastName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    TitleOfCourtesy = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Address = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    HomePhone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    Extension = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    Photo = table.Column<byte[]>(type: "bytea", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    ReportsTo = table.Column<int>(type: "integer", nullable: true),
                    PhotoPath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_@n.Employees_@n.Employees_ReportsTo",
                        column: x => x.ReportsTo,
                        principalTable: "@n.Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "@n.Regions",
                columns: table => new
                {
                    RegionID = table.Column<int>(type: "integer", nullable: false),
                    RegionDescription = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Regions", x => x.RegionID);
                });

            migrationBuilder.CreateTable(
                name: "@n.Shippers",
                columns: table => new
                {
                    ShipperID = table.Column<int>(type: "integer", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Phone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Shippers", x => x.ShipperID);
                });

            migrationBuilder.CreateTable(
                name: "@n.Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "integer", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ContactTitle = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    City = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Region = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Country = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Phone = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    Fax = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: true),
                    HomePage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "AppRegistries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Item = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: false),
                    Key = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: false),
                    Value = table.Column<string>(type: "character varying(768)", maxLength: 768, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRegistries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditRoots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalQuantity = table.Column<int>(type: "integer", nullable: false),
                    LimitQuantity = table.Column<int>(type: "integer", nullable: false),
                    RowVersion = table.Column<DateTime>(type: "timestamp with time zone", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRoots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutoModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastWriteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Month_DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Month_DateTimeOffset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Month_NullableDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Month_NullableDateTimeOffset = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Trim = table.Column<string>(type: "text", nullable: true),
                    Upper = table.Column<string>(type: "text", nullable: true),
                    Lower = table.Column<string>(type: "text", nullable: true),
                    Condensed = table.Column<string>(type: "text", nullable: true),
                    Even = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BulkTestModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniqueCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulkTestModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Address_Street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Address_City = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompositeKeyModels",
                columns: table => new
                {
                    Id1 = table.Column<Guid>(type: "uuid", nullable: false),
                    Id2 = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompositeKeyModels", x => new { x.Id1, x.Id2 });
                });

            migrationBuilder.CreateTable(
                name: "ConcurrencyModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    DatabaseWinValue = table.Column<int>(type: "integer", nullable: false),
                    ClientWinValue = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcurrencyModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityMonitorModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Event = table.Column<string>(type: "text", nullable: true),
                    TypeName = table.Column<string>(type: "text", nullable: true),
                    Key = table.Column<string>(type: "text", nullable: true),
                    ChangeValues = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityMonitorModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacadeModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacadeModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LS_Indices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Int0 = table.Column<int>(type: "integer", nullable: false),
                    Int1 = table.Column<int>(type: "integer", nullable: false),
                    Int2_G1 = table.Column<int>(type: "integer", nullable: false),
                    Int3_G1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LS_Indices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LS_Names",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LS_Names", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LS_Providers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Password = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: true),
                    NameModel = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: true),
                    JsonModel = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: true),
                    DictionaryModel = table.Column<string>(type: "character varying(127)", maxLength: 127, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LS_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RowLockModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    LockDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RowLockModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimpleRows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Group_Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Group_Age = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleRows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YearMonthModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearMonthModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "@n.CustomerCustomerDemos",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CustomerTypeID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_@n.CustomerCustomerDemos", x => new { x.CustomerTypeID, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_@n.CustomerCustomerDemos_@n.CustomerDemographics_CustomerTy~",
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
                });

            migrationBuilder.CreateTable(
                name: "@n.Territories",
                columns: table => new
                {
                    TerritoryID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TerritoryDescription = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RegionID = table.Column<int>(type: "integer", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "@n.Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    CustomerID = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    EmployeeID = table.Column<int>(type: "integer", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RequiredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShippedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShipVia = table.Column<int>(type: "integer", nullable: true),
                    Freight = table.Column<double>(type: "double precision", nullable: true),
                    ShipName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    ShipAddress = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    ShipCity = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    ShipRegion = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    ShipPostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ShipCountry = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "@n.Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    SupplierID = table.Column<int>(type: "integer", nullable: true),
                    CategoryID = table.Column<int>(type: "integer", nullable: true),
                    QuantityPerUnit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    UnitPrice = table.Column<double>(type: "double precision", nullable: true),
                    UnitsInStock = table.Column<short>(type: "smallint", nullable: true),
                    UnitsOnOrder = table.Column<short>(type: "smallint", nullable: true),
                    ReorderLevel = table.Column<short>(type: "smallint", nullable: true),
                    Discontinued = table.Column<bool>(type: "boolean", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AuditLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Root = table.Column<Guid>(type: "uuid", nullable: false),
                    ValueCount = table.Column<int>(type: "integer", nullable: false)
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
                name: "@n.EmployeeTerritories",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "integer", nullable: false),
                    TerritoryID = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "@n.OrderDetails",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    ProductID = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<double>(type: "double precision", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AuditValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AppRegistries_Item_Key",
                table: "AppRegistries",
                columns: new[] { "Item", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLevels_Root",
                table: "AuditLevels",
                column: "Root");

            migrationBuilder.CreateIndex(
                name: "IX_AuditValues_Level",
                table: "AuditValues",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_BulkTestModels_UniqueCode",
                table: "BulkTestModels",
                column: "UniqueCode",
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
                unique: true);
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
                name: "AppRegistries");

            migrationBuilder.DropTable(
                name: "AuditValues");

            migrationBuilder.DropTable(
                name: "AutoModels");

            migrationBuilder.DropTable(
                name: "BulkTestModels");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "CompositeKeyModels");

            migrationBuilder.DropTable(
                name: "ConcurrencyModels");

            migrationBuilder.DropTable(
                name: "EntityMonitorModels");

            migrationBuilder.DropTable(
                name: "FacadeModels");

            migrationBuilder.DropTable(
                name: "LS_Indices");

            migrationBuilder.DropTable(
                name: "LS_Names");

            migrationBuilder.DropTable(
                name: "LS_Providers");

            migrationBuilder.DropTable(
                name: "RowLockModels");

            migrationBuilder.DropTable(
                name: "SimpleModels");

            migrationBuilder.DropTable(
                name: "SimpleRows");

            migrationBuilder.DropTable(
                name: "YearMonthModels");

            migrationBuilder.DropTable(
                name: "@n.CustomerDemographics");

            migrationBuilder.DropTable(
                name: "@n.Territories");

            migrationBuilder.DropTable(
                name: "@n.Orders");

            migrationBuilder.DropTable(
                name: "@n.Products");

            migrationBuilder.DropTable(
                name: "AuditLevels");

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

            migrationBuilder.DropTable(
                name: "AuditRoots");
        }
    }
}
