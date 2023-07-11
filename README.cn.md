# LinqSharp

其他语言：[English](https://github.com/zmjack/LinqSharp/blob/master/README.md)

<br/>

**LinqSharp** 是个开源 **LINQ** 扩展库，它允许您编写简单代码来生成复杂查询，包括查询扩展和动态查询生成。

**LinqSharp.EFCore** 是对 **EntityFramework** 的增强库，提供更多数据注解、数据库函数及自定义储存规则等。

<br/>

**LinqSharp** 可为 **LINQ** 提供如下方面的增强：

- [查询扩展](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/query.md)
- [动态 LINQ](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/filter.md)

**LinqSharp.EFCore** 可为 **Entity Frameowk** 提供如下方面的增强：

- [表设计数据特性](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-data-annotations-1.md)
- [字段格式化特性](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-data-annotations-2.md)
- [复合查询](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-compound-query.md)
- [自定义转译器](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-translator.md)
- [解决并发冲突](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-concurrency-resolving.md)
- [用于 Key-Value 结构的代理查询](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-agent-query.md)
- [直接访问支持](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-direct-handling.md)

<br/>

支持的 Entity Framework 版本： **EF Core 7.0** / 6.0 / 5.0 / 3.1 / 2.1

<br/>

## 安装

通过 **Nuget** 安装：

```powershell
dotnet add package LinqSharp
dotnet add package LinqSharp.EFCore
```

<br/>

## 最近更新

**LinqSharp** 三岁了！

多年来，我们创建了很多功能，也删除了很多功能。 

多数功能已获得认可，除非必要，否则我们不太可能对其进行中断性更新。 

现在是将它们添加到 **wiki** 的时候了。 



我们正在准备一份详细的文档，相信很快就能提供给大家。

**感谢！**

<br/>

### 版本：7.0.13

- 新功能：添加 **IFieldFilter** 用于构建基于字段的条件，支持动态构建。

### 版本：7.0.11

- 新功能：添加 **IFieldLocalFilter** / **IFieldQueryFilter** 用于构建基于字段的条件。

### 版本：7.0.10

- 新功能：添加 **IEnumerableExtensions.Index**，用于创建索引以提供更快的查询。

### 版本：7.0.9

- **IQueryFilter** 不再需要实现本地过滤方法。如有需要，请实现 **ILocalFilter**。
- **Filter** 扩展现在支持按顺序执行多个过滤器。

### 版本：7.0.2

- 动态查询：**QueryHelper** 提供属性链解析，以支持 **Owned Entity** 的动态查询。
- 优化 **GroupByCount** 性能（耗时约 **-35%**），但 **计划删除** 该方法。
- 标记 **GroupByCount** 为 **已过时** 方法，请使用 **Chunk** 方法代替。
  - **EFCore 6.0 版本以上**：不提供，使用原生方法。
  - **EFCore 5.0 版本以下**：代码兼容。

### 版本：7.0

- 提供两个新的数据特性：
  - **[AutoCreatedBy]**：自动维护 **创建条目** 的用户信息。
  - **[AutoUpdatedBy]**：自动维护 **更新条目** 的用户信息。
  - 使 **DbContext** 实现 **IUserTraceable** 接口，详见 [文档](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-data-annotations-2.md#自动维护操作条目的用户信息)。
- **【中断性变更】** 已移除 **QuickDataView**，请使用 IEnumerableExtensions.**FullJoin** 代替。
- **【中断性变更】** 已移除 **IEntity.AcceptBut**。
- **【中断性变更】** 已重命名 IQueryableExtensions.**ToSql** 为 **ToQueryString**。
  - **EFCore 5.0 版本以上**：不提供，使用原生方法。
  - **EFCore 3.1 版本以下**：代码兼容。

### 版本：6.0.16

- **【中断性更新】** 已移除 **Ensure** 相关方法，请使用 **AddOrUpdate** 相关方法代替。
- **【中断性变更】** 已移除 **CustomDatabaseFacade**。
- 提供 **EntityMonitoringFacade** 用于监视表 **CRUD**，以方便编写其他对接操作。

### 版本：6.0.14

- **AutoAttribute** 成员方法更新：

  ```c#
  /*
  public abstract object Format(object entity, object value);
  */
  public abstract object Format(object entity, Type propertyType, object value);
  ```

### 版本：6.0.6

- 更改方法名 **XWhere** 到 **Filter**。
- 允许创建独立筛选器 **IQueryFilter**，并在 **Filter** 方法中进行查询。

### 版本：6.0.4

- 简化转储器写法。

  旧写法：

  ```csharp
  [Provider(typeof(JsonProvider<NameModel>))]
  public NameModel NameModel { get; set; }
  ```

  新写法：

  ```csharp
  [JsonProvider]
  public NameModel NameModel { get; set; }
  ```

### 版本：6.0

- 为避免命名冲突，**IndexAttribute** 已被重命名为 **IndexFieldAttribute**。

<br/>

## 使用示例数据库试用

**Northwnd** 是 **SQL Server** 早期附带的示例数据库，描述了“公司销售网”的案例场景。

该数据库包括“雇员（**Employees**）、产品订单（**Orders**）、供应商（**Suppliers**）”的关系网络。

<br/>

**Northwnd** 的 **NuGet** 版本是 **SQLite** 数据库（**Code First**）。

代码仓库：https://github.com/zmjack/Northwnd

<br/>

通过 **NuGet** 安装 **Northwnd** ：

```powershell
dotnet add package Northwnd
```

试用 **LinqSharp**:

```csharp
using (var context = NorthwndContext.UseSqliteResource())
{
    ...
}
```

例如：

```csharp
using (var sqlite = NorthwndContext.UseSqliteResource())
{
    var query = sqlite.Shippers.Where(x => x.CompanyName == "Speedy Express");
    var sql = query.ToQueryString();
    Console.WriteLine(sql);
}
```

变量 **sql** 为：

```sql
SELECT "x"."ShipperID", "x"."CompanyName", "x"."Phone"
FROM "Shippers" AS "x"
WHERE "x"."CompanyName" = 'Speedy Express';
```

<br/>