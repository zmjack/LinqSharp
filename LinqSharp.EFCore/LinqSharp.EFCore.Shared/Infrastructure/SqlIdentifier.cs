// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp.EFCore.Infrastructure;

public readonly struct SqlIdentifier
{
    public char LeftChar { get; }
    public char RightChar { get; }

    public SqlIdentifier(char leftChar, char rightChar)
    {
        LeftChar = leftChar;
        RightChar = rightChar;
    }

    public SqlIdentifier(ProviderName name)
    {
        switch (name)
        {
            case ProviderName.Cosmos: break;
            case ProviderName.Firebird: (LeftChar, RightChar) = ('"', '"'); break;
            case ProviderName.IBM: (LeftChar, RightChar) = ('"', '"'); break;
            case ProviderName.Jet: (LeftChar, RightChar) = ('[', ']'); break;
            case ProviderName.MyCat: (LeftChar, RightChar) = ('`', '`'); break;
            case ProviderName.MySql: (LeftChar, RightChar) = ('`', '`'); break;
            case ProviderName.OpenEdge: break;
            case ProviderName.Oracle: (LeftChar, RightChar) = ('"', '"'); break;
            case ProviderName.PostgreSQL: (LeftChar, RightChar) = ('"', '"'); break;
            case ProviderName.Sqlite: (LeftChar, RightChar) = ('"', '"'); break;
            case ProviderName.SqlServer: (LeftChar, RightChar) = ('[', ']'); break;
            case ProviderName.SqlServerCompact35: (LeftChar, RightChar) = ('[', ']'); break;
            case ProviderName.SqlServerCompact40: (LeftChar, RightChar) = ('[', ']'); break;
            case ProviderName.Unknown: break;

            default: throw new InvalidOperationException($"unsupported provider ({name}).");
        };
    }

    public string QuoteName(string content) => $"{LeftChar}{content}{RightChar}";

}
