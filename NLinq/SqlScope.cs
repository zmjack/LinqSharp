using Dawnx;
using Dawnx.Ranges;
using Dawnx.Utilities;
using NStandard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace NLinq
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

        public IEnumerable<Dictionary<string, object>> SqlQuery(FormattableString formattableSql)
        {
            var command = SqlCommand(formattableSql);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var dict = new Dictionary<string, object>();
                foreach (var i in IntegerRange.Create(reader.FieldCount))
                    dict[reader.GetName(i)] = reader.GetValue(i);

                yield return dict;
            }
            reader.Close();
        }

        protected TDbCommand SqlCommand(FormattableString formattableSql)
        {
            var values = formattableSql.GetArguments();
            var sql = formattableSql.Format;

            var range = IntegerRange.Create(formattableSql.ArgumentCount);
            foreach (var i in range)
                sql = sql.Replace($"{{{i}}}", $"@p{i}");

            var command = new TDbCommand().Then(_ =>
            {
                _.CommandText = sql;
                _.Connection = Connection;
            });

            foreach (var i in range)
            {
                command.Parameters.Add(new TDbParameter().Then(x =>
                {
                    x.ParameterName = $"@p{i}";
                    x.Value = values[i];
                    x.DbType = XObject.For(values[i], value =>
                    {
                        switch (value.GetType().FullName)
                        {
                            case BasicTypeUtility.@bool: return DbType.Boolean;
                            case BasicTypeUtility.@byte: return DbType.Byte;
                            case BasicTypeUtility.@sbyte: return DbType.SByte;
                            case BasicTypeUtility.@char: return DbType.Byte;
                            case BasicTypeUtility.@short: return DbType.Int16;
                            case BasicTypeUtility.@ushort: return DbType.UInt16;
                            case BasicTypeUtility.@int: return DbType.Int32;
                            case BasicTypeUtility.@uint: return DbType.UInt32;
                            case BasicTypeUtility.@long: return DbType.Int64;
                            case BasicTypeUtility.@ulong: return DbType.UInt64;
                            case BasicTypeUtility.@float: return DbType.Single;
                            case BasicTypeUtility.@double: return DbType.Double;
                            case BasicTypeUtility.@string: return DbType.String;
                            case BasicTypeUtility.@decimal: return DbType.Decimal;
                            case BasicTypeUtility.DateTime: return DbType.DateTime;
                            default: return DbType.Object;
                        }
                    });
                }));
            }
            return command;
        }

    }
}
