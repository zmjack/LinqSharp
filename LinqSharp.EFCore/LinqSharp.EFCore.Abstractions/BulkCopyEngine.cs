// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace LinqSharp.EFCore
{
    public abstract class BulkCopyEngine
    {
        public abstract string[] GetOrderedColumns(DbConnection connection, string tableName);
        public abstract void WriteToServer(DbConnection connection, string tableName, IEnumerable<DataTable> sources);
    }
}
