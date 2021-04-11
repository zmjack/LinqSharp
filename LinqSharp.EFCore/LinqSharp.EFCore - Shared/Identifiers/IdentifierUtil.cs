using System;

namespace LinqSharp.EFCore.Identifiers
{
    public static class IdentifierUtil
    {
        public static IdentifierPair? GetDelimitedIdentifiers(DatabaseProviderName name)
        {
            return name switch
            {
                DatabaseProviderName.Cosmos => null,
                DatabaseProviderName.Firebird => new IdentifierPair('"', '"'),
                DatabaseProviderName.IBM => new IdentifierPair('"', '"'),
                DatabaseProviderName.Jet => new IdentifierPair('[', ']'),
                DatabaseProviderName.MyCat => new IdentifierPair('`', '`'),
                DatabaseProviderName.MySql => new IdentifierPair('`', '`'),
                DatabaseProviderName.OpenEdge => null,
                DatabaseProviderName.Oracle => new IdentifierPair('"', '"'),
                DatabaseProviderName.PostgreSQL => new IdentifierPair('"', '"'),
                DatabaseProviderName.Sqlite => new IdentifierPair('"', '"'),
                DatabaseProviderName.SqlServer => new IdentifierPair('[', ']'),
                DatabaseProviderName.SqlServerCompact35 => new IdentifierPair('[', ']'),
                DatabaseProviderName.SqlServerCompact40 => new IdentifierPair('[', ']'),
                DatabaseProviderName.Unknown => null,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
