﻿// <auto-generated />
using System;
using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DbCreator.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230414052230_202304141300")]
    partial class _202304141300
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CustomerCustomerDemographic", b =>
                {
                    b.Property<string>("CustomerDemographicsCustomerTypeID")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("CustomersCustomerID")
                        .HasColumnType("varchar(5)");

                    b.HasKey("CustomerDemographicsCustomerTypeID", "CustomersCustomerID");

                    b.HasIndex("CustomersCustomerID");

                    b.ToTable("CustomerCustomerDemographic");
                });

            modelBuilder.Entity("EmployeeTerritory", b =>
                {
                    b.Property<int>("EmployeesEmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("TerritoriesTerritoryID")
                        .HasColumnType("varchar(20)");

                    b.HasKey("EmployeesEmployeeID", "TerritoriesTerritoryID");

                    b.HasIndex("TerritoriesTerritoryID");

                    b.ToTable("EmployeeTerritory");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.LS_Name", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Note")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name", "CreationTime")
                        .IsUnique();

                    b.ToTable("LS_Names");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.AppRegistryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Item")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Item", "Key")
                        .IsUnique();

                    b.ToTable("AppRegistries");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.AuditLevel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Root")
                        .HasColumnType("char(36)");

                    b.Property<int>("ValueCount")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Root");

                    b.ToTable("AuditLevels");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.AuditRoot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("LimitQuantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TotalQuantity")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AuditRoots");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.AuditValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Level")
                        .HasColumnType("char(36)");

                    b.Property<int>("Quantity")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Level");

                    b.ToTable("AuditValues");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.BulkTestModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("UniqueCode");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("BulkTestModels");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.CPKeyModel", b =>
                {
                    b.Property<Guid>("Id1")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Id2")
                        .HasColumnType("char(36)");

                    b.HasKey("Id1", "Id2");

                    b.ToTable("CompositeKeyModels");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.ConcurrencyModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("ClientWinValue")
                        .HasColumnType("int");

                    b.Property<int>("DatabaseWinValue")
                        .HasColumnType("int");

                    b.Property<int>("RowVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ConcurrencyModels");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.EntityMonitorModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ChangeValues")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Event")
                        .HasColumnType("longtext");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.Property<string>("TypeName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EntityMonitorModels");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.LS_Index", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Int0")
                        .HasColumnType("int");

                    b.Property<int>("Int1")
                        .HasColumnType("int");

                    b.Property<int>("Int2_G1")
                        .HasColumnType("int");

                    b.Property<int>("Int3_G1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Int0");

                    b.HasIndex("Int1")
                        .IsUnique();

                    b.HasIndex("Int2_G1", "Int3_G1")
                        .IsUnique();

                    b.ToTable("LS_Indices");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.LS_Provider", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("DictionaryModel")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("JsonModel")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("NameModel")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.Property<string>("Password")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)");

                    b.HasKey("Id");

                    b.ToTable("LS_Providers");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.SimpleModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SimpleModels");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.SimpleRow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("SimpleRows");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.TrackModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ForCondensed")
                        .HasColumnType("longtext");

                    b.Property<int>("ForEven")
                        .HasColumnType("int");

                    b.Property<string>("ForLower")
                        .HasColumnType("longtext");

                    b.Property<string>("ForTrim")
                        .HasColumnType("longtext");

                    b.Property<string>("ForUpper")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastWriteTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("TrackModels");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.YearMonthModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("YearMonthModels");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Test.FacadeModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("FacadeModels");
                });

            modelBuilder.Entity("Northwnd.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Picture")
                        .HasColumnType("longblob");

                    b.HasKey("CategoryID");

                    b.ToTable("@Northwnd.Categories", (string)null);
                });

            modelBuilder.Entity("Northwnd.Customer", b =>
                {
                    b.Property<string>("CustomerID")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("Address")
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("City")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("ContactName")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("ContactTitle")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Country")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Fax")
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)");

                    b.Property<string>("Phone")
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Region")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.HasKey("CustomerID");

                    b.ToTable("@Northwnd.Customers", (string)null);
                });

            modelBuilder.Entity("Northwnd.CustomerCustomerDemo", b =>
                {
                    b.Property<string>("CustomerTypeID")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("CustomerID")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.HasKey("CustomerTypeID", "CustomerID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("CustomerTypeID");

                    b.ToTable("@Northwnd.CustomerCustomerDemos", (string)null);
                });

            modelBuilder.Entity("Northwnd.CustomerDemographic", b =>
                {
                    b.Property<string>("CustomerTypeID")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("CustomerDesc")
                        .HasColumnType("longtext");

                    b.HasKey("CustomerTypeID");

                    b.ToTable("@Northwnd.CustomerDemographics", (string)null);
                });

            modelBuilder.Entity("Northwnd.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("City")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Country")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Extension")
                        .HasMaxLength(4)
                        .HasColumnType("varchar(4)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<DateTime?>("HireDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("HomePhone")
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Notes")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("longblob");

                    b.Property<string>("PhotoPath")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Region")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<int?>("ReportsTo")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("TitleOfCourtesy")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.HasKey("EmployeeID");

                    b.HasIndex("ReportsTo");

                    b.ToTable("@Northwnd.Employees", (string)null);
                });

            modelBuilder.Entity("Northwnd.EmployeeTerritory", b =>
                {
                    b.Property<int>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("TerritoryID")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("EmployeeID", "TerritoryID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("TerritoryID");

                    b.ToTable("@Northwnd.EmployeeTerritories", (string)null);
                });

            modelBuilder.Entity("Northwnd.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<string>("CustomerID")
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<int?>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<double?>("Freight")
                        .HasColumnType("double");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("RequiredDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ShipAddress")
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("ShipCity")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("ShipCountry")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("ShipName")
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("ShipPostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("ShipRegion")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<int?>("ShipVia")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ShippedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("OrderID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("ShipVia");

                    b.ToTable("@Northwnd.Orders", (string)null);
                });

            modelBuilder.Entity("Northwnd.OrderDetail", b =>
                {
                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<float>("Discount")
                        .HasColumnType("float");

                    b.Property<short>("Quantity")
                        .HasColumnType("smallint");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("double");

                    b.HasKey("OrderID", "ProductID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductID");

                    b.ToTable("@Northwnd.OrderDetails", (string)null);
                });

            modelBuilder.Entity("Northwnd.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryID")
                        .HasColumnType("int");

                    b.Property<bool>("Discontinued")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("QuantityPerUnit")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<short?>("ReorderLevel")
                        .HasColumnType("smallint");

                    b.Property<int?>("SupplierID")
                        .HasColumnType("int");

                    b.Property<double?>("UnitPrice")
                        .HasColumnType("double");

                    b.Property<short?>("UnitsInStock")
                        .HasColumnType("smallint");

                    b.Property<short?>("UnitsOnOrder")
                        .HasColumnType("smallint");

                    b.HasKey("ProductID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("SupplierID");

                    b.ToTable("@Northwnd.Products", (string)null);
                });

            modelBuilder.Entity("Northwnd.Region", b =>
                {
                    b.Property<int>("RegionID")
                        .HasColumnType("int");

                    b.Property<string>("RegionDescription")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("RegionID");

                    b.ToTable("@Northwnd.Regions", (string)null);
                });

            modelBuilder.Entity("Northwnd.Shipper", b =>
                {
                    b.Property<int>("ShipperID")
                        .HasColumnType("int");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("Phone")
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)");

                    b.HasKey("ShipperID");

                    b.ToTable("@Northwnd.Shippers", (string)null);
                });

            modelBuilder.Entity("Northwnd.Supplier", b =>
                {
                    b.Property<int>("SupplierID")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("City")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<string>("ContactName")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("ContactTitle")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Country")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Fax")
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)");

                    b.Property<string>("HomePage")
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasMaxLength(24)
                        .HasColumnType("varchar(24)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Region")
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.HasKey("SupplierID");

                    b.ToTable("@Northwnd.Suppliers", (string)null);
                });

            modelBuilder.Entity("Northwnd.Territory", b =>
                {
                    b.Property<string>("TerritoryID")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("RegionID")
                        .HasColumnType("int");

                    b.Property<string>("TerritoryDescription")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("TerritoryID");

                    b.HasIndex("RegionID");

                    b.ToTable("@Northwnd.Territories", (string)null);
                });

            modelBuilder.Entity("CustomerCustomerDemographic", b =>
                {
                    b.HasOne("Northwnd.CustomerDemographic", null)
                        .WithMany()
                        .HasForeignKey("CustomerDemographicsCustomerTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Northwnd.Customer", null)
                        .WithMany()
                        .HasForeignKey("CustomersCustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EmployeeTerritory", b =>
                {
                    b.HasOne("Northwnd.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeesEmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Northwnd.Territory", null)
                        .WithMany()
                        .HasForeignKey("TerritoriesTerritoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Client", b =>
                {
                    b.OwnsOne("LinqSharp.EFCore.Data.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("ClientId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("City")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("Street")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.HasKey("ClientId");

                            b1.ToTable("Clients");

                            b1.WithOwner()
                                .HasForeignKey("ClientId");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.AuditLevel", b =>
                {
                    b.HasOne("LinqSharp.EFCore.Data.Test.AuditRoot", "RootLink")
                        .WithMany("Levels")
                        .HasForeignKey("Root")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RootLink");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.AuditValue", b =>
                {
                    b.HasOne("LinqSharp.EFCore.Data.Test.AuditLevel", "LevelLink")
                        .WithMany("Values")
                        .HasForeignKey("Level")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LevelLink");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.SimpleRow", b =>
                {
                    b.OwnsOne("LinqSharp.EFCore.Data.Test.SimpleRowItemGroup", "Group", b1 =>
                        {
                            b1.Property<Guid>("SimpleRowId")
                                .HasColumnType("char(36)");

                            b1.Property<int>("Age")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.HasKey("SimpleRowId");

                            b1.ToTable("SimpleRows");

                            b1.WithOwner()
                                .HasForeignKey("SimpleRowId");
                        });

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Northwnd.CustomerCustomerDemo", b =>
                {
                    b.HasOne("Northwnd.Customer", "CustomerLink")
                        .WithMany("CustomerCustomerDemos")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Northwnd.CustomerDemographic", "CustomerDemographicLink")
                        .WithMany("CustomerCustomerDemos")
                        .HasForeignKey("CustomerTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CustomerDemographicLink");

                    b.Navigation("CustomerLink");
                });

            modelBuilder.Entity("Northwnd.Employee", b =>
                {
                    b.HasOne("Northwnd.Employee", "Superordinate")
                        .WithMany("Subordinates")
                        .HasForeignKey("ReportsTo");

                    b.Navigation("Superordinate");
                });

            modelBuilder.Entity("Northwnd.EmployeeTerritory", b =>
                {
                    b.HasOne("Northwnd.Employee", "EmployeeLink")
                        .WithMany("EmployeeTerritories")
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Northwnd.Territory", "TerritoryLink")
                        .WithMany("EmployeeTerritories")
                        .HasForeignKey("TerritoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmployeeLink");

                    b.Navigation("TerritoryLink");
                });

            modelBuilder.Entity("Northwnd.Order", b =>
                {
                    b.HasOne("Northwnd.Customer", "CustomerLink")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerID");

                    b.HasOne("Northwnd.Employee", "EmployeeLink")
                        .WithMany("Orders")
                        .HasForeignKey("EmployeeID");

                    b.HasOne("Northwnd.Shipper", "Shipper")
                        .WithMany("Orders")
                        .HasForeignKey("ShipVia");

                    b.Navigation("CustomerLink");

                    b.Navigation("EmployeeLink");

                    b.Navigation("Shipper");
                });

            modelBuilder.Entity("Northwnd.OrderDetail", b =>
                {
                    b.HasOne("Northwnd.Order", "OrderLink")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Northwnd.Product", "ProductLink")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("OrderLink");

                    b.Navigation("ProductLink");
                });

            modelBuilder.Entity("Northwnd.Product", b =>
                {
                    b.HasOne("Northwnd.Category", "CategoryLink")
                        .WithMany("Products")
                        .HasForeignKey("CategoryID");

                    b.HasOne("Northwnd.Supplier", "SupplierLink")
                        .WithMany("Products")
                        .HasForeignKey("SupplierID");

                    b.Navigation("CategoryLink");

                    b.Navigation("SupplierLink");
                });

            modelBuilder.Entity("Northwnd.Territory", b =>
                {
                    b.HasOne("Northwnd.Region", "Region")
                        .WithMany("Territories")
                        .HasForeignKey("RegionID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.AuditLevel", b =>
                {
                    b.Navigation("Values");
                });

            modelBuilder.Entity("LinqSharp.EFCore.Data.Test.AuditRoot", b =>
                {
                    b.Navigation("Levels");
                });

            modelBuilder.Entity("Northwnd.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Northwnd.Customer", b =>
                {
                    b.Navigation("CustomerCustomerDemos");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Northwnd.CustomerDemographic", b =>
                {
                    b.Navigation("CustomerCustomerDemos");
                });

            modelBuilder.Entity("Northwnd.Employee", b =>
                {
                    b.Navigation("EmployeeTerritories");

                    b.Navigation("Orders");

                    b.Navigation("Subordinates");
                });

            modelBuilder.Entity("Northwnd.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Northwnd.Product", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Northwnd.Region", b =>
                {
                    b.Navigation("Territories");
                });

            modelBuilder.Entity("Northwnd.Shipper", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Northwnd.Supplier", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Northwnd.Territory", b =>
                {
                    b.Navigation("EmployeeTerritories");
                });
#pragma warning restore 612, 618
        }
    }
}
