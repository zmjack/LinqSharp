# LinqSharp

[● 返回列表](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

<br/>

## 乐观并发冲突

乐观并发包括乐观地尝试将实体保存到数据库，希望数据在加载实体后未发生更改。 如果事实证明数据已更改，则会引发异常，必须在尝试再次保存之前解决冲突。

<br/>

### 何时发生

以下表（表名：**ConcurrencyModels**）为例：

| 列名             | 类型 | 特性               |
| ---------------- | ---- | ------------------ |
| Id               | Guid | 主键               |
| Value            | int  | 默认（客户端优先） |
| ClientWinValue   | int  | 客户端优先         |
| DatabaseWinValue | int  | 数据库优先         |
| RowVersion       | int  | 并发检查           |

现有数据记录：

| Id                                   | Value | ClientWinValue | DatabaseWinValue | RowVersion |
| ------------------------------------ | ----- | -------------- | ---------------- | ---------- |
| 314ce3b7-e6b9-46a4-81a4-b5b140653189 | 0     | 0              | 0                | 0          |

<br/>

假设现有两个客户端同时修改该条数据记录：

```mysql
UPDATE `ConcurrencyModels` 
SET `Value` = 1, ClientWinValue = 1, DatabaseWinValue = 1, RowVersion = 100 
WHERE Id = '314ce3b7-e6b9-46a4-81a4-b5b140653189' AND RowVersion = 0;
```

```mysql
UPDATE `ConcurrencyModels` 
SET `Value` = 2, ClientWinValue = 2, DatabaseWinValue = 2, RowVersion = 200 
WHERE Id = '314ce3b7-e6b9-46a4-81a4-b5b140653189' AND RowVersion = 0;
```

任意一条更新语句执行后（受影响条目为 **1**），**RowVersion** 值将会改变为新值，这将导致另一条更新语句执行后不会任何效果（受影响条目为 **0**）。

<br/>

**EFCore** 在处理并发冲突时，若受影响条目为 **0**，则会抛出异常（**DbUpdateConcurrencyException**）：

```powershell
'The database operation was expected to affect 1 row(s), but actually affected 0 row(s); '
```

<br/>

### 解决并发冲突

以上文为例，若 `RowVersion = 100` 的更新语句先执行，受影响条目则为 **1**：

```mysql
UPDATE `ConcurrencyModels` 
SET `Value` = 1, ClientWinValue = 1, DatabaseWinValue = 1, RowVersion = 100 
WHERE Id = '314ce3b7-e6b9-46a4-81a4-b5b140653189' AND RowVersion = 0;
```

| Id                                                           | Value                                          | ClientWinValue                                 | DatabaseWinValue                               | RowVersion                                                   |
| ------------------------------------------------------------ | ---------------------------------------------- | ---------------------------------------------- | ---------------------------------------------- | ------------------------------------------------------------ |
| <font color="orange">314ce3b7-e6b9-46a4-81a4-b5b140653189</font> | ~~0~~<br/><font color="limegreen">**1**</font> | ~~0~~<br/><font color="limegreen">**1**</font> | ~~0~~<br/><font color="limegreen">**1**</font> | <font color="orange">~~0~~</font><br/><font color="limegreen">**100**</font> |

那么，执行另一条语句 `RowVersion = 200` 执行时，受影响条目则为 **0**：

```mysql
UPDATE `ConcurrencyModels` 
SET `Value` = 2, ClientWinValue = 2, DatabaseWinValue = 2, RowVersion = 200 
WHERE Id = '314ce3b7-e6b9-46a4-81a4-b5b140653189' AND RowVersion = 0;
```

| Id                                                           | Value | ClientWinValue | DatabaseWinValue | RowVersion                   |
| ------------------------------------------------------------ | ----- | -------------- | ---------------- | ---------------------------- |
| <font color="orange">314ce3b7-e6b9-46a4-81a4-b5b140653189</font> | 1     | 1              | 1                | <font color="red">100</font> |

<br/>

对于发生更新冲突的语句，由于部分值已经发生改变，通常会有 **客户端优先**、**数据库优先** 两种处理方式：

- 客户端优先（**ClientWins**）：使用新值覆盖已存储的值。
- 数据库优先（**DatabaseWins**）：保留已存储的值。

<br/>

为了正确更新条目，我们必须修正第二条语句后进行重试：

- 客户端优先：

  ```mysql
UPDATE `ConcurrencyModels` 
    SET `Value` = 2, ClientWinValue = 2, DatabaseWinValue = 2, RowVersion = 200 
    WHERE Id = '314ce3b7-e6b9-46a4-81a4-b5b140653189' AND RowVersion = 100;
  ```
  
  | Id                                                           | Value                                          | ClientWinValue                                 | DatabaseWinValue                               | RowVersion                                                   |
| ------------------------------------------------------------ | ---------------------------------------------- | ---------------------------------------------- | ---------------------------------------------- | ------------------------------------------------------------ |
  | <font color="orange">314ce3b7-e6b9-46a4-81a4-b5b140653189</font> | ~~1~~<br/><font color="limegreen">**2**</font> | ~~1~~<br/><font color="limegreen">**2**</font> | ~~1~~<br/><font color="limegreen">**2**</font> | <font color="orange">~~100~~</font><br/><font color="limegreen">**200**</font> |

- 数据库优先

    ```mysql
    UPDATE `ConcurrencyModels` 
    SET `Value` = 2, ClientWinValue = 2, RowVersion = 200 
    WHERE Id = '314ce3b7-e6b9-46a4-81a4-b5b140653189' AND RowVersion = 100;
    ```

    | Id                                                           | Value                                          | ClientWinValue                                 | DatabaseWinValue | RowVersion                                                   |
    | ------------------------------------------------------------ | ---------------------------------------------- | ---------------------------------------------- | ---------------- | ------------------------------------------------------------ |
    | <font color="orange">314ce3b7-e6b9-46a4-81a4-b5b140653189</font> | ~~1~~<br/><font color="limegreen">**2**</font> | ~~1~~<br/><font color="limegreen">**2**</font> | 1                | <font color="orange">~~100~~</font><br/><font color="limegreen">**200**</font> |

<br/>

### 使用 LinqSharp.EFCore 的自动解决冲突

1. 为 **DbContext** 实现 **IConcurrencyResolvableContext** 接口：

   ```csharp
   public class ApplicationDbContext : IConcurrencyResolvableContext
   {
       // Larger values will reduce update conflicts,
       // but will make more retries.
       // It is recommended to set a smaller value according to the actual situation.
       
       // 越大的值将减少更新冲突，但会进行更多重试。
       // 建议根据实际情况设置更小的值。
       
       public int MaxConcurrencyRetry => 3;
   }
   ```

2. 标记需要解决冲突的模型：

   使用特性 **ConcurrencyResolvable** 标记模型类，**ConcurrencyPolicy** 标记字段：

   ```csharp
   [ConcurrencyResolvable]
   public class ConcurrencyModel
   {
       [Key]
       public Guid Id { get; set; }
   
       [ConcurrencyCheck]
       public int RowVersion { get; set; }
   
       public int Value { get; set; }
   
       [ConcurrencyPolicy(ConcurrencyResolvingMode.DatabaseWins)]
       public int DatabaseWinValue { get; set; }
   
       [ConcurrencyPolicy(ConcurrencyResolvingMode.ClientWins)]
       public int ClientWinValue { get; set; }
   }
   ```

   <font color="red">未使用 **ConcurrencyPolicy** 标记的字段默认为 客户端优先（**ClientWins**），故此示例中 **Value** 与 **ClientWinValue** 具有相同特性。</font>

3. 重写 **DbContext** 方法 **SaveChanges**、**SaveChangesAsync**：

   ```csharp
   public override int SaveChanges(bool acceptAllChangesOnSuccess)
   {
       return LinqSharpEF.SaveChanges(this, base.SaveChanges, acceptAllChangesOnSuccess);
   }
   
   public override Task<int> SaveChangesAsync(
       bool acceptAllChangesOnSuccess, 
       CancellationToken cancellationToken = default)
   {
       return LinqSharpEF.SaveChangesAsync(this, base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);
   }
   ```

<br/>

