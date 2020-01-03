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
                foreach (var i in new int[reader.FieldCount].Let(i => i))
                    dict[reader.GetName(i)] = reader.GetValue(i);

                yield return dict;
            }
            reader.Close();
        }

        protected TDbCommand SqlCommand(FormattableString formattableSql)
        {
            var values = formattableSql.GetArguments();
            var sql = formattableSql.Format;

            var range = new int[formattableSql.ArgumentCount].Let(i => i);
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
                        switch (value.GetType())
                        {
                            case Type type when type == typeof(bool): return DbType.Boolean;
                            case Type type when type == typeof(byte): return DbType.Byte;
                            case Type type when type == typeof(sbyte): return DbType.SByte;
                            case Type type when type == typeof(char): return DbType.Byte;
                            case Type type when type == typeof(short): return DbType.Int16;
                            case Type type when type == typeof(ushort): return DbType.UInt16;
                            case Type type when type == typeof(int): return DbType.Int32;
                            case Type type when type == typeof(uint): return DbType.UInt32;
                            case Type type when type == typeof(long): return DbType.Int64;
                            case Type type when type == typeof(ulong): return DbType.UInt64;
                            case Type type when type == typeof(float): return DbType.Single;
                            case Type type when type == typeof(double): return DbType.Double;
                            case Type type when type == typeof(string): return DbType.String;
                            case Type type when type == typeof(decimal): return DbType.Decimal;
                            case Type type when type == typeof(DateTime): return DbType.DateTime;
                            default: return DbType.Object;
                        }
                    });
                }));
            }
            return command;
        }

    }
}
