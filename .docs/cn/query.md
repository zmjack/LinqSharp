## 基础扩展

### 输出 SQL

“**输出 SQL**”是研究“**SQL 生成**”的基础，使用 **LinqSharp.EFCore** 中的 **ToSql** 方法：

```csharp
using (var sqlite = new NorthwndContext(options))
{
    var query = sqlite.Regions.Where(x => x.RegionDescription == "Northern");
    var sql = query.ToSql();
}
```
生成 SQL：
```sqlite
SELECT "r"."RegionID", "r"."RegionDescription"
FROM "Regions" AS "r"
WHERE "r"."RegionDescription" = 'Northern';
```

**注1**：由于不同版本的 **EntityFrameworkCore** 的 **SQL** 生成器设计不同，因此，生成 SQL 可能会存在差异。（**EntityFrameworkCore 5.0** 公开了 **ToQueryString** 来支持这项功能）。

**注2**：**LinqSharp.EFCore** 最新版本不兼容所有 **EntityFrameworkCore**，需使用“**大版本号**”与 **EntityFrameworkCore** 一致的发行库（例如，2.2.x，3.0.x，3.1.x）。

<br/>

### 默认返回方法扩展

- **MinOrDefault**：原函数 **Min** 的不抛异常版本，异常返回默认值；
- **MaxOrDefault**：原函数 **Max** 的不抛异常版本，异常返回默认值；
- **AverageOrDefault**： 原函数 **Average** 的不抛异常版本，异常返回默认值。

```csharp
// throw 'Sequence contains no elements'
new int[0].Min();

new int[0].MinOrDefault();      // 0
new int[0].MinOrDefault(-1);    // -1
```

<br/>

### 查询值最小或最大的记录

- **WhereMin**：查询指定字段最小的记录；
- **WhereMax**：查询指定字段最大的记录。

**WhereMin** 和 **WhereMax** 会进行两次查询：

1. 查询指定字段的“最小值”或“最大值”；
2. 查询指定字段“最小值”或“最大值”的记录。

例如，查询员工（**Empolyees**）表中年龄最小的员工：

| EmployeeID | FirstName | BirthDate           |
| :--------- | :-------- | :------------------ |
| 1          | Nancy     | 12/08/1948 00:00:00 |
| 2          | Andrew    | 02/19/1952 00:00:00 |
| 3          | Janet     | 08/30/1963 00:00:00 |
| 4          | Margaret  | 09/19/1937 00:00:00 |
| 5          | Steven    | 03/04/1955 00:00:00 |
| 6          | Michael   | 07/02/1963 00:00:00 |
| 7          | Robert    | 05/29/1960 00:00:00 |
| 8          | Laura     | 01/09/1958 00:00:00 |
| 9          | Anne      | 01/27/1966 00:00:00 |

```csharp
var query = sqlite.Employees
    .WhereMax(x => x.BirthDate);
var result = query.Select(x => new
{
    x.EmployeeID,
    x.FirstName,
    x.BirthDate,
}).ToArray();
```

生成 SQL：

```sql
/* Step 1 */
SELECT MIN("e"."BirthDate")
FROM "Employees" AS "e";

/* Step 2 */
SELECT *
FROM "Employees" AS "e"
WHERE "e"."BirthDate" = '1966-01-27 00:00:00';
```

执行结果：

| EmployeeID | FirstName | BirthDate           |
| :--------- | :-------- | :------------------ |
| 9          | Anne      | 01/27/1966 00:00:00 |

<br/>

### 数据搜索

- **Search**：返回“从指定字段或外键表字段中进行模糊或精确查询”的查询结果。

**Search** 函数提供了四种搜索模式（**SearchOption**）：

- **Contains**（默认）： 任何指定字段中“包含”搜索字符串；
- **NotContains**：所有指定字段中都“不包含”搜索字符串；
- **Equals**：搜索字符串与某指定字段“相等”；
- **NotEquals**：搜索字符串“不在”任何指定字段中。

例如，查询雇员（**Employees**）表中地址（**Address**）或城市（**City**）包含字母 **m** 的雇员：

