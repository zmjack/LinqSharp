<h1 align="center">
    <img src="https://github.com/zmjack/LinqSharp/blob/master/LinqSharp.png" height="32" />
    LinqSharp
</h1>

<p align="center">
    <a href="https://linqsharp.net" rel="nofollow"><img src="https://img.shields.io/badge/English-linqsharp.net-orange" /></a>
    <a href="https://zh.linqsharp.net" rel="nofollow"><img src="https://img.shields.io/badge/中文-zh.linqsharp.net-orange" /></a>
</p>
<p align="center">
    <a href="https://www.nuget.org/packages/LinqSharp" rel="nofollow"><img src="https://img.shields.io/nuget/v/LinqSharp.svg?logo=nuget&label=LinqSharp" /></a>
    <a href="https://www.nuget.org/packages/LinqSharp" rel="nofollow"><img src="https://img.shields.io/nuget/dt/LinqSharp.svg?logo=nuget&label=Download" /></a>
    <a href="https://www.nuget.org/packages/LinqSharp.EFCore" rel="nofollow"><img src="https://img.shields.io/nuget/v/LinqSharp.EFCore.svg?logo=nuget&label=LinqSharp.EFCore" /></a>
    <a href="https://www.nuget.org/packages/LinqSharp.EFCore" rel="nofollow"><img src="https://img.shields.io/nuget/dt/LinqSharp.EFCore.svg?logo=nuget&label=Download" /></a>
</p>


**LinqSharp** is an open source **LINQ** extension library that allows you to write simple code to generate complex queries, including query extensions and dynamic query generation.

**LinqSharp.EFCore** is an enhanced library for **EntityFramework**, providing more data annotations, database functions, and custom storage rules, etc.

<br/>

**LinqSharp** provides enhancements to **LINQ** in the following ways:

- <font color="orange">[Chinese]</font> [Query extensions](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/query.md)
- <font color="orange">[Chinese]</font> [Dynamic LINQ](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/filter.md)

**LinqSharp.EFCore** provides enhancements to **Entity Frameowk** in the following ways:

- <font color="orange">[Chinese]</font> [Data annotations for table design](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-data-annotations-1.md)
- <font color="orange">[Chinese]</font> [Data annotations for field formatting](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-data-annotations-2.md)
- <font color="orange">[Chinese]</font> [Compound query](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-compound-query.md)
- <font color="orange">[Chinese]</font> [Custom translator](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-translator.md)
- <font color="orange">[Chinese]</font> [Concurrency Resolving](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-concurrency-resolving.md)
- <font color="orange">[Chinese]</font> [Agent query for Key-Value structure](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-agent-query.md)
- <font color="orange">[Chinese]</font> [Direct handling](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-direct-handling.md)

<br/>

#### Supported versions of .NET

