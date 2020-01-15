using NStandard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace LinqSharp
{
    /// <summary>
    /// Easy to use and secure SQL Executor
    /// </summary>
    /// <typeparam name="TDbConnection"></typeparam>
    /// <typeparam name="TDbCommand"></typeparam>
    /// <typeparam name="TDbParameter"></typeparam>
    public abstract class SqlScope<TDbConnection, TDbCommand, TDbParameter>
        : Scope<TDbConnection, SqlScope<TDbConnection, TDbCommand, TDbParameter>>
        where TDbConnection : DbConnection
        where TDbCommand : DbCommand, new()
        where TDbParameter : DbParameter, new()
    {
        private TDbConnection Connection;

        public SqlScope(TDbConnection model) : base(model)
        {
            Connection = model;
            Connection.Open();
        }

        public override void Disposing()
        {
            Connection.Close();
            base.Disposing();
        }

        public int Sql(FormattableString formattableSql)
        {
            var command = SqlCommand(formattableSql);
            return command.ExecuteNonQuery();
        }

        public int Sql(string sql, TDbParameter[] parameters)
        {
            var command = SqlCommand(sql, parameters);
            return command.ExecuteNonQuery();
        }

        public IEnumerable<Dictionary<string, object>> SqlQuery(string sql, TDbParameter[] parameters)
        {
            var command = SqlCommand(sql, parameters);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var dict = new Dictionary<string, object>();
                foreach (var i in new int[reader.FieldCount].Let(i => i))
                    dict[reader.GetName(i)] = reader.GetValue(i);

                yield return dict;
            }
            reader.Close();
        }

        public IEnumerable<Dictionary<string, object>> SqlQuery(FormattableString formattableSql)
        {
            var command = SqlCommand(formattableSql);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var dict = new Dictionary<string, object>();
                foreach (var i in new int[reader.FieldCount].Let(i => i))
                    dict[reader.GetName(i)] = reader.GetValue(i);

                yield return dict;
            }
            reader.Close();
        }

        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        public TDbCommand SqlCommand(string sql, TDbParameter[] parameters)
        {
            var cmd = new TDbCommand
            {
                CommandText = sql,
                Connection = Connection,
            };

            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                    cmd.Parameters.Add(parameters[i]);
            }

            return cmd;
        }

        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        protected TDbCommand SqlCommand(FormattableString formattableSql)
        {
            var args = formattableSql.GetArguments();
            var sql = formattableSql.Format;
            var cmd = new TDbCommand
            {
                CommandText = sql.NFor((_sql, i) => _sql.Replace($"{{{i}}}", $"@p{i}"), formattableSql.ArgumentCount),
                Connection = Connection,
            };

            for (int i = 0; i < formattableSql.ArgumentCount; i++)
            {
                cmd.Parameters.Add(new TDbParameter().Then(x =>
                {
                    x.ParameterName = $"@p{i}";
                    x.Value = args[i];
                    x.DbType = args[i].For(value =>
                    {
                        return (value.GetType()) switch
                        {
                            Type type when type == typeof(bool) => DbType.Boolean,
                            Type type when type == typeof(byte) => DbType.Byte,
                            Type type when type == typeof(sbyte) => DbType.SByte,
                            Type type when type == typeof(char) => DbType.Byte,
                            Type type when type == typeof(short) => DbType.Int16,
                            Type type when type == typeof(ushort) => DbType.UInt16,
                            Type type when type == typeof(int) => DbType.Int32,
                            Type type when type == typeof(uint) => DbType.UInt32,
                            Type type when type == typeof(long) => DbType.Int64,
                            Type type when type == typeof(ulong) => DbType.UInt64,
                            Type type when type == typeof(float) => DbType.Single,
                            Type type when type == typeof(double) => DbType.Double,
                            Type type when type == typeof(string) => DbType.String,
                            Type type when type == typeof(decimal) => DbType.Decimal,
                            Type type when type == typeof(DateTime) => DbType.DateTime,
                            _ => DbType.Object,
                        };
                    });
                }));
            }
            return cmd;
        }

    }
}
