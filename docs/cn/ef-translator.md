## 自定义转译器

使用解析器能够更细粒度地控制 **SQL** 生成，通常能够完成以下任务：

1. 绑定 **用户定义函数** 到 **.NET 函数** ，以提供更细粒度的 **SQL** 转译；
2. 提供更强的兼容性，使同一函数调用能够为不同目标数据库转译对应的 **SQL** 。

<br/>

先看一段用于 **随机取 2 条记录** 的错误例子：

```csharp
var random = new Random();	
(
    from supplier in Suppliers
    orderby random.NextDouble()
    select supplier.CompanyName
).Take(2).ToArray().Dump();	
```

在使用 **MySQL** 数据库的情况下，生成的 **SQL** 如下：

```sql
SELECT `@`.`CompanyName`
FROM `@Northwnd.Suppliers` AS `@`
ORDER BY (SELECT 1)
LIMIT 2
```

这里存在这几个问题：

- **orderyby** 子句 `random.NextDouble` 在求值器中只会执行一次，导致转译后为 `SELECT 1` ；
- **Random.NextDouble** 函数无法转译为 **MySQL** 的随机数函数 **RAND()**。
- 不同数据库的随机数函数可能不同，例如 **Sqlite** 为 **RANDOM()**。

<br/>

为了达成这个目标，我们需要创造一个转译器 **DbRandom**。

它包含以下内容：

- 继承于 **Translator** 类；
- 包含一个静态函数 **DbRandom.NextDouble** 用于绑定到目标数据库的 **随机函数**；
- 为不同的数据库配置各自的 **随机函数**。

下面是完整的例子：

```csharp
public class DbRandom : Translator
{
    private static readonly Random RandomInstance = new();
    public static double NextDouble() => RandomInstance.NextDouble();

    public DbRandom() { }

    public override void RegisterAll(ProviderName providerName, ModelBuilder modelBuilder)
    {
        switch (providerName)
        {
            case ProviderName.Jet:
                Register_RND(modelBuilder);
                break;

            case ProviderName.MyCat:
            case ProviderName.MySql:
            case ProviderName.SqlServer:
            case ProviderName.SqlServerCompact35:
            case ProviderName.SqlServerCompact40:
                Register_RAND(modelBuilder);
                break;

            case ProviderName.PostgreSQL:
            case ProviderName.Sqlite:
                Register_RANDOM(modelBuilder);
                break;

            case ProviderName.Oracle:
                Register_Oracle_RANDOM(modelBuilder);
                break;
        }
    }

    private void Register_RND(ModelBuilder modelBuilder)
    {
        Register(modelBuilder, () => NextDouble(), args => SqlTranslator.Function<double>("RND", args));
    }

    private void Register_RAND(ModelBuilder modelBuilder)
    {
        Register(modelBuilder, () => NextDouble(), args => SqlTranslator.Function<double>("RAND", args));
    }

    private void Register_RANDOM(ModelBuilder modelBuilder)
    {
        Register(modelBuilder, () => NextDouble(), args => SqlTranslator.Function<double>("RANDOM", args));
    }

    private void Register_Oracle_RANDOM(ModelBuilder modelBuilder)
    {
        Register(modelBuilder, () => NextDouble(), args => SqlTranslator.Function<double>("DBMS_RANDOM", "RANDOM", args));
    }
}
```

定义好转译器后，接下来需要绑定该转译器到 **DbContext** 上。

重写 **DbContext** 中的 **OnModelCreating** 方法：

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    LinqSharpEF.UseFuncProvider<DbRandom>(this, modelBuilder);
}
```

最后，调整一下代码，看看 **SQL** 语句是否已经正确生成：

```csharp
(
    from supplier in Suppliers
    orderby DbRandom.NextDouble()
    select supplier.CompanyName
).Take(2).ToQueryString().Dump();
```

```mysql
SELECT `@`.`CompanyName`
FROM `@Northwnd.Suppliers` AS `@`
ORDER BY RAND()
LIMIT 2
```

<br/>

事实上，随机函数映射已经在 **LinqSharp.EFCore** 中内置。

例如，上述例子也可以使用如下代码快速完成：

```csharp
(
    from supplier in Suppliers.Random(2)
    select supplier.CompanyName
).ToQueryString().Dump();
```

<br/>

