## 直接访问

**LinqSharp.EFCore** 为数据操作提供了直接访问函数：

- 清空表（**TRUNCATE TABLE**）
- 批量导入（**BULK INSERT**）

支持情况：

| Direct Action      | Firebird | IBM  | MySql | Oracle | PostgreSQL | SqlServer |
| ------------------ | :------: | :--: | :---: | :----: | :--------: | :-------: |
| **TRUNCATE TABLE** |    ✔️     |  ✔️   |   ✔️   |   ✔️    |     ✔️      |     ✔️     |
| **BULK INSERT**    |    🔘     |  🔘   |   ✔️   |   🔘    |     🔘      |     ✔️     |

| Direct Action      | Jet  | Sqlite | SqlServer<br />Compact35 | SqlServer<br />Compact40 |
| ------------------ | :--: | :----: | :----------------------: | :----------------------: |
| **TRUNCATE TABLE** |  ✔️   |   ✔️    |            ✔️             |            ✔️             |
| **BULK INSERT**    |  ❌   |   ❌    |            ❌             |            ❌             |

<br/>

### 清空表（TRUNCATE TABLE）

清空表（**TRUNCATE TABLE**）能够快速移除表中所有行记录。

**SqlServer** 使用示例：

```csharp
using var context = ApplicationDbContext.UseSqlServer();
using (context.BeginDirectScope())
{
    context.BulkTestModels.Truncate();
}
```

执行 SQL：

```sql
TRUNCATE TABLE [BulkTestModels];
```

<br/>

### 批量导入（BULK INSERT）

批量导入（**BULK INSERT**）允许使用一个源数据批量写入到数据表。

数据库支持情况：


| 支持库                         | 数据库                    |
| ------------------------------ | ------------------------- |
| **LinqSharp.EFCore.MySql**     | ✔️ **MySql**               |
| **LinqSharp.EFCore.SqlServer** | ✔️ **Jet** ✔️ **SqlServer** |

**SqlServer** 使用示例：

引用库：

```powershell
dotnet add package LinqSharp.EFCore.SqlServer
```

在 **LinqSharpEFRegister** 中注册工具类：

```csharp
LinqSharpEFRegister.RegisterBulkCopyEngine(
    DatabaseProviderName.SqlServer, new SqlServerBulkCopyEngine());
```

使用批量导入（**BulkInsert**）:

```csharp
using var context = ApplicationDbContext.UseSqlServer();
var models = new BulkTestModel[100000].Let(i => new BulkTestModel
{
    Id = Guid.NewGuid(),
    Code = Path.GetRandomFileName(),
    Name = Path.GetRandomFileName().Bytes().For(BytesFlow.Base58),
});

using (context.BeginDirectScope())
{
    context.BulkTestModels.Truncate();      // Truncate table
    context.BulkTestModels.BulkInsert(models);
}
```

**注意**，**BULK INSERT** 不同于 **INSERT INTO**，如遇到错误数据（键冲突或违反约束等）无法导入会直接跳过，且不抛出错误提示，您可能需要编写额外代码进行检查。

<br/>

