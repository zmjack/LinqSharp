// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
#if EFCORE5_0_OR_GREATER
using MySqlConnector;
#else
using MySql.Data.MySqlClient;
#endif
using System.Collections.Generic;
using System.Data;

namespace LinqSharp.EFCore.MySql
{
    /// <summary>
    /// (AllowLoadLocalInfile=True must be set in the connection string.)
    /// </summary>
    public class MySqlBulkCopyEngine : BulkCopyEngine
    {
        protected override string[] GetDatabaseColumnNames<TEntity>(DbContext dbContext)
        {
            var connection = dbContext.Database.GetDbConnection() as MySqlConnection;
            using var command = new MySqlCommand
            {
                Connection = connection,
                CommandText = @"
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = @database AND TABLE_NAME = @table
ORDER BY ORDINAL_POSITION;",
            };
            command.Parameters.Add(new MySqlParameter("@database", connection.Database));
            command.Parameters.Add(new MySqlParameter("@table", dbContext.GetTableName<TEntity>()));

            var needOpen = connection.State != ConnectionState.Open;
            try
            {
                if (needOpen) connection.Open();
                var columnNameList = new List<string>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read()) columnNameList.Add(reader.GetValue(0) as string);
                    reader.Close();
                }
                return columnNameList.ToArray();
            }
            finally
            {
                if (needOpen) connection.Close();
            }
        }

        public override void WriteToServer<TEntity>(DbContext dbContext, IEnumerable<TEntity> entities, int bulkSize)
        {
            var connection = dbContext.Database.GetDbConnection() as MySqlConnection;
            var bulkCopy = new MySqlBulkCopy(connection)
            {
                DestinationTableName = dbContext.GetTableName<TEntity>(),
            };

            var needOpen = connection.State != ConnectionState.Open;
            try
            {
                if (needOpen) connection.Open();
                foreach (var group in entities.GroupByCount(bulkSize))
                {
                    var table = BuildDataTable(dbContext, group);
                    bulkCopy.WriteToServer(table);
                }
            }
            finally
            {
                if (needOpen) connection.Close();
            }
        }
    }

}
