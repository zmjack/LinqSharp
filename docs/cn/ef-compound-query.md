## 复合查询

复合查询（**Compound Query**）属于动态查询的特殊构建方式，它可以将多个查询定义合并成单一查询，能够有效节省数据库访问往返，也可用于缓存数据。

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

- 使用 **CompoundQuery** 进行查询：

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
    (`@1`.`CategoryName` = @__p_Item1_0) 
    AND 
    (EXTRACT(year FROM `@2`.`OrderDate`) = @__p_Item2_1)
) 
OR 
(
    (`@1`.`CategoryName` = @__p_Item1_2) 
    AND 
    (EXTRACT(year FROM `@2`.`OrderDate`) = @__p_Item2_3)
)
```

<br/>

### 为什么使用复合查询

复合是动态 **LINQ** 的特殊构建方式，区别是

- **CompoundQuery** 使用的表达式是多个 **QueryDef** 的合并结果；
- **CompoundQuery** 的查询结果会分配到每个 **QueryDef** 中，每个 **QueryDef** 拥有独立的结果集。

基于这种特殊的查询方式，

- 这对于设计数据计算容器非常有帮助。

<br/>

### 使用场景示例

查询 **Products** 表中，**UnitPrice** 满足以下条件的记录，进行分别汇总：

| 名称             | 筛选条件         | 结果记录数 |
| ---------------- | ---------------- | ---------- |
| Greater than 60  | UnitPrice >= 60  | 5          |
| Greater than 100 | UnitPrice >= 100 | 2          |
| \<All\>          | Above            | 5          |

使用前需要在 **DbContext** 上实现 **ICompoundQueryable** 接口：

```csharp
public class ApplicationDbContext : DbContext, ICompoundQueryable<ApplicationDbContext>
{    
}
```

查询示例：

```csharp
var queryDefs = new[]
{
	new QueryDef<Product>("Greater than 60").Where(x => x.UnitPrice >= 60),
	new QueryDef<Product>("Greater than 100").Where(x => x.UnitPrice >= 100),
};

using (var query = this.BeginCompoundQuery(x => x.Products.OrderBy(x => x.CategoryID)))
{
	var all = query.Feed(queryDefs);
	all.Dump("All");
}

foreach (var def in queryDefs)
{
	def.Result.Dump(def.Name);
}

```

```mysql
SELECT *
FROM `@Northwnd.Products` AS `@`
WHERE (`@`.`UnitPrice` >= 60.0) OR (`@`.`UnitPrice` >= 100.0)
ORDER BY `@`.`CategoryID`
```

![compound-query](https://github.com/zmjack/LinqSharp/blob/master/docs/images/compound-query.png?raw=true)

<br/>

