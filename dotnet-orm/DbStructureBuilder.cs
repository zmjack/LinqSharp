using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LinqSharp.Cli
{
    public class DbStructureBuilder
    {
        readonly Dictionary<Type, DbTable> DbTables = new Dictionary<Type, DbTable>();

        public string GetCsv()
        {
            var sb = new StringBuilder();
            void appendLine(string[] cols)
            {
                sb.AppendLine(cols.Join("\t"));
            }

            foreach (var table in DbTables.Values)
            {
                appendLine(new[] { table.Name });
                appendLine(new[] { "Field", "Runtime Type", "Max Length", "Index", "Required", "Reference", "Note" });

                foreach (var field in table.TableFields)
                {
                    appendLine(new[]
                    {
                        field.Name,
                        field.RuntimeType.GetSimplifiedName(),
                        field.MaxLength?.ToString(),
                        field.Index,
                        field.Required ? "Required" : "",
                        field.ReferenceType?.For(type => DbTables[type].Name),
                        field.RuntimeType.For(type=>
                        {
                            if (type.IsEnum)
                            {
                                var values = type.GetFields().Where(x => x.Name != "value__").Select(x => new
                                {
                                    Name = DataAnnotationEx.GetDisplayName(x),
                                    LongValue = Convert.ChangeType(Enum.Parse(type, x.Name), typeof(long)),
                                });
                                var note = values.Select(value => $"{value.LongValue}={value.Name}").Join(" ");
                                return note;
                            }
                            else return "";
                        }),
                    });
                }
                appendLine(new[] { "" });
            }

            return sb.ToString();
        }

        public void Cache(Type dbContextType)
        {
            var dbSetProps = dbContextType.GetProperties().Where(x => x.PropertyType.IsType(typeof(DbSet<>)));
            foreach (var prop in dbSetProps)
            {
                var defaultTableName = prop.Name;
                var tableType = prop.PropertyType.GetGenericArguments()[0];
                CacheTable(tableType, defaultTableName);
            }
        }

        private DbTable CacheTable(Type tableType, string defaultName)
        {
            if (!DbTables.ContainsKey(tableType))
            {
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                var tableName = tableAttr?.For(x => $"{x.Schema}.{x.Name}") ?? defaultName;

                var filedTypes = tableType.GetProperties().Where(x => x.CanRead && x.CanWrite).ToArray();
                var tableFields = filedTypes.Select(type => new DbTableField
                {
                    Name = type.Name,
                    RuntimeType = type.PropertyType,
                    Index = type.HasAttribute<KeyAttribute>() ? "Key"
                        : type.HasAttribute<CPKeyAttribute>() ? "CPKey"
                        : type.GetCustomAttribute<IndexAttribute>()?.For(x => $"{x.Group} ({x.Type})") ?? "",
                    MaxLength = type.GetCustomAttribute<StringLengthAttribute>()?.MaximumLength ?? null,
                    Required = type.HasAttribute<RequiredAttribute>(),
                    ReferenceType = type.GetCustomAttribute<ForeignKeyAttribute>()?.Name.For(name =>
                    {
                        return tableType.GetProperty(name).PropertyType;
                    }),
                }).ToArray();

                var dbTable = new DbTable
                {
                    Name = tableName,
                    TableFields = tableFields,
                };
                DbTables[tableType] = dbTable;
            }

            return DbTables[tableType];
        }

    }
}
