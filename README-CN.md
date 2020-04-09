# LinqSharp

**LinqSharp** 是很智能的 **Linq** 扩展库，它允许您编写更简单的代码来生成复杂查询、进行数据检查、自定义存储 逻辑等常用功能。

- [English Readme](https://github.com/zmjack/LinqSharp/blob/master/README.md)
- [中文自述](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

<br/>

**LinqSharp** 按不同应用场景可以为 **Entity Frameowk** 提供如下方面的增强：

- <font color="limegreen">[README]</font> 查询扩展（增强 SQL 生成、增强内存查询）
- <font color="limegreen">[README]</font> 动态 LINQ
- <font color="orange">[暂无文档]</font> 数据检查模式（方便进行数据一致性检查）
- <font color="orange">[暂无文档]</font> 数据库生成辅助工具（复合主键、字段索引）
- <font color="orange">[暂无文档]</font> 数据库自定义函数映射（增强 SQL 生成，例如 RAND 函数）
- <font color="orange">[暂无文档]</font> 自定义存储扩展（数据格式调整、复杂数据存储、加密储存等）
- <font color="orange">[暂无文档]</font> 列式存储代理（全局注册信息）

<br/>

**支持的 Entity Framework 版本：**

- Entity Framework Core 3.1
- Entity Framework Core 2.0+

 **受限支持的 Entity Framework 版本：**

- Entity Framework Core 3.0：1 项失败。

<br/>

## 如何使用？

示例将使用 **Northwnd** 库作为数据源来展示其用法。

您可以通过 **Nuget** 安装 **Northwnd**：

```powershell
install-package Northwnd
```

![](https://raw.githubusercontent.com/zmjack/Northwnd/master/Northwnd/%40Resources/Northwnd/Northwnd.png)

**Northwnd** 提供了基于代码优先的 **Northwnd** 数据库定义和 **Sqlite** 数据源。

您可以使用如下代码进行简单的查询尝试，同时可以使用 **ToSql** 方法来输出生成的 **SQL** 语句：

```C#
using (var sqlite = NorthwndContext.UseSqliteResource())
{
    var query = sqlite.Shippers.Where(x => x.CompanyName == "Speedy Express");
    var sql = query.ToSql();
}
```

```sqlite
SELECT "x"."ShipperID", "x"."CompanyName", "x"."Phone"
FROM "Shippers" AS "x"
WHERE "x"."CompanyName" = 'Speedy Express';
```

<iframe width="100%" height="475" src="https://dotnetfiddle.net/Widget/X55y12" frameborder="0"></iframe>
**NorthwndContext.UseSqliteResource()** 方法会使用默认 **Sqlite** 数据源：

> **%userprofile%/.nuget/northwnd/{version}/content/@Resources/Northwnd/northwnd.db**

<br/>

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

<br/>

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

<br/>

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

<br/>

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

<br/>

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

<br/>

## 动态 LINQ

通常情况下，静态 LINQ 已经可以适应多数的查询情景。

看一下更复杂的查询场景。

1. 设计查询逻辑，接受输入一组数据，每个数据包含 (**TitleOfCourtesy**，***Year***)；
2. 查询 **Employees** 表中 **TitleOfCourtesy** 和 **BirthDate.Year** 匹配输入数据的记录。

假设输入数据：

```c#
new[] { ("Mr.", 1955), ("Ms.", 1963) };
```

很容易写出静态 LINQ 调用：

```c#
var query = mysql.Employees.Where(x =>
    (x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955)
    || (x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963));
```
但是，输入数据并不是定量的，所以我们需要把它们按指定逻辑连起来。

我们提供了 **XWhere** 方法来帮助你完成语句连接。

<br/>

### XWhere

动态构建查询树，解决动态参数查询场景。示例为 MySQL。

---

1. 按本节给出的查询场景，先定义输入数据：

```c#
var searches = new[] { ("Mr.", 1955), ("Ms.", 1963) };
```

2. 在 **XWhere** 函数中将 **searches** 的每个单元转换成表达式单元 **parts**，并使用 **Or** 方法连接：

```c#
var query = mysql.Employees.XWhere(h =>
{
    var parts = searches
        .Select(s => 
                h.Where(x => x.TitleOfCourtesy == s.Item1 
                          && x.BirthDate.Value.Year == s.Item2))
        .ToArray();
    return h.Or(parts);
});
```

**h.Or** 方法还提供更简单的重截：

```c#
var query = mysql.Employees.XWhere(h =>
{
    return h.Or(searches, s => 
        x => x.TitleOfCourtesy == s.Item1 && x.BirthDate.Value.Year == s.Item2);
});
```

3. 生成表达式查询（示例）

```c#
// 示例等效于 Where 语法
mysql.Employees.Where(x =>
	(x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955)
	|| (x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963));
```

```mysql
SELECT *
FROM `@Northwnd.Employees` AS `x`
WHERE 
((`x`.`TitleOfCourtesy` = 'Mr.') AND (EXTRACT(year FROM `x`.`BirthDate`) = 1955)) 
OR 
((`x`.`TitleOfCourtesy` = 'Ms.') AND (EXTRACT(year FROM `x`.`BirthDate`) = 1963));
```

---

**XWhere** 同样支持手动连接不同的表达式单元。

使用运算符：

```c#
var query = mysql.Employees.XWhere(h =>
{
    return h.Where(x => x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955)
         | h.Where(x => x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963);
});
```
或使用函数：

```c#
var query = mysql.Employees.XWhere(h =>
{
    return h.Or(
        h.Where(x => x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955),
        h.Where(x => x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963));
});
```

或者混合使用：

```c#
var query = mysql.Employees.XWhere(h =>
{
    return h.Or(
        h.Where(x => x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955),
        h.Where(x => x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963)
    ) & h.WhereSearch("Manager", x => x.Title);
});
```
<br/>

