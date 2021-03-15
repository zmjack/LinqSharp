# LinqSharp

[● 返回列表](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

<br/>

## 数据注解

**LinqSharp** 为 **CodeFirst** 模型提供了更多且易于使用的数据注解：

- 表设计数据注解
- **字段标准化数据注解**（本文）

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

目前包括：

- **AutoCondensed**（使用紧凑字符串）
- **AutoCreationTime**（自动设置创建时间）
- **AutoLastWriteTime**（自动设置修改时间）
- **AutoLower**（转换为小写字母）
- **AutoUpper**（转换为大写字母）
- **AutoTrim**（去除字符串边界空格）

<br/>

例如：

```csharp
public class TrackModel : IEntity<TrackModel>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [AutoCreationTime]
    public DateTime CreationTime { get; set; }

    [AutoLastWriteTime]
    public DateTime LastWriteTime { get; set; }

    [AutoTrim]
    public string ForTrim { get; set; }

    [AutoUpper]
    public string ForUpper { get; set; }

    [AutoLower]
    public string ForLower { get; set; }

    [AutoCondensed]
    public string ForCondensed { get; set; }
}
```

使用插入操作：

```csharp
using var context = ApplicationDbContext.UseMySql();

var model = new TrackModel
{
    ForTrim = "   127.0.0.1 ",
    ForLower = "LinqSharp",
    ForUpper = "LinqSharp",
    ForCondensed = "  Welcome to  use   LinqSharp  ",
};
context.TrackModels.Add(model);
context.SaveChanges();

Assert.Equal("127.0.0.1", model.ForTrim);
Assert.Equal("linqsharp", model.ForLower);
Assert.Equal("LINQSHARP", model.ForUpper);
Assert.Equal("Welcome to use LinqSharp", model.ForCondensed);
```

