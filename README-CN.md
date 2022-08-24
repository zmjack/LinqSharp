# LinqSharp

其他语言：[English](https://github.com/zmjack/LinqSharp/blob/master/README.md)

<br/>

目前正在进行的工作：优化 **预查询** 性能及调用结构。

<br/>

**LinqSharp** 是个开源 **LINQ** 扩展库，它允许您编写简单代码来生成复杂查询，包括查询扩展和动态查询生成。

**LinqSharp.EFCore** 是对 **EntityFramework** 的增强库，提供更多数据注解、数据库函数及自定义储存规则等。

<br/>

**LinqSharp** 可为 **LINQ** 提供如下方面的增强：

- [查询扩展](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/query.md)
- [动态 LINQ](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/xwhere.md)



**LinqSharp.EFCore** 可为 **Entity Frameowk** 提供如下方面的增强：

- [表设计数据注解](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/ef-data-annotations-1.md)
- [字段标准化数据注解](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/ef-data-annotations-2.md)
- [直接访问函数（清空表，批量导入）](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/ef-direct-functions.md)
- <font color="orange">[试验（文档可能并不准确）]</font> [预查询](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/ef-pre-query.md)
- <font color="orange">[暂无文档]</font> 函数映射
- <font color="orange">[暂无文档]</font> 列式存储代理
- <font color="orange">[暂无文档]</font> 关联计算与审计

<br/>

支持的 Entity Framework 版本： **EF Core 6.0** / 5.0 / 3.1 / 3.0 / 2.1

<br/>

## 安装

通过 **Nuget** 安装：

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

# 最近更新

#### 2.1.106 | 3.0.106 | 3.1.106 | 5.0.6 | 6.0.6 及未来版本。

- 更改方法名 **XWhere** 到 **Filter**。
- 允许创建独立的筛选器 **IQueryFilter**，并在 **Filter** 方法中进行查询。

---

#### 2.1.104 | 3.0.104 | 3.1.104 | 5.0.4 | 6.0.4 及未来版本。

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

---

#### 2.1.80 | 3.0.80 | 3.1.80 | 5.0.0 | 6.0.0 及未来版本。

- 为避免命名冲突，**IndexAttribute** 已被重命名为 **IndexFieldAttribute**。

<br/>

## 使用示例数据库尝试

**Northwnd** 是 **SQL Server** 早期附带的示例数据库，描述了“公司销售网”的案例场景。

该数据库包括“雇员（**Employees**）、产品订单（**Orders**）、供应商（**Suppliers**）”的关系网络。

<br/>

**Northwnd** 的 **NuGet** 版本是 **SQLite** 数据库（**Code First**）。

代码仓库：https://github.com/zmjack/Northwnd

<br/>

通过 **NuGet** 安装 **Northwnd** ：

```powershell
Install-Package Northwnd
```

```powershell
dotnet add package Northwnd
```

随后，便可以试用 **LinqSharp**:

```csharp
using (var context = NorthwndContext.UseSqliteResource())
{
    ...
}
```

<br/>

例如：

```csharp
using (var sqlite = NorthwndContext.UseSqliteResource())
{
    var query = sqlite.Shippers.Where(x => x.CompanyName == "Speedy Express");
    var sql = query.ToSql();
}
```

变量 **sql** 为：

```sql
SELECT "x"."ShipperID", "x"."CompanyName", "x"."Phone"
FROM "Shippers" AS "x"
WHERE "x"."CompanyName" = 'Speedy Express';
```

<br/>