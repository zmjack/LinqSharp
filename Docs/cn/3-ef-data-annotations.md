# LinqSharp

[● 返回列表](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

<br/>

## 数据注解

**LinqSharp** 为 **CodeFirst** 模型提供了更多且易于使用的数据注解，分为：

- 表设计数据注解
- 数据标准化数据注解

表设计数据注解，是调用 **Flunt API** 的替代方案，方便编写 **CodeFirst** 模型。

<br/>

### 表设计数据注解

表设计数据注解解析的内部实现是对 **Flunt API** 进行调用。

因此，使用相关功能实现，需要重写 **DbContext** 下的 **OnModelCreating** 方法：

```csharp
public class ApplicationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        LinqSharpEF.OnModelCreating(this, base.OnModelCreating, modelBuilder);
    }
}
```

<br/>

#### 索引（Index）

索引，是对数据库表中一列或多列值进行排序，以达到快速访问数据库表中的特定数据的能力。

**Entity Framework** 没有提供索引的数据注解，但在 **Flunt API** 提供了相应功能。

为了简化设计，**LinqSharp** 提供了 **IndexAttribute** 的数据注解实现。

<br/>

假设，我们要创建这样的表：

- **Id**：主键
- **Int0**：非聚集索引
- **Int1**：唯一索引
- **Int2_G1**：和 **Int3_G1** 组成 唯一索引
- **Int3_G1**：和 **Int2_G1** 组成 唯一索引

| Field       | Runtime Type | Max Length | Index                    | Required |
| ----------- | ------------ | ---------- | ------------------------ | -------- |
| **Id**      | Guid         |            | Key                      |          |
| **Int0**    | int          |            | Normal                   |          |
| **Int1**    | int          |            | Unique                   |          |
| **Int2_G1** | int          |            | Unique (Int2_G1&Int3_G1) |          |
| **Int3_G1** | int          |            | Unique (Int2_G1&Int3_G1) |          |

如果使用 **Flunt API** 设计，我们就需要为指定表的每个索引字段进行定义：

```csharp
public class LS_Index
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int Int0 { get; set; }
    public int Int1 { get; set; }
    public int Int2_G1 { get; set; }
    public int Int3_G1 { get; set; }
}
```

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<LS_Index> LS_Indices { get; set; }        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity("LS_Index").HasIndex("Int0");
        modelBuilder.Entity("LS_Index").HasIndex("Int1").IsUnique();
        modelBuilder.Entity("LS_Index").HasIndex("Int2_G1", "Int3_G1").IsUnique();
    }
}
```

而使用 **Data Annotation** 设计，则仅需要使用 **IndexAttribute**：

```csharp
public class LS_Index
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Index(IndexType.Normal)]
    public int Int0 { get; set; }

    [Index(IndexType.Unique)]
    public int Int1 { get; set; }

    [Index(IndexType.Unique, Group = "Int2_G1&Int3_G1")]
    public int Int2_G1 { get; set; }

    [Index(IndexType.Unique, Group = "Int2_G1&Int3_G1")]
    public int Int3_G1 { get; set; }
}
```

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<LS_Index> LS_Indices { get; set; }        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        LinqSharpEF.OnModelCreating(this, base.OnModelCreating, modelBuilder);
    }
}
```

如果使用 **MySQL**，这段代码会生成如下索引：

| 名                                | 字段             | 索引类型 | 索引方法 |
| --------------------------------- | ---------------- | -------- | -------- |
| **IX_LS_Indices_Int0**            | Int0             | NORMAL   | BTREE    |
| **IX_LS_Indices_Int1**            | Int1             | UNIQUE   | BTREE    |
| **IX_LS_Indices_Int2_G1_Int3_G1** | Int2_G1, Int3_G1 | UNIQUE   | BTREE    |

<br/>

#### 自定义数据存储（Provider）

自定义数据存储是为复杂数据提供对象化地解析的功能，内部实现是对 **Flunt API - HasConversion** 的调用。

例如，设计一个 **CodeFirst** 模型，满足：

- 字段 **NameModel** 读取类型是 **NameModel**，存储为 **JSON**；
- 字段 **Password** 读取值是实际值，存储为 **BASE64**。



使用 **ProviderAttribute** 为指定字段进行定义：

```csharp
public class LS_Provider
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(127)]
    [Provider(typeof(PasswordProvider))]
    public string Password { get; set; }

    [StringLength(127)]
    [Provider(typeof(JsonProvider<NameModel>))]
    public NameModel NameModel { get; set; }

    public class PasswordProvider : IProvider<string, string>
    {
        public override string ReadFromProvider(string value) 
            => value.Flow(StringFlow.FromBase64);
        public override string WriteToProvider(string model) 
            => model.Flow(StringFlow.Base64);
    }
}
```

**JsonProvider** 是 **LinqSharp** 的内建转换器，作用是储存 **JSON** 化。

自定义转换器类需要继承接口 **IProvider<TModel, TProvider>**，其中 **TModel** 为对象类型，**TProvider** 为储存类型。

**IProvider** 接口包含两个方法：

- “将数据写入储存介质”的转换方法：**WriteToProvider(TModel model)**；
- “从储存介质读取数据”的转换方法：**ReadFromProvider(TModel model)**；

例如，**JsonProvider** 的实现：

```csharp
public class JsonProvider<TModel> : IProvider<TModel, string>
{
    public override TModel ReadFromProvider(string value) 
        => (TModel)JsonConvert.DeserializeObject(value, typeof(TModel));
    public override string WriteToProvider(TModel model) 
        => JsonConvert.SerializeObject(model);
}
```

使用 **Provider**，只需要将 **Provider** 绑定到属性字段即可。例如：

```csharp
[Provider(typeof(JsonProvider<NameModel>))]
public NameModel NameModel { get; set; }
```

测试例子：

```csharp
using (var context = ApplicationDbContext.UseMySql())
{
    context.LS_Providers.Add(new LS_Provider
    {
        Password = "0416",			// BASE64 储存
        NameModel = new NameModel   // JSON 储存
        { 
            Name = "Jack",
            NickName = "zmjack",
        },
    });
    context.SaveChanges();
}
```
数据库记录：

| Id                                   | Password | NameModel                           |
| ------------------------------------ | -------- | ----------------------------------- |
| 0e589993-0b20-4d88-8616-d62b21300c39 | MDQxNg== | {"Name":"Jack","NickName":"zmjack"} |

<br/>

