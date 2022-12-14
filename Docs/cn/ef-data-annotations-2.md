# LinqSharp

[● 返回列表](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

<br/>

## 数据特性

**LinqSharp.EFCore** 为 **CodeFirst** 模型提供了更多且易于使用的数据注解：

- 表设计数据注解
- **字段标准化数据特性**（本文）

表设计数据注解，是调用 **Flunt API** 的替代方案，方便编写 **CodeFirst** 模型。

<br/>

### 字段标准化数据注解

重写 **DbContext** 下的 **OnModelCreating** 方法以启用 **LinqSharp** 扩展功能：

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

#### AutoX

**Auto** 系列注释是为数据提交到数据库前，用来规范数据值的自动处理标记。

目前内置标记包括：

| 类名                  | 说明           | 支持的属性                | 生效规则        |
| --------------------- | -------------- | ------------------------- | --------------- |
| **AutoCreationTime**  | 使用创建时间   | DateTime / DateTimeOffset | Added           |
| **AutoLastWriteTime** | 自动修改时间   | DateTime / DateTimeOffset | Added, Modified |
| **AutoCondensed**     | 使用紧凑字符串 | string                    | Added, Modified |
| **AutoLower**         | 转换为小写字母 | string                    | Added, Modified |
| **AutoUpper**         | 转换为大写字母 | string                    | Added, Modified |
| **AutoTrim**          | 去除边界空格   | string                    | Added, Modified |

如果需要自定义，应创建 **AutoAttribute** 的子类型。

例如，设计一个 **Attribute**，用于转换奇数值（若 `value` 为偶数，返回原值，否则返回 `valule * 2`）：

```c#
// If `value` is odd, return `value * 2`, otherwise return `value`.
public class AutoEvenAttribute : AutoAttribute
{
    public AutoEvenAttribute() : base(EntityState.Added, EntityState.Modified) { }

    public override object Format(object entity, Type propertyType, object value)
    {
        if (propertyType != typeof(int))
        {
            throw Exception_NotSupportedTypes(propertyType, nameof(propertyType));
        }

        if (value is int @int && @int % 2 == 1)
            return @int * 2;
        else return 0;
    }
}
```
<br/>

举例模型定义：

```csharp
public class Model : IEntity<TrackModel>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [AutoTrim]
    public string ForTrim { get; set; }

    [AutoUpper]
    public string ForUpper { get; set; }

    [AutoLower]
    public string ForLower { get; set; }

    [AutoCondensed]
    public string ForCondensed { get; set; }
    
    [AutoEven]
    public int ForEven { get; set; }
}
```

创建实例模型：

```csharp
var model = new Model
{
    ForTrim = "   127.0.0.1 ",
    ForLower = "LinqSharp",
    ForUpper = "LinqSharp",
    ForCondensed = "  Welcome to  use   LinqSharp  ",    
    ForEven = 101,
};
```

在 **SaveChanges** 后，**Model** 中的属性将被设置为：

```c#
var model = new Model
{
    // "   127.0.0.1 " -> "127.0.0.1"
    ForTrim = "127.0.0.1",
    
    // "LinqSharp" -> "linqsharp"
    ForLower = "linqsharp",
    
    // "LinqSharp" -> "LINQSHARP"
    ForUpper = "LINQSHARP",
    
    // "  Welcome to  use   LinqSharp  " -> "Welcome to use LinqSharp"
    ForCondensed = "Welcome to use LinqSharp",
    
    // 101 -> 202
    ForEven = 202,
};
```

