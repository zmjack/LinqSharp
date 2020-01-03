using Microsoft.EntityFrameworkCore;

namespace NLinq
{
    public static class XDbContext
    {
        public static DatabaseProviderName GetProviderName(this DbContext @this)
        {
            switch (@this.Database.ProviderName)
            {
                case string name when name.Contains(DatabaseProviderName.Cosmos.ToString()): return DatabaseProviderName.Cosmos;
                case string name when name.Contains(DatabaseProviderName.Firebird.ToString()): return DatabaseProviderName.Firebird;
                case string name when name.Contains(DatabaseProviderName.IBM.ToString()): return DatabaseProviderName.IBM;
                case string name when name.Contains(DatabaseProviderName.Jet.ToString()): return DatabaseProviderName.Jet;
                case string name when name.Contains(DatabaseProviderName.MyCat.ToString()): return DatabaseProviderName.MyCat;
                case string name when name.Contains(DatabaseProviderName.MySql.ToString()): return DatabaseProviderName.MySql;
                case string name when name.Contains(DatabaseProviderName.OpenEdge.ToString()): return DatabaseProviderName.OpenEdge;
                case string name when name.Contains(DatabaseProviderName.Oracle.ToString()): return DatabaseProviderName.Oracle;
                case string name when name.Contains(DatabaseProviderName.PostgreSQL.ToString()): return DatabaseProviderName.PostgreSQL;
                case string name when name.Contains(DatabaseProviderName.Sqlite.ToString()): return DatabaseProviderName.Sqlite;
                case string name when name.Contains(DatabaseProviderName.SqlServer.ToString()): return DatabaseProviderName.SqlServer;
                case string name when name.Contains(DatabaseProviderName.SqlServerCompact35.ToString()): return DatabaseProviderName.SqlServerCompact35;
                case string name when name.Contains(DatabaseProviderName.SqlServerCompact40.ToString()): return DatabaseProviderName.SqlServerCompact40;
                default: return DatabaseProviderName.Unknown;
            }
        }

    }
}
