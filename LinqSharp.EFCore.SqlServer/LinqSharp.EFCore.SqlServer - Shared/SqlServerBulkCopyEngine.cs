// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

#if EFCore2
using System.Data.SqlClient;
#else
using Microsoft.Data.SqlClient;
#endif
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;

namespace LinqSharp.EFCore.SqlServer
{
    public class SqlServerBulkCopyEngine : BulkCopyEngine
    {
        protected override string[] GetDatabaseColumnNames<TEntity>(DbContext dbContext)
        {
            var connection = dbContext.Database.GetDbConnection() as SqlConnection;
            using var command = new SqlCommand
            {
                Connection = connection,
                CommandText = @"
SELECT syscolumns.[name], syscolumns.[colid]
FROM sysobjects LEFT JOIN syscolumns on sysobjects.[id] = syscolumns.[id]
WHERE sysobjects.[xtype] = 'U' AND sysobjects.[name] = @table
ORDER BY syscolumns.colorder;",
            };
            command.Parameters.Add(new SqlParameter("@table", dbContext.GetTableName<TEntity>()));

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
            var connection = dbContext.Database.GetDbConnection() as SqlConnection;
            using var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, null)
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
