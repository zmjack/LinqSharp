# LinqSharp

**LinqSharp** 是一个更智能 **Linq** 扩展库，它允许您编写更简单的代码来生成复杂查询、进行数据检查、自定义存储 逻辑等常用功能。



**LinqSharp** 按不同应用场景可以为 **Entity Frameowk** 提供如下方面的增强：

- 查询扩展（增强 SQL 生成、增强内存查询）
- 数据检查设计模式（方便进行数据一致性检查）
- 数据库生成辅助工具（复合主键、字段索引）
- 数据库自定义函数映射（增强 SQL 生成，例如 RAND 函数）
- 自定义存储扩展（数据格式调整、复杂数据存储、加密储存等）
- 列式存储代理（全局注册信息）
- 更加容易编写的 SQL 参数化查询（SQL 参数化）



**支持的 Entity Framework 版本：**

- Entity Framework Core 2.0+

 **未来支持的 Entity Framework 版本：**

- Entity Framework Core 3.0+
  Entity Framework Core 3.0 基于 NETStandard2.1 构建且部分内部 API 已发生变化，因此暂未支持。



## 如何使用？

为了便于说明，以下示例将使用 **Northwnd** 库作为数据源来展示其用法。您可以通过 **Nuget** 安装 **Northwnd**。

```powershell
install-package Northwnd
```

