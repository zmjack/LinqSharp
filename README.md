# LinqSharp

Other Language: [中文](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

<br/>

**LinqSharp** is an open source **LINQ** extension library that allows you to write simple code to generate complex queries, including query extensions and dynamic query generation.

**LinqSharp.EFCore** is an enhanced library for **EntityFramework**, providing more data annotations, database functions, and custom storage rules, etc.

<br/>

**LinqSharp** provides enhancements to **LINQ** in the following ways:

- <font color="orange">[No documentation yet, but have [Chinese version](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/1-introduce.md)]</font> Query extension
- <font color="orange">[No documentation yet, but have [Chinese version](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/2-xwhere.md)]</font> Dynamic LINQ



**LinqSharp.EFCore** provides enhancements to **Entity Frameowk** in the following ways:

- <font color="orange">[no documentation yet]</font> Data check mode
- <font color="orange">[no documentation yet]</font> Database generation helper
- <font color="orange">[no documentation yet]</font> Database custom function mapping
- <font color="orange">[no documentation yet]</font> Custom storage extensions
- <font color="orange">[no documentation yet]</font> Column storage agent

<br/>

**Supported version of Entity Framework:** 

- Entity Framework Core 3.1
- Entity Framework Core 2.1+



**Restricted supported version of Entity Framework: **

- Entity Framework Core 3.0+ : 1 failed.

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