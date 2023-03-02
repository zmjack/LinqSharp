## 用于 Key-Value 的代理查询

代理查询（**Agent Query**）属于动态查询的特殊构建方式。

它可以将特定实体（**Entity**）的每个属性映射为对数据行（**Row**）的访问，可用于对 **Key-Value** 存储结构的便捷访问。

<br/>

### 示例

1. 定义 **Key-Value** 表实体 **AppRegistryEntity**：

```csharp
public class AppRegistryEntity : KeyValueEntity { }
```
```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<AppRegistryEntity> AppRegistries { get; set; }
}
```
2. 定义代理实体 **AppRegistry**：

```csharp
public class AppRegistry : KeyValueAgent<AppRegistryEntity>
{
    public virtual string Theme { get; set; } = "Default";
    public virtual string Color { get; set; }

    public virtual int Volume { get; set; }
    public virtual DateTime? LoginTime { get; set; }

    public virtual bool Lock { get; set; }
}
```
3. 查询操作

```csharp
using (var context = ApplicationDbContext.UseMySql())
using (var query = context.BeginAgentQuery(x => x.AppRegistries))
{
    var jerry = query.GetAgent<AppRegistry>("/User/Jerry");
    
    jerry.Theme = "Sky";
    jerry.Color = "brown";
    jerry.Volume = 10;
    jerry.LoginTime = now;
    jerry.Lock = true;

    context.SaveChanges();
}
```
4. 执行结果

| Id       | Item        | Key           | Value             |
| -------- | ----------- | ------------- | ----------------- |
| \<Guid\> | /User/Jerry | **Theme**     | Sky               |
| \<Guid\> | /User/Jerry | **Color**     | brown             |
| \<Guid\> | /User/Jerry | **Volume**    | 10                |
| \<Guid\> | /User/Jerry | **LoginTime** | 2023/1/1 15:00:00 |
| \<Guid\> | /User/Jerry | **Lock**      | True              |