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
- <font color="orange">[暂无文档]</font> 列式存储代理
- [直接访问（清空表，批量导入）](https://github.com/zmjack/LinqSharp/blob/master/docs/cn/ef-direct-functions.md)

<br/>

支持的 Entity Framework 版本： **EF Core 7.0** / 6.0 / 5.0 / 3.1 / 3.0 / 2.1

<br/>

## 安装

通过 **Nuget** 安装：

```powershell
dotnet add package LinqSharp
dotnet add package LinqSharp.EFCore
```

<br/>

## 最近更新

### 版本：7.0

- 提供两个新的数据特性：
  - **[AutoCreatedBy]**：自动维护 **创建条目** 的用户信息。
  - **[AutoUpdatedBy]**：自动维护 **更新条目** 的用户信息。
  - 使 **DbContext** 实现 **IUserTraceable** 接口，详见 [文档](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/ef-data-annotations-2.md#自动维护操作条目的用户信息)。
- **【中断性变更】** 已移除 **QuickDataView**，请使用 **IEnumerableExtensions.FullJoin** 代替。
- **【中断性变更】** 已移除 **IEntity.AcceptBut**。
- **【中断性变更】** 已重命名 IQueryableExtensions.**ToSql** 为 **ToQueryString**。
  - **EFCore 5.0 版本以上**：不提供，使用原生方法。
  - **EFCore 3.1 版本以下**：提供功能，代码兼容。

### 版本：6.0.16

- **【中断性变更】** 已移除 **CustomDatabaseFacade**。
- 提供 **EntityMonitoringFacade** 用于监视表 **CRUD**，以方便编写其他对接操作。
- **【中断性更新】** 已移除 **Ensure** 相关方法，请使用 **AddOrUpdate** 相关方法代替。

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