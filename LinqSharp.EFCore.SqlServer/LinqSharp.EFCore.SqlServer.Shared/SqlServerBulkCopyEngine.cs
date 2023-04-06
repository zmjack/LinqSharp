// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

#if !COMPATIBLE
using Microsoft.Data.SqlClient;
#else
using System.Data.SqlClient;
#endif
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace LinqSharp.EFCore.SqlServer
{
    public class SqlServerBulkCopyEngine : BulkCopyEngine
    {
        private static ArgumentException NotRequiredConnection(string paramName) => new("SqlConnection is required.", paramName);

        public override string[] GetOrderedColumns(DbConnection connection, string tableName)
        {
            if (connection is not SqlConnection _connection) throw NotRequiredConnection(nameof(connection));

            using var command = new SqlCommand
            {
                Connection = _connection,
                CommandText = @"
SELECT syscolumns.[name], syscolumns.[colid]
FROM sysobjects LEFT JOIN syscolumns on sysobjects.[id] = syscolumns.[id]
WHERE sysobjects.[xtype] = 'U' AND sysobjects.[name] = @table
ORDER BY syscolumns.colorder;",
            };
            command.Parameters.Add(new SqlParameter("@table", tableName));

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                var columnNameList = new List<string>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columnNameList.Add(reader.GetValue(0) as string);
                    }
                    reader.Close();
                }

                return columnNameList.ToArray();
            }
            finally
            {
                connection.Close();
            }
        }

        public override void WriteToServer(DbConnection connection, string tableName, IEnumerable<DataTable> sources)
        {
            if (connection is not SqlConnection _connection) throw NotRequiredConnection(nameof(connection));

            using var bulkCopy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.Default, null)
            {
                DestinationTableName = tableName,
            };

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                foreach (var source in sources)
                {
                    bulkCopy.WriteToServer(source);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }

}