| EmployeeID | Address                       | City     |
| :--------- | :---------------------------- | :------- |
| 1          | 507 - 20th Ave. E. Apt. 2A    | Seattle  |
| 2          | 908 W. Capital Way            | Tacoma   |
| 3          | 722 Moss Bay Blvd.            | Kirkland |
| 4          | 4110 Old Redmond Rd.          | Redmond  |
| 5          | 14 Garrett Hill               | London   |
| 6          | Coventry House Miner Rd.      | London   |
| 7          | Edgeham Hollow Winchester Way | London   |
| 8          | 4726 - 11th Ave. N.E.         | Seattle  |
| 9          | 7 Houndstooth Rd.             | London   |

```csharp
var query = sqlite.Employees
    .Search("m", e => new
    {
        e.Address,
        e.City,
    });
var result = query.Select(x => new
{
    x.EmployeeID,
    x.Address,
    x.City,
}).ToArray();
```

生成 SQL：

```sql
SELECT *
FROM "Employees" AS "e"
WHERE (('m' = '') OR (instr("e"."Address", 'm') > 0)) 
   OR (('m' = '') OR (instr("e"."City", 'm') > 0)); 
```

运行结果：

| EmployeeID | Address                       | City    |
| :--------- | :---------------------------- | :------ |
| 2          | 908 W. Capital Way            | Tacoma  |
| 4          | 4110 Old Redmond Rd.          | Redmond |
| 7          | Edgeham Hollow Winchester Way | London  |

**Search** 函数同样提供了外链表的查询（主表或从表查询）。

例如，查询供应商（**Suppliers**）表中供应任何种类豆腐（**Tofu**）的供应商：

| SupplierID | CompanyName                        | Products                                  |
| :--------- | :--------------------------------- | :---------------------------------------- |
| 1          | Exotic Liquids                     | Chai, Chang, Aniseed Syrup                |
| \|         | \|                                 | \|                                        |
| \|         | \|                                 | \|                                        |
| 4          | Tokyo Traders                      | Mishi Kobe Niku, Ikura, Longlife Tofu     |
| 5          | Cooperativa de Quesos 'Las Cabras' | Queso Cabrales, Queso Manchego La Pastora |
| 6          | Mayumi's                           | Konbu, Tofu, Genen Shouyu                 |
| \|         | \|                                 | \|                                        |
| \|         | \|                                 | \|                                        |
| 29         | Forêts d'érables                   | Sirop d'érable, Tarte au sucre            |

```csharp
var query = sqlite.Suppliers
    .Include(x => x.Products)
    .Search("Tofu", s => new
    {
        ProductNames = s.Products.Select(x => x.ProductName),
    });

var result = query.Select(x => new
{
    x.SupplierID,
    x.CompanyName,
    Products = string.Join(", ", x.Products.Select(p => p.ProductName)),
}).ToArray();
```

生成 SQL：

```sql
SELECT *
FROM "Suppliers" AS "s"
LEFT JOIN "Products" AS "p" ON "s"."SupplierID" = "p"."SupplierID"
WHERE EXISTS (
    SELECT 1
    FROM "Products" AS "p0"
    WHERE ("s"."SupplierID" = "p0"."SupplierID") 
      AND (('Tofu' = '') OR (instr("p0"."ProductName", 'Tofu') > 0)))
ORDER BY "s"."SupplierID", "p"."ProductID";
```

运行结果：

| SupplierID | CompanyName   | Products                              |
| :--------- | :------------ | :------------------------------------ |
| 4          | Tokyo Traders | Mishi Kobe Niku, Ikura, Longlife Tofu |
| 6          | Mayumi's      | Konbu, Tofu, Genen Shouyu             |

<br/>

### 分页查询

- **SelectPage**：查询结果分页或执行分页查询。（分页参数从第 **1** 页开始）

例如，查询雇员（**Employees**）表，按每页 **2** 条记录分页，选择第 **3** 页的记录返回：

