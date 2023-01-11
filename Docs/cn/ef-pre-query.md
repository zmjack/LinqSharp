# LinqSharp

[● 返回列表](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

<br/>

## 预查询

预查询（**PreQuery**）属于动态查询的特殊构建方式，它可以将多个查询定义合并成单一查询，能够有效节省数据库访问往返，可用于缓存数据。

本文以示例数据库 **Northwnd** 举例说明。

<br/>

例如，构建以下查询需求：

| 产品类型          | 订单年份 | 查询内容                                  |
| ----------------- | -------- | ----------------------------------------- |
| 饮料（Beverages） | 1996     | 所有产品订单单价（OrderDetail.UnitPrice） |
| 海产品（Seafood） | 1997     | 所有产品订单单价（OrderDetail.UnitPrice） |

我们可以先将数据取出进行缓存。

先构建查询参数：

```csharp
(string CategoryName, int Year)[] queryParams = new[]
{
    ("Beverages", 1996),
    ("Seafood", 1997),
};
```

- 使用 **Filter** 进行查询：
  

    ```csharp
    var query = this.OrderDetails
        .Include(x => x.OrderLink)
        .Include(x => x.ProductLink).ThenInclude(x => x.CategoryLink)
        .Filter(h => h.Or(queryParams.Select(p =>
        {
            return h.Where(x => 
                x.ProductLink.CategoryLink.CategoryName == p.CategoryName 
             && x.OrderLink.OrderDate.Value.Year == p.Year);
        })));
    ```

- 使用 **PreQuery** 进行查询：

    ```csharp
    var preQueries = queryParams.Select(p =>
    {
        return this.CreatePreQuery(x => x.OrderDetails)
            .Include(x => x.OrderLink)
            .Include(x => x.ProductLink).ThenInclude(x => x.CategoryLink)
            .Where(x =>
                x.ProductLink.CategoryLink.CategoryName == p.CategoryName
             && x.OrderLink.OrderDate.Value.Year == p.Year);
    }).ToArray();
    var query = this.ExcuteQueries(preQueries);
    ```

它们将生成相同的 SQL：

```mysql
-- Region Parameters
-- @__p_Item1_0='Beverages' (Size = 15), 
-- @__p_Item2_1='1996', 
-- @__p_Item1_2='Seafood' (Size = 15), 
-- @__p_Item2_3='1997'
-- EndRegion
SELECT *
FROM `@Northwnd.OrderDetails` AS `@`
INNER JOIN `@Northwnd.Products` AS `@0` ON `@`.`ProductID` = `@0`.`ProductID`
LEFT JOIN `@Northwnd.Categories` AS `@1` ON `@0`.`CategoryID` = `@1`.`CategoryID`
INNER JOIN `@Northwnd.Orders` AS `@2` ON `@`.`OrderID` = `@2`.`OrderID`
WHERE 
    (
        ((`@1`.`CategoryName` = @__p_Item1_0) AND `@2`.`OrderDate` IS NOT NULL)
        AND 
        (EXTRACT(year FROM `@2`.`OrderDate`) = @__p_Item2_1)
    )
    OR 
    (
        ((`@1`.`CategoryName` = @__p_Item1_2) AND `@2`.`OrderDate` IS NOT NULL)
        AND 
        (EXTRACT(year FROM `@2`.`OrderDate`) = @__p_Item2_3)
    )
```

<br/>

### 为什么使用预查询

预查询（**PreQuery**）是动态 **LINQ** 的特殊构建方式，区别是

- **PreQuery** 构建的是 多个表达式合并的查询结果集；
- **PreQuery** 可以进行合并查询，并将查询结果筛选到每个 **PreQuery** 中。

基于这种特殊的查询方式，

- **PreQuery** 可以获得合并后的查询结果；
- 也可以使每个独立 **PreQuery** 获得各自独立的查询结果（**Filter** 只能查询合并结果）；
- 这对于设计数据计算容器非常有帮助。

<br/>

### 使用方法

1. 使用 **DbContext** 扩展方法 **CreatePreQuery** 为指定 **DbSet** 创建 **PreQuery**；
2. （重要）使用 **DbContext** 扩展方法 **ExcuteQueries** 为多个 **PreQuery** 执行查询，得到合并查询结果；
3.  **PreQuery** 下的 **Excute** 方法可以获得该筛选信息下的独立查询结果。

<br/>

### 原理

每个预查询包含以下部分：

- 预先加载信息（Eager Loading Info），使用 **Include** 或 **ThenInclude** 方法。
- 筛选信息（Where Part Info），使用 **Where** 方法。

执行原理：

- **ExcuteQueries** 方法会将每个 **PreQuery** 的筛选条件进行合并后提交数据库查询，缓存并返回合并结果；
- **PreQuery.Excute** 方法会从合并结果缓存中筛选当前条件的记录（若未缓存则进行新的查询）。

<br/>

### 案例

查询 **Products** 表中，**UnitPrice** 满足以下条件的记录，进行分别汇总：

| 名称             | 筛选条件         | 结果记录数 |
| ---------------- | ---------------- | ---------- |
| Greater than 60  | UnitPrice >= 60  | 5          |
| Greater than 100 | UnitPrice >= 100 | 2          |
| \<All\>          | Above            | 5          |

```csharp
var preQueries = new[]
{
    this.CreatePreQuery(x => x.Products)
        .As("Greater than 60")
        .Where(x => x.UnitPrice >= 60),
    
    this.CreatePreQuery(x => x.Products)
        .As("Greater than 100")
        .Where(x => x.UnitPrice >= 100),
};

// Calling this function will submit the query
this.ExcuteQueries(preQueries).Dump("All");

foreach (var preQuery in preQueries)
{
    preQuery.Excute().OrderBy(x => x.UnitPrice).Dump(preQuery.Name);
}
```

```mysql
SELECT *
FROM `@Northwnd.Products` AS `@`
WHERE (`@`.`UnitPrice` >= 60.0) OR (`@`.`UnitPrice` >= 100.0)
```

<br/>