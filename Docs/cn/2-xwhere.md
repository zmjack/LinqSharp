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
var query = mysql.Employees.Where(x =>
    (x.TitleOfCourtesy == "Mr." && x.City == "London")
    || (x.TitleOfCourtesy == "Ms." && x.City == "Seattle"));
```

如果是 **不定量** 数据，那么我们需要

1. 使用 **for / foreach** 为每组数据创建查询子条件；
2. 把所有查询子条件使用 **and / or** 连接起来进行查询。

<br/>

**LinqSharp** 提供了 **XWhere** 方法支持这样的动态查询。

<br/>

### 动态 LINQ

- **XWhere**：动态构建查询树。

按照上节给出的查询场景举例：

1. 定义输入数据：

```csharp
var searches = new[] { ("Mr.", "London"), ("Ms.", "Seattle") };
```

2. 在 **XWhere** 函数中将 **searches** 的每个单元 **s** 转换成单元表达式 **parts**，然后使用 **Or** 方法连接每个 **part**：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    var parts = searches.Select(s =>
        h.Where(x => x.TitleOfCourtesy == s.Item1 && x.City == s.Item2));
    return h.Or(parts);
});
```

更简单些，我们可以使用 **h.Or** 方法的另一个重载：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(searches,
        s => x => x.TitleOfCourtesy == s.Item1 && x.City == s.Item2);
});
```

**注：Visual Studio** 分析器似乎不能很好地为这个表达式工作，直到您完整地编写完整条语句，它将是正确的。

3. 执行查询：

```csharp
// 示例等效于 Where 语法
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

**XWhere** 也支持使用运算符或连接函数来合并表达式单元，并且支持 **LinqSharp** 基础扩展（**Search** 函数等）。

使用 **运算符**：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Where(x => x.TitleOfCourtesy == "Mr." && x.City == "London")
         | h.Where(x => x.TitleOfCourtesy == "Ms." && x.City == "Seattle");
});
```

使用 **函数**：

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

使用 **函数** 的好处是

- 可以在 **for / foreach** 或其他逻辑结构中动态构建。

<br/>

灵活运用这两种方式，是构建动态 LINQ 查询的基本方式。

<br/>

#### 复杂点的例子

例如，我们在使用 **Where** 的同时还使用 **Search**，最终将使用 **And** 连接起来。

在这个案例，我们将混合使用 **运算符** 和 **函数** 来构建查询：

```csharp
var query = sqlite.Employees.XWhere(h =>
{
    return h.Or(
        h.Where(x => x.TitleOfCourtesy == "Mr." && x.City == "London"),
        h.Where(x => x.TitleOfCourtesy == "Ms." && x.City == "Seattle")
    ) & h.Search("m", e => new
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
AND 
(
    ("e"."Address" IS NOT NULL AND (('m' = '') OR (instr("e"."Address", 'm') > 0))) 
    OR 
    ("e"."City" IS NOT NULL AND (('m' = '') OR (instr("e"."City", 'm') > 0)))
);
```

<br/>

#### 动态属性查询

想通过给定字符串，对指定字段进行查询？当然可以！动态属性查询就是支持这项功能的。

例如，我们给定查询数据定义，在 **City** 查询 **Londom**，在 **FirstName** 中查询 **Andrew**：

```csharp
var searches = new[] { ("City", "London"), ("FirstName", "Andrew") };
```

然后在在 **Employees** 表中进行查询：





