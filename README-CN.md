# LinqSharp

其他语言：[English](https://github.com/zmjack/LinqSharp/blob/master/README.md)

<br/>

**LinqSharp** 是个开源 **LINQ** 扩展库，它允许您编写简单代码来生成复杂查询，包括查询扩展和动态查询生成。

**LinqSharp.EFCore** 是对 **EntityFramework** 的增强库，提供更多数据注解、数据库函数及自定义储存规则等。

<br/>

**LinqSharp** 可为 **LINQ** 提供如下方面的增强：

- [查询扩展](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/1-introduce.md)
- [动态 LINQ](https://github.com/zmjack/LinqSharp/blob/master/Docs/cn/2-xwhere.md)



**LinqSharp.EFCore** 可为 **Entity Frameowk** 提供如下方面的增强：

- <font color="orange">[暂无文档]</font> 更多数据库生成注释
- <font color="orange">[暂无文档]</font> 自定义字段储存
- <font color="orange">[暂无文档]</font> 数据库自定义函数映射
- <font color="orange">[暂无文档]</font> 自定义存储扩展
- <font color="orange">[暂无文档]</font> 列式存储代理

<br/>

**支持的 Entity Framework 版本：**

- Entity Framework Core 3.1
- Entity Framework Core 2.1+



 **受限支持的 Entity Framework 版本：**

- Entity Framework Core 3.0：1 项失败。

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

```sqlite
SELECT "x"."ShipperID", "x"."CompanyName", "x"."Phone"
FROM "Shippers" AS "x"
WHERE "x"."CompanyName" = 'Speedy Express';
```

<br/>