using System;

namespace LinqSharp.EFCore.Test
{
    [Flags]
    public enum TestDatabases
    {
        SqlServer = 1,
        MySql = 2,
        Sqlite = 4,
        All = SqlServer | MySql | Sqlite,
    }
}