![](https://raw.githubusercontent.com/zmjack/Northwnd/master/Northwnd/%40Resources/Northwnd/Northwnd.png)

**Northwnd** 提供了基于代码优先的 **Northwnd** 数据库定义和 **Sqlite** 数据源。

您可以使用如下代码进行简单的查询尝试，同时可以使用 **ToSql** 方法来输出生成的 **SQL** 语句：

<iframe width="100%" height="475" src="https://dotnetfiddle.net/Widget/X55y12" frameborder="0"></iframe>
```C#
using (var sqlite = NorthwndContext.UseSqliteResource())
{
    var query = sqlite.Shippers.Where(x => x.CompanyName == "Speedy Express");
    var sql = query.ToSql();
    // the sql is
    /*
    SELECT "x"."ShipperID", "x"."CompanyName", "x"."Phone"
	FROM "Shippers" AS "x"
	WHERE "x"."CompanyName" = 'Speedy Express';
	*/
}
```

```sqlite
SELECT *
FROM "Shippers" AS "x"
WHERE "x"."CompanyName" = 'Speedy Express';
```

**NorthwndContext.UseSqliteResource()** 方法会使用默认 **Sqlite** 数据源：

> **%userprofile%/.nuget/northwnd/{version}/content/@Resources/Northwnd/northwnd.db**



## 查询扩展

### WhereSearch

查询“某些字段包括指定字符串”的记录。

----

例如，查询表 ***Employees*** 中字段 ***FirstName*** 包含 ***Steven*** 的记录：

  ```C#
sqlite.Employees.WhereSearch("Steven", x => x.FirstName);
  ```

  ```sqlite
SELECT *
FROM "Categories" AS "x"
WHERE instr("x"."FirstName", 'Steven') > 0;
  ```

----

例如，查询表 ***Employees*** 中字段 ***FirstName*** 或 ***LastName*** 的记录：

  ```C#
sqlite.Employees.WhereSearch("An", x => new { x.FirstName, x.LastName })
  ```

  ```sqlite
SELECT *
FROM "Employees" AS "x"
WHERE (instr("x"."FirstName", 'An') > 0) OR (instr("x"."LastName", 'An') > 0);
  ```

----

可以看到 **WhereSearch** 提供了查询多个字段的能力，但是这样还不够。

在一些复杂的查询场景，我们可能希望连接多个表来执行查询。

例如，查询“谁出售了产品（**Product**）给名叫 ***QUICK*** 的客户（**Customer**）”：

  ```c#
sqlite.Employees.WhereSearch("QUICK", x => x.Orders.Select(o => o.CustomerID));
  ```

  ```sqlite
SELECT *
FROM "Employees" AS "x"
WHERE EXISTS (
    SELECT 1
    FROM "Orders" AS "o"
    WHERE (instr("o"."CustomerID", 'QUICK') > 0) AND ("x"."EmployeeID" = "o"."EmployeeID"));
  ```

----

也许我们还需要在多个字段中查找多个字符串。

例如，查询“产品名称（***ProductName*** ）或数量单位（***QuantityPerUnit***）中包含 **ToFu** 和 ***pkg*** 的产品（Product）”：

  ```c#
sqlite.Products
    .WhereSearch("Tofu", x => new { x.ProductName, x.QuantityPerUnit })
    .WhereSearch("pkg", x => new { x.ProductName, x.QuantityPerUnit });
  ```

  ```c#
sqlite.Products
	.WhereSearch(new[] { "Tofu", "pkg" }, 
                 x => new { x.ProductName, x.QuantityPerUnit });
  ```

```sqlite
SELECT *
FROM "Products" AS "x"
WHERE 
((instr("x"."ProductName", 'Tofu') > 0) OR (instr("x"."QuantityPerUnit", 'Tofu') > 0)) AND 
((instr("x"."ProductName", 'pkg') > 0) OR (instr("x"."QuantityPerUnit", 'pkg') > 0));
```

----



### WhereMatch

查询“**某些字段等于指定字符串**”的记录。

----

逻辑不同于 **WhereSearch** 的模糊匹配，**WhereMatch** 将执行精确匹配。

例如，例如，查询表 ***Employees*** 中字段 ***FirstName*** 为 ***Steven*** 的记录：

```c#
sqlite.Employees.WhereSearch("Steven", x => x.FirstName);
```

```sqlite
SELECT *
FROM "Employees" AS "x"
WHERE "x"."FirstName" = 'Steven';
```

----



### WhereBetween / WhereBefore / WhereAfter

- WhereBetween：查询“**某个日期字段在指定的日期范围**”  的记录。
- WhereBefore：查询“**某个日期字段在指定的日期范围**”  的记录。
- WhereAfter：查询“**某个日期字段在指定的日期范围**”  的记录。

注：此组函数在日期字段为 ***NULL*** 时，返回 ***false***。

----

例如，查询“生日在 ***1960-5-1*** 到 ***1960-5-31*** 的员工”：

  ```c#
sqlite.Employees.WhereBetween(
    x => x.BirthDate, 
    new DateTime(1960, 5, 1), new DateTime(1960, 5, 31));
  ```

  ```sqlite
SELECT *
FROM "Employees" AS "x"
WHERE CASE
    WHEN "x"."BirthDate" IS NOT NULL
    THEN CASE
        WHEN ('1960-05-01 00:00:00' <= "x"."BirthDate") AND ("x"."BirthDate" <= '1960-05-31 00:00:00')
        THEN 1 ELSE 0
    END ELSE 0
END = 1;
  ```



### WhereMax / WhereMin

- WhereMax：查询“**范围内某个字段最大**”  的记录。

- WhereMin：查询“**范围内某个字段最小**”  的记录。

注：该组函数将会分成两步查询：(1) 查询最小值；(2) 查询最小值的记录。

---

例如，查询“年龄最小的员工（Employees）”：

```c#
sqlite.Employees.WhereMin(x => x.BirthDate);
```

```sqlite
SELECT *
FROM "Employees" AS "x"
WHERE "x"."BirthDate" = '1937-09-19 00:00:00';
```

---



### OrderByCase / OrderByCaseDescending

- OrderByCase：查询记录并“**按指定字符串序列排序**"。
- OrderByCaseDescending：查询记录并“**按指定字符串序列倒序排序**"。

注：**ThenByCase** / **ThenByCaseDescending** 为该函数组的后序排序方法。

---

例如，查询地区描述（RegionDescription），

  ```c#
sqlite.Regions.OrderByCase(
    x => x.RegionDescription, 
    new[] { "Northern", "Eastern", "Western", "Southern" });
  ```

  ```sqlite
SELECT "x"."RegionID", "x"."RegionDescription"
FROM "Regions" AS "x"
ORDER BY CASE
    WHEN "x"."RegionDescription" = 'Northern'
    THEN 0 ELSE CASE
        WHEN "x"."RegionDescription" = 'Eastern'
        THEN 1 ELSE CASE
            WHEN "x"."RegionDescription" = 'Western'
            THEN 2 ELSE CASE
                WHEN "x"."RegionDescription" = 'Southern'
                THEN 3 ELSE 4
            END
        END
    END
END;
  ```

---



## 过期内容（待删除或改进）

### WhereOr

  ```C#
  sqlite.Employees.WhereOr(sqlite.Employees
  	.GroupBy(x => x.TitleOfCourtesy)
  	.Select(g => new
  	{
  		TitleOfCourtesy = g.Key,
  		BirthDate = g.Max(x => x.BirthDate),
  	}));
  ```

  This invoke will generated two SQL string, the first is:

  ```sqlite
  SELECT "x"."TitleOfCourtesy", MAX("x"."BirthDate") AS "BirthDate"
  FROM "Employees" AS "x"
  GROUP BY "x"."TitleOfCourtesy";
  ```

  the follow SQL will use all the field of the first result as it's where condition. So, the follow SQL string is:

  ```sqlite
  SELECT "e"."EmployeeID", "e"."Address", "e"."BirthDate", "e"."City", "e"."Country", "e"."Extension", "e"."FirstName", "e"."HireDate", "e"."HomePhone", "e"."LastName", "e"."Notes", "e"."Photo", "e"."PhotoPath", "e"."PostalCode", "e"."Region", "e"."ReportsTo", "e"."Title", "e"."TitleOfCourtesy"
  FROM "Employees" AS "e"
  WHERE (((("e"."TitleOfCourtesy" = 'Dr.') AND ("e"."BirthDate" = '1952-02-19 00:00:00')) OR (("e"."TitleOfCourtesy" = 'Mr.') AND ("e"."BirthDate" = '1963-07-02 00:00:00'))) OR (("e"."TitleOfCourtesy" = 'Mrs.') AND ("e"."BirthDate" = '1937-09-19 00:00:00'))) OR (("e"."TitleOfCourtesy" = 'Ms.') AND ("e"."BirthDate" = '1966-01-27 00:00:00'));
  ```

### TryUpdate

  ```C#
  sqlite.Orders
  	.TryUpdate(x => x.Order_Details.Any(y => y.Discount >= 0.02))
      .Set(x => x.ShipCity, "Reims")
      .Save();
  ```

  ```sqlite
  UPDATE "Orders" SET "ShipCity"='Reims' WHERE EXISTS (
      SELECT 1
      FROM "Order Details" AS "y"
      WHERE ("y"."Discount" >= 0.02) AND ("Orders"."OrderID" = "y"."OrderID"));
  ```

  **Next feature**: to support set a value which is calculated by the specified entity ***x***.

### TryDelete

  ```C#
  sqlite.Orders
  	.TryDelete(x => x.Order_Details.Any(y => y.Discount >= 0.02))
      .Save();
  ```

  ```sqlite
  DELETE FROM "Orders" WHERE EXISTS (
      SELECT 1
      FROM "Order Details" AS "y"
      WHERE ("y"."Discount" >= 0.02) AND ("Orders"."OrderID" = "y"."OrderID"));
  ```



