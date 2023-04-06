# LinqSharp

Other Language: [中文](https://github.com/zmjack/LinqSharp/blob/master/README.cn.md)

<br/>

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

**Supported version of Entity Framework:** **EF Core 7.0** / 6.0 / 5.0 / 3.1 / 2.1

<br/>

## Install

You can install **LinqSharp** through **NuGet**：

```powershell
dotnet add package LinqSharp
dotnet add package LinqSharp.EFCore
```

<br/>

## Recent

### 版本：7.0.2

- Dynamic Query: **QueryHelper** provides property chain analysis to support dynamic query of **Owned Entity**.
- Optimized **GroupByCount** performance (takes about **-35%** in time), but **planned to remove** this method.
- Mark **GroupByCount** as **Obsolute** method, please use **Chunk** method instead.
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

