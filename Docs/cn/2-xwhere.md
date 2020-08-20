# LinqSharp

**LinqSharp** 是个开源的 **LINQ** 扩展库，它允许您编写简单代码来生成复杂查询，包括常用查询扩展和动态查询生成。

**LinqSharp.EFCore** 是对 **EntityFramework** 的增强库，提供更多数据注解、数据库函数及自定义储存规则等。

- [English Readme](https://github.com/zmjack/LinqSharp/blob/master/README.md)
- [中文自述](https://github.com/zmjack/LinqSharp/blob/master/README-CN.md)

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

如果是 **不定量** 数据，那么我们需要

1. 使用 **for / foreach** 为每组数据创建查询子条件；
2. 把所有查询子条件使用 **and / or** 连接起来进行查询。

<br/>

**LinqSharp** 提供了 **XWhere** 方法支持这样的动态查询。

<br/>

### 动态条件查询

- **XWhere**：动态构建查询树。

按照上节给出的场景举例：

首先定义输入数据：

```csharp
var searches = new[] { ("Mr.", "London"), ("Ms.", "Seattle") };
```

随后在 **XWhere** 函数中将 **searches** 的每个单元 **s** 转换成单元表达式 **parts**，然后使用 **Or** 方法连接每个 **part**：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    var parts = searches.Select(s =>
        h.Where(x => x.TitleOfCourtesy == s.Item1 || x.City == s.Item2));
    return h.Or(parts);
});
```

更简单些，我们可以使用 **h.Or** 方法的另一个重载来编写代码：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(searches,
        s => x => x.TitleOfCourtesy == s.Item1 || x.City == s.Item2);
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

执行等效查询：

```csharp
// 示例等效于原生 Where 语法
sqlite.Employees.Where(x =>
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

使用 **运算符**：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Where(x => x.TitleOfCourtesy == "Mr." && x.City == "London")
         | h.Where(x => x.TitleOfCourtesy == "Ms." && x.City == "Seattle");
});
```

使用 **连接函数**：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(
        h.Where(x => x.TitleOfCourtesy == "Mr." && x.City == "London"),
        h.Where(x => x.TitleOfCourtesy == "Ms." && x.City == "Seattle"));
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

例如，我们在使用 **Where** 的同时还使用 **Search**，使用 **Or** 连接起来。

在这个案例中，我们将混合使用 **运算符** 和 **函数** 来构建查询：

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

#### 动态属性查询

动态属性是通过给定字符串，解析为对指定字段的查询。

例如，下面这个例子：

- 在 **City** 查询 **Londom**，在 **FirstName** 中查询 **Andrew**；
- 使用 **And** 连接起来。

定义输入数据（例如在 **City** 查询 **Londom**，在 **FirstName** 中查询 **Andrew**，使用 **Or** 连接起来）：

```csharp
var searches = new[] { ("City", "London"), ("FirstName", "Andrew") };
```

然后在 **Employees** 表中进行查询：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(searches, s => h.Property<string>(s.Item1) == s.Item2);
});
```

生成 SQL：

```sql
SELECT *
FROM "Employees" AS "e"
WHERE ("e"."City" = 'London') Or ("e"."FirstName" = 'Andrew');
```

<br/>

我们甚至允许动态属性值参与运算：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(searches, s => (h.Property<string>(s.Item1) + "!") == s.Item2);
});
```

生成 SQL：

```sql
SELECT *
FROM "Employees" AS "e"
WHERE (("e"."City" || '!') = 'London') OR (("e"."FirstName" || '!') = 'Andrew');
```

<br/>

---

是不是很棒呢？