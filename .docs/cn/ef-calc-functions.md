## 计算函数

**LinqSharp.EFCore** 为数据操作提供了更多计算函数：

- 字符串连接（**Concat**）
- 构建日期（**DateTime**）
- 随机数（**Random**）

| Calculation Function | Firebird | IBM  | MySql | Oracle | PostgreSQL | SqlServer |
| -------------------- | :------: | :--: | :---: | :----: | :--------: | :-------: |
| DbFunc.**Concat**    |    🔘     |  🔘   |   ✔️   |   ✔️    |     ✔️      |     ✔️     |
| DbFunc.**DateTime**  |    🔘     |  🔘   |   ✔️   |   🔘    |     🔘      |     ✔️     |
| DbFunc.**Random**    |    🔘     |  🔘   |   ✔️   |   ✔️    |     ✔️      |     ✔️     |

| Direct Action       | Jet  | Sqlite | SqlServer<br />Compact35 | SqlServer<br />Compact40 |
| ------------------- | :--: | :----: | :----------------------: | :----------------------: |
| DbFunc.**Concat**   |  ❌   |   ❌    |            ✔️             |            ✔️             |
| DbFunc.**DateTime** |  ❌   |   ❌    |            ✔️             |            ✔️             |
| DbFunc.**Random**   |  ✔️   |   ✔️    |            ✔️             |            ✔️             |

<br/>

