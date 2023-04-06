// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

#if !COMPATIBLE
using MySqlConnector;
#else
using MySql.Data.MySqlClient;
#endif
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace LinqSharp.EFCore.MySql
{
    /// <summary>
    /// (AllowLoadLocalInfile=True must be set in the connection string.)
    /// </summary>
    public class MySqlBulkCopyEngine : BulkCopyEngine
    {
        private static ArgumentException NotRequiredConnection(string paramName) => new("MySqlConnection is required.", paramName);

        public override string[] GetOrderedColumns(DbConnection connection, string tableName)
        {
            if (connection is not MySqlConnection _connection) throw NotRequiredConnection(nameof(connection));

            using var command = new MySqlCommand
            {
                Connection = _connection,
                CommandText = @"
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = @database AND TABLE_NAME = @table
ORDER BY ORDINAL_POSITION;",
            };
            command.Parameters.Add(new MySqlParameter("@database", connection.Database));
            command.Parameters.Add(new MySqlParameter("@table", tableName));

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
            if (connection is not MySqlConnection _connection) throw NotRequiredConnection(nameof(connection));

            var bulkCopy = new MySqlBulkCopy(_connection)
            {
                DestinationTableName = tableName,
            };

            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                foreach (var source in sources.ToArray())
                {
                    bulkCopy.WriteToServer(source);
                }
            }
            finally
            {
                _connection.Close();
            }
        }
    }

}