| EmployeeID | Address                       | City     |
| :--------- | :---------------------------- | :------- |
| 1          | 507 - 20th Ave. E. Apt. 2A    | Seattle  |
| 2          | 908 W. Capital Way            | Tacoma   |
| 3          | 722 Moss Bay Blvd.            | Kirkland |
| 4          | 4110 Old Redmond Rd.          | Redmond  |
| 5          | 14 Garrett Hill               | London   |
| 6          | Coventry House Miner Rd.      | London   |
| 7          | Edgeham Hollow Winchester Way | London   |
| 8          | 4726 - 11th Ave. N.E.         | Seattle  |
| 9          | 7 Houndstooth Rd.             | London   |

```csharp
var query = sqlite.Employees
    .SelectPage(pageNumber: 3, pageSize: 2);
var result = query.Select(x => new
{
    x.EmployeeID,
    x.Address,
    x.City,
}).ToArray();
```

生成 SQL：

```sql
SELECT *
FROM "Employees" AS "e"
ORDER BY (SELECT 1)
LIMIT 2 OFFSET 4;
```

运行结果：

| EmployeeID | Address                  | City   |
| :--------- | :----------------------- | :----- |
| 5          | 14 Garrett Hill          | London |
| 6          | Coventry House Miner Rd. | London |

<br/>

### 序列排序

- **OrderByCase / ThenByCase**：按指定字符串序列排序。

例如，查询地区（**Regions**）表，将结果按 **N / E / W / S** 的地区序列排序返回：

| RegionID | RegionDescription |
| :------- | :---------------- |
| 1        | Eastern           |
| 2        | Western           |
| 3        | Northern          |
| 4        | Southern          |

```csharp
var query = sqlite.Regions
    .OrderByCase(x => x.RegionDescription, new[]
    {
        "Northern",
        "Eastern",
        "Western",
        "Southern",
    });
var result = query.Select(x => new
{
    x.RegionID,
    x.RegionDescription,
});
```

执行 SQL：

```sql
SELECT *
FROM "Regions" AS "r"
ORDER BY CASE
    WHEN "r"."RegionDescription" = 'Northern' THEN 0
    ELSE CASE
        WHEN "r"."RegionDescription" = 'Eastern' THEN 1
        ELSE CASE
            WHEN "r"."RegionDescription" = 'Western' THEN 2
            ELSE CASE
                WHEN "r"."RegionDescription" = 'Southern' THEN 3
                ELSE 4
            END
        END
    END
END;
```

运行结果：

| RegionID | RegionDescription |
| :------- | :---------------- |
| 3        | Northern          |
| 1        | Eastern           |
| 2        | Western           |
| 4        | Southern          |

<br/>

----

### 按组元素数量分组

数量分组函数 **GroupByCount** 用于根据指定每组记录数量（每组最多允许 **n** 条记录）进行特殊分组。

例如，将如下指定字符串按每行 **16** 个字符分成多行：

```csharp
var s = "0123456789ABCDEF0123456789ABCDEF"
    .GroupByCount(16)
    .Select(g => new string(g.ToArray()));
```

> 0123456789ABCDEF
> 0123456789ABCDEF

<br/>

### 树结构查询

- **SelectMore**：按树结构遍历，选择“树节点中 **所有** 满足条件的 **节点**”；
- **SelectUntil**：按树结构遍历，**直到** 在每个子路径中找到满足条件的节点，选择 **该节点**；
- **SelectWhile**：按树结构遍历，选择“**所有子路径** 中连续满足条件的 **路径节点**”。

例如，雇员（**Employees**）表按照 **EmployeeID** 和 **ReportsTo** 定义结构如下：

