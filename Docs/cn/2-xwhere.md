# LinqSharp

[● 返回列表](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

<br/>

## 动态 LINQ

虽然 **LINQ** 已经可以适应多数的查询情景，但是我们依然需要动态 **LINQ** 来支持更多的查询场景。

例如，下面这个例子：

1. 案例需要接受输入的 **不定组数** 的数据，每组数据为 (**TitleOfCourtesy**，**City**)；
2. 查询 **Employees** 表中 **TitleOfCourtesy** 和 **City** 匹配输入数据的记录。

假设输入数据：

```csharp
var searches = new[] { ("Mr.", "London"), ("Ms.", "Seattle") };
```

如果是 **定量** 数据，我们很容易写出静态 **LINQ**：

```csharp
var query = sqlite.Employees.Where(x =>
    (x.TitleOfCourtesy == "Mr." && x.City == "London")
    || (x.TitleOfCourtesy == "Ms." && x.City == "Seattle"));
```

如果是 **不定量** 数据，那么就需要按以下逻辑进行

1. 使用 **for / foreach** 为每组数据创建查询子条件；
2. 把所有查询子条件使用 **and / or** 连接起来进行查询。

这样我们就需要使用 **动态LINQ** 查询。

<br/>

### 静态查询拆分

**XWhere** 是 **LinqSharp** 提供的动态查询方法，首先先了解一下如何将静态查询表达式拆分为动态查询。

例如，

```csharp
var query = sqlite.Employees.Where(
    x => (x.TitleOfCourtesy == "Mr." && x.City == "London")
      || (x.TitleOfCourtesy == "Ms." && x.City == "Seattle"));
```

使用 **XWhere** 改写为：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return
        h.Or(
            h.Where(x => x.TitleOfCourtesy == "Mr." && x.City == "London"),
            h.Where(x => x.TitleOfCourtesy == "Ms." && x.City == "Seattle"));
});
```

这样，我们就可以把各个查询条件拆分成单条语句，然后使用逻辑运算符 **And / Or** 将各条件连接起来，就可以生成完整的查询表达式。

<br/>

### 动态查询示例

- **XWhere**：动态构建查询树。

按照上节给出的场景举例：

1、首先定义输入数据：

```csharp
var searches = new[] { ("Mr.", "London"), ("Ms.", "Seattle") };
```

2、在 **XWhere** 函数中将 **searches** 转换成单元表达式 **parts**，然后使用 **Or** 方法连接每个 **part**：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    var parts = searches.Select(s =>
        h.Where(x => x.TitleOfCourtesy == s.Item1 && x.City == s.Item2));
    return h.Or(parts);
});
```

更简单些，我们可以使用 **h.Or** 方法的另一个重载来编写代码：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(searches,
        s => x => x.TitleOfCourtesy == s.Item1 && x.City == s.Item2);
});
```

或者，使用 **for / foreach** 构建：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    var exp = h.CreateEmpty();
    foreach (var s in searches)
    {
        exp |= h.Where(x => x.TitleOfCourtesy == s.Item1 && x.City == s.Item2);
    }
    return exp;
});
```

3、示例同等于等效查询：

```csharp
// 示例等效 Where 语法
var query = sqlite.Employees.Where(x =>
	(x.TitleOfCourtesy == "Mr." && x.City == "London")
	|| (x.TitleOfCourtesy == "Ms." && x.City == "Seattle"));
```

生成 SQL：

```sql
SELECT *
FROM "Employees" AS "x"
WHERE 
(("x"."TitleOfCourtesy" = 'Mr.') AND ("x"."City" = 'London')) 
OR 
(("x"."TitleOfCourtesy" = 'Ms.') AND ("x"."City" = 'Seattle'));
```

<br/>

#### 运算符

**XWhere** 也支持使用 **运算符** 或 **连接函数** 来合并表达式单元，并且支持 **LinqSharp** 扩展（**Search** 函数等）。

使用 **连接函数**：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(
        h.Where(x => x.TitleOfCourtesy == "Mr." && x.City == "London"),
        h.Where(x => x.TitleOfCourtesy == "Ms." && x.City == "Seattle"));
});
```

使用 **运算符**：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Where(x => x.TitleOfCourtesy == "Mr." && x.City == "London")
         | h.Where(x => x.TitleOfCourtesy == "Ms." && x.City == "Seattle");
});
```

或者混合使用。

<br/>

使用 **运算符** 的好处是

- 具有 **运算符优先级**；
- 可以使用 **括号** 自定义优先级。

使用 **连接函数** 的好处是

- 方便在 **for / foreach** 或其他逻辑结构中动态构建。

<br/>

灵活运用这两种方式，是构建 **动态LINQ** 查询的基本方式。

<br/>

#### 稍微复杂的例子

在上述案例中，假设我们在使用 **Where** 的同时还使用 **Search**，我们将混合使用 **运算符** 和 **函数** 来构建：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(
        h.Where(x => x.TitleOfCourtesy == "Mr." && x.City == "London"),
        h.Where(x => x.TitleOfCourtesy == "Ms." && x.City == "Seattle")
    ) | h.Search("m", e => new
    {
        e.Address,
        e.City,
    });
});
```

生成 SQL：

```sql
SELECT *
FROM "Employees" AS "e"
WHERE (
    (("e"."TitleOfCourtesy" = 'Mr.') AND ("e"."City" = 'London')) 
    OR 
    (("e"."TitleOfCourtesy" = 'Ms.') AND ("e"."City" = 'Seattle'))
) 
OR 
(
    ("e"."Address" IS NOT NULL AND (('m' = '') OR (instr("e"."Address", 'm') > 0))) 
    OR 
    ("e"."City" IS NOT NULL AND (('m' = '') OR (instr("e"."City", 'm') > 0)))
);
```

<br/>

#### 动态字段查询

动态字段查询，指通过解析 **指定字符串** 为 **字段名** 的查询方法。

例如，下面这个例子：

- 在多个指定字段中进行查询（例如，在 **City** 查询 **Londom**，在 **FirstName** 中查询 **Andrew**）；
- 将查询条件使用 **Or** 连接起来。.

固定条件下，可以使用以下语句：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(
        h.Where(x => x.City == "London"),
        h.Where(x => x.FirstName == "Adnrew"));
});
```

使用 **Property** 方法可以将 **字段名参数化**：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(
        h.Property<string>("City") == "London",
        h.Property<string>("FirstName") == "Adnrew");
});
```

**Property** 方法使用了 **LINQ** 的相似写法，是不是很方便呢？

<br/>

为支持更多应用场景，我们甚至允许 **动态属性值** 参与运算。

例如，将 **City** 值连接指定字符串 **!!**，然后与另一字符串进行比较。

静态语句为：

```csharp
var query = sqlite.Employees
    .Where(x => x.City + "!!" == "London!!");
```

动态语句为：

```csharp
var query = sqlite.Employees
    .XWhere(h => h.Property<string>("City") + "!!" == "London!!");
```

<br/>

#### 完整例子

了解基本用法后，来看一个完整例子。

- 输入数据为多组数据，每组数据为 “（字段名，值）”；
- 每组数据的生成条件使用 **And** 连接。

1、假设输入数据：

```csharp
var searches = new[] { ("City", "London"), ("FirstName", "Andrew") };
```

2、在 **Employees** 表中进行查询：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.And(searches, s => h.Property<string>(s.Item1) == s.Item2);
});
```

生成 SQL：

```sql
SELECT *
FROM "Employees" AS "e"
WHERE ("e"."City" = 'London') AND ("e"."FirstName" = 'Andrew');
```

<br/>