| Frameworks        | Versions                                                     |
| ----------------- | ------------------------------------------------------------ |
| **.NET**          | ![Static Badge](https://img.shields.io/badge/-8.0-8A2BE2) ![Static Badge](https://img.shields.io/badge/-7.0-8A2BE2) ![Static Badge](https://img.shields.io/badge/-6.0-8A2BE2) ![Static Badge](https://img.shields.io/badge/-5.0-8A2BE2) |
| **.NET Standard** | ![Static Badge](https://img.shields.io/badge/-2.1-orange) ![Static Badge](https://img.shields.io/badge/-2.0-orange) |

#### Supported versions of Entity Framework

| **Entity Framework Core** | Versions                                                     |
| ------------------------- | ------------------------------------------------------------ |
| **Supported**             | ![Static Badge](https://img.shields.io/badge/-7.0-8A2BE2) ![Static Badge](https://img.shields.io/badge/-6.0-8A2BE2) ![Static Badge](https://img.shields.io/badge/-5.0-8A2BE2) ![Static Badge](https://img.shields.io/badge/-3.1-8A2BE2) |
| **Preparing to end**      | ![Static Badge](https://img.shields.io/badge/-2.1-ff0000)    |

We are preparing to discontinue support for **LinqSharp.EFCore - EF2.1** because projects that reference it may encounter the following error when compiling:

System.Runtime.CompilerServices.Unsafe doesn't support **netcoreapp2.1**. Consider updating your TargetFramework to **netcoreapp3.1 or later**.

<br/>

## Install

You can install **LinqSharp** through **NuGet**：

```powershell
dotnet add package LinqSharp
dotnet add package LinqSharp.EFCore
```

<br/>

## Recent

### Version 7.0.37.5 - EFCore

- Add new annotation **RowLock** to prevent locked records from being changed or deleted.
- Add new annotation **AutoMonthOnly** to format **DateTime** / **DateTimeOffset** / **DateOnly** only keeping the year and month.

### Version 7.0.37

- Rename **IExtraFieldFilter** to **IAdvancedFieldFilter**.

### Version 7.0.36

- New Methods: Added **Random()** and **RandomOrDefault()** to get a random record.

- **Breaking Change**: Columns marked by **SpecialAutoAttribute** cannot be modified manually, and their values can only be maintained by the engine.

  This change will work better with the **Update** method provided by EFCore.

### Version 7.0.35

- **Experimental:**

  Some methods in **LinqSharp.EFCore** that do not depend on **EntityFramework** have been extracted to **LinqSharp.EFCore**.

  Third-party libraries can use these methods more conveniently by referencing **LinqSharp.EFCore.Core**.

- **DirectQuery** has been renamed to **DirectQueryScope**.

### Version: 7.0.34

- New Feature: Added **IExtraFieldFilter** interface for more flexible field filtering.

### Version: 7.0.32

- Compatibility updates: **NStandard - 0.48.0 - Update**, **Ref**.

### Version: 7.0.30

- Compatibility updates: **NStandard - 0.45.0 - Update**, **DateOnlyType** and **DateTimeType**.

### Version: 7.0.27

- **Breaking Change**: Adjusted some namespace names.
- **Bug fixed**: Null support for **AllSame**.

### Version: 7.0.24

- **Breaking Change**: Adjusted some namespace names.

### Version: 7.0.20

- **Indexing** / **UniqueIndexing** modified to lazy query.

### Version: 7.0.18.1

- Two field filters have been added: **DateOnlyRangeFilter**, **DateTimeRangeFilter**.
- These methods has been marked as obsolete, please use **FilterBy(Func<,>, DateTimeRangeFilter)** method instead:
  - **WhereAfter**
  - **WhereBefore**
  - **WhereBetween**

### Version: 7.0.17

- New Feature: Added **FilterBy** support for **QueryHelper**, **IFieldFilter** can now be applied directly to **QueryHelper**.

### Version: 7.0.13

- New Feature: Added **IFieldFilter** for building field-based conditions, support dynamic building.

### Version: 7.0.11

- New Feature: Added **IFieldLocalFilter** / **IFieldQueryFilter** for building field-based conditions.

### Version: 7.0.10

- New Feature: Added **IEnumerableExtensions.Index** for creating indexes to provide faster queries.

### Version: 7.0.9

- **IQueryFilter** no longer needs to implement local filter methods. Implement **ILocalFilter** if needed.
- **Filter** extension now supports executing multiple filters sequentially.

### Version: 7.0.2

- Dynamic Query: **QueryHelper** provides property chain analysis to support dynamic query of **Owned Entity**.
- Optimized **GroupByCount** performance (takes about **-35%** in time), but **planned to remove** this method.
- Mark **GroupByCount** as **Obsolete** method, please use **Chunk** method instead.
  - **EFCore 6.0 and above**: Not provided, use the native method.
  - **EFCore 5.0 and below**: Code compatibility.

### Version: 7.0

- Provides two new data annotations:

  - **[AutoCreatedBy]**: Automatically maintain user information for **created entries**.
  - **[AutoUpdatedBy]**: Automatically maintain user information for **updated entries**.
  - Make **DbContext** implement **IUserTraceable** interface, see [documentation](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-data-annotations-2.md for details #Automatic maintenance of user information for operation entries).
- **\[Breaking Change\]** **QuickDataView** has been removed, please use **IEnumerableExtensions.FullJoin** instead.
- **\[Breaking Change\]** **IEntity.AcceptBut** has been removed.
- **\[Breaking Change\]** Change the method name **IQueryableExtensions.ToSql** to **ToQueryString**.
  - **EFCore 5.0 and above**: Not provided, use the native method.
  - **EFCore 3.1 and below**: Code compatibility.

### Version: 6.0.16

- **\[Breaking Change\]** The **Ensure** methods have been removed, and **AddOrUpdate** methods are recommended to be used instead.
- **\[Breaking Change\]** **CustomDatabaseFacade** has been removed.
- Provide **EntityMonitoringFacade** for monitoring table **CRUD** to facilitate writing other docking operations.

### Version: 6.0.14

- Some methods in **AutoAttribute** has been changed:

  ```c#
  /*
  public abstract object Format(object entity, object value);
  */
  public abstract object Format(object entity, Type propertyType, object value);
  ```

### Version: 6.0.6

- Change the method name **XWhere** to **Filter**.
- Allows to create a stand-alone filter **IQueryFilter** and query in the **Filter** method.

### Version: 6.0.4

- Simplify the code writing for **Provider**.
  
- After 2.1.104 | 3.0.104 | 3.1.104 | 5.0.4 | 6.0.4 .
  

Old：

  ```csharp
  [Provider(typeof(JsonProvider<NameModel>))]
  public NameModel NameModel { get; set; }
  ```

New：

  ```csharp
  [JsonProvider]
  public NameModel NameModel { get; set; }
  ```

### Version: 6.0

- To avoid naming conflicts, **IndexAttribute** has been renamed to **IndexFieldAttribute**.

<br/>

## Try using the sample database

**Northwnd**, an early sample database shipped with SQL Server, describes a simple "enterprise sales network" scenario.

The database includes a network of employees, orders, and suppliers.

<br/>

The **NuGet** version of **Northwnd** is a **SQLite** database (**Code First**).

Repository：https://github.com/zmjack/Northwnd

<br/>

You can install **Northwnd** through **NuGet**：

```powershell
dotnet add package Northwnd
```

Try **LinqSharp**:

```csharp
using (var context = NorthwndContext.UseSqliteResource())
{
    ...
}
```

<br/>

For example:

```C#
using (var sqlite = NorthwndContext.UseSqliteResource())
{
    var query = sqlite.Shippers.Where(x => x.CompanyName == "Speedy Express");
    var sql = query.ToQueryString();
    Console.WriteLine(sql);
}
```

The variable **sql** is:

```sql
SELECT "x"."ShipperID", "x"."CompanyName", "x"."Phone"
FROM "Shippers" AS "x"
WHERE "x"."CompanyName" = 'Speedy Express';
```

<br/>