![employee-tree.png](https://github.com/zmjack/LinqSharp/blob/master/docs/images/employee-tree.png?raw=true)

| EmployeeID | FirstName | ReportsTo | ReportsTo_ |
| :--------- | :-------- | :-------- | :--------- |
| 1          | Nancy     | 2         | Andrew     |
| 2          | Andrew    |           |            |
| 3          | Janet     | 2         | Andrew     |
| 4          | Margaret  | 2         | Andrew     |
| 5          | Steven    | 2         | Andrew     |
| 6          | Michael   | 5         | Steven     |
| 7          | Robert    | 5         | Steven     |
| 8          | Laura     | 2         | Andrew     |
| 9          | Anne      | 5         | Steven     |

<br/>

### SelectMore

按树结构遍历，选择“树节点中 **所有** 满足条件的 **节点**”。

例如，查询由 **2** 号雇员 **Andrew** 领导的所有成员（2, 1, 3, 4, 5, 6, 7, 9, 8）：

![select-more.png](https://github.com/zmjack/LinqSharp/blob/master/docs/images/select-more.png?raw=true)

方法： 使用 **SelectMore** 从根节点查找即可。

```csharp
var employees = sqlite.Employees
    .Include(x => x.Superordinate)
    .Include(x => x.Subordinates)
    .ToArray();
var query = employees
    .Where(x => x.EmployeeID == 2)
    .SelectMore(x => x.Subordinates);

var result = query.Select(x => new
{
    x.EmployeeID,
    x.FirstName,
    x.ReportsTo,
    ReportsTo_ = x.Superordinate?.FirstName,
});
```

运行结果：

| EmployeeID | FirstName | ReportsTo | ReportsTo_ |
| :--------- | :-------- | :-------- | :--------- |
| 2          | Andrew    |           |            |
| 1          | Nancy     | 2         | Andrew     |
| 3          | Janet     | 2         | Andrew     |
| 4          | Margaret  | 2         | Andrew     |
| 5          | Steven    | 2         | Andrew     |
| 6          | Michael   | 5         | Steven     |
| 7          | Robert    | 5         | Steven     |
| 9          | Anne      | 5         | Steven     |
| 8          | Laura     | 2         | Andrew     |

<br/>

### SelectUntil

按树结构遍历，**直到** 在每个子路径中找到满足条件的节点，选择 **该节点**。

例如，查询由 **2** 号雇员 **Andrew** 领导的所有基层员工（叶节点，1, 3, 6, 7, 9, 8）：

![select-until.png](https://github.com/zmjack/LinqSharp/blob/master/docs/images/select-until.png?raw=true)

方法：使用 **SelectUntil** 从根节点查找，直到节点 **Subordinates** 为空。

```csharp
var employees = sqlite.Employees
    .Include(x => x.Superordinate)
    .Include(x => x.Subordinates)
    .ToArray();
var query = employees
    .Where(x => x.EmployeeID == 2)
    .SelectUntil(x => x.Subordinates, x => !x.Subordinates.Any());

var result = query.Select(x => new 
{
    x.EmployeeID,
    x.FirstName,
    x.ReportsTo,
    ReportsTo_ = x.Superordinate?.FirstName,
});
```

运行结果：

| EmployeeID | FirstName | ReportsTo | ReportsTo_ |
| :--------- | :-------- | :-------- | :--------- |
| 1          | Nancy     | 2         | Andrew     |
| 3          | Janet     | 2         | Andrew     |
| 4          | Margaret  | 2         | Andrew     |
| 6          | Michael   | 5         | Steven     |
| 7          | Robert    | 5         | Steven     |
| 9          | Anne      | 5         | Steven     |
| 8          | Laura     | 2         | Andrew     |

<br/>

## SelectWhile

按树结构遍历，选择“**所有子路径** 中连续满足条件的 **路径节点**”。

例如，查询由 **2** 号雇员 **Andrew** 领导的所有非基层员工（非叶节点，2, 5）：

![select-while.png](https://github.com/zmjack/LinqSharp/blob/master/docs/images/select-while.png?raw=true)

方法：使用 **SelectWhile** 从根节点查找路径，直到节点 **Subordinates** 为空。

```csharp
var employees = sqlite.Employees
    .Include(x => x.Superordinate)
    .Include(x => x.Subordinates)
    .ToArray();
var query = employees
    .Where(x => x.EmployeeID == 2)
    .SelectWhile(x => x.Subordinates, x => x.Subordinates.Any());

var result = query.Select(x => new 
{
    x.EmployeeID,
    x.FirstName,
    Subordinates = string.Join(", ", x.Subordinates
        .SelectMore(s => s.Subordinates)
        .Select(s => s.FirstName)),
});
```

运行结果：

| EmployeeID | FirstName | Subordinates                                                 |
| :--------- | :-------- | :----------------------------------------------------------- |
| 2          | Andrew    | Nancy, Janet, Margaret, Steven, Michael, Robert, Anne, Laura |
| 5          | Steven    | Michael, Robert, Anne                                        |

<br/>

