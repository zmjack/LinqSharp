# LinqSharp

Other Language: [中文](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

<br/>

Current work in progress: Optimize **PreQuery** queries.

<br/>

**LinqSharp** is an open source **LINQ** extension library that allows you to write simple code to generate complex queries, including query extensions and dynamic query generation.

**LinqSharp.EFCore** is an enhanced library for **EntityFramework**, providing more data annotations, database functions, and custom storage rules, etc.

<br/>

**LinqSharp** provides enhancements to **LINQ** in the following ways:

- <font color="orange">[No documentation yet, but have [chinese version](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/query.md)]</font> Query extension
- <font color="orange">[No documentation yet, but have [chinese version](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/filter.md)]</font> Dynamic LINQ



**LinqSharp.EFCore** provides enhancements to **Entity Frameowk** in the following ways:

- <font color="orange">[no documentation yet, but have [chinese version](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/ef-data-annotations-1.md)]</font> Data annotations for table design
- <font color="orange">[no documentation yet, but have [chinese version](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/ef-data-annotations-2.md)]</font>]</font> Data annotations for field standard
- <font color="orange">[no documentation yet]</font> Function mapping
- <font color="orange">[no documentation yet]</font> Column storage agent
- <font color="orange">[no documentation yet]</font> Data calculation and audit

<br/>

**Supported version of Entity Framework:** **EF Core 6.0** / 5.0 / 3.1 / 3.0 / 2.1

<br/>

## Install

You can install **LinqSharp** through **NuGet**：

**Package Manager**

```powershell
Install-Package LinqSharp
Install-Package LinqSharp.EFCore
```

**.NET CLI**

```powershell
dotnet add package LinqSharp
dotnet add package LinqSharp.EFCore
```

<br/>

# Recent

#### Version: (2.1|3.0|3.1).114+ | (5.0|6.0).14+

- Some methods in **AutoAttribute** has been changed:

  ```c#
  /*
  public abstract object Format(object entity, object value);
  */
  public abstract object Format(object entity, Type propertyType, object value);
  ```

#### Version: (2.1|3.0|3.1).106+ | (5.0|6.0).6+

- Change the method name **XWhere** to **Filter**.
- Allows to create a stand-alone filter **IQueryFilter** and query in the **Filter** method.

---

#### Version: (2.1|3.0|3.1).104+ | (5.0|6.0).4+

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

---

#### Version: (2.1|3.0|3.1).80+ | (5.0|6.0).0+

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
Install-Package Northwnd
```

```powershell
dotnet add package Northwnd
```

Then, you can try **LinqSharp**:

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
    var sql = query.ToSql();
}
```

The variable **sql** is:

```sql
SELECT "x"."ShipperID", "x"."CompanyName", "x"."Phone"
FROM "Shippers" AS "x"
WHERE "x"."CompanyName" = 'Speedy Express';
```

<br/>

