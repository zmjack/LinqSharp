# LinqSharp.EFCore

## Function Provider Supports

| .NET Function        |               Jet                |              MySql               |              Oracle              |            PostgreSQL            |              Sqlite              |            SqlServer             |
| -------------------- | :------------------------------: | :------------------------------: | :------------------------------: | :------------------------------: | :------------------------------: | :------------------------------: |
| DbFunc. **Random**   | <font color="limegreen">✔</font> | <font color="limegreen">✔</font> | <font color="limegreen">✔</font> | <font color="limegreen">✔</font> | <font color="limegreen">✔</font> | <font color="limegreen">✔</font> |
| DbFunc. **Concat**   |    <font color="red">✘</font>    | <font color="limegreen">✔</font> | <font color="limegreen">✔</font> | <font color="limegreen">✔</font> |    <font color="red">✘</font>    | <font color="limegreen">✔</font> |
| DbFunc. **DateTime** |    <font color="red">✘</font>    | <font color="limegreen">✔</font> |    <font color="red">✘</font>    |    <font color="red">✘</font>    |    <font color="red">✘</font>    | <font color="limegreen">✔</font> |

<br/>

## DateTime

### SqlServer

Use:

```c#
.Where(x => DbFunc.DateTime(x.Year, x.Month, x.Day) > DbFunc.DateTime(2012, 4, 16));
```

```mssql
CONVERT(DATETIME, CONCAT([x].[Year], N'-', [x].[Month], N'-', [x].[Day])) > CONVERT(DATETIME, CONCAT(2012, N'-', 4, N'-', 16))
```

Not Use:

```c#
// Wrong
.Where(x => DbFunc.DateTime(x.Year, x.Month, x.Day) > new DateTime(2012, 4, 16));
```

```mssql
/* Wrong */
WHERE CONVERT(DATETIME, CONCAT([x].[Year], N'-', [x].[Month], N'-', [x].[Day])) > '2012-04-16T00:00:00.0000000'
```

### MySql

Everything is OK.

```c#
.Where(x => DbFunc.DateTime(x.Year, x.Month, x.Day) > DbFunc.DateTime(2012, 4, 16));
```

```mysql
WHERE STR_TO_DATE(CONCAT(`x`.`Year`, '-', `x`.`Month`, '-', `x`.`Day`), '%Y-%m-%d')
> STR_TO_DATE(CONCAT(2012, '-', 4, '-', 16), '%Y-%m-%d');
```

Or

```c#
.Where(x => DbFunc.DateTime(x.Year, x.Month, x.Day) > new DateTime(2012, 4, 16));
```

```mysql
WHERE STR_TO_DATE(CONCAT(`x`.`Year`, '-', `x`.`Month`, '-', `x`.`Day`), '%Y-%m-%d')
> '2012-04-16 00:00:00.000000';
```

