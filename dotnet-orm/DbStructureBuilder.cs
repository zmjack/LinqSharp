using Microsoft.EntityFrameworkCore;
using NStandard;
using NStandard.Flows;
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
                sb.AppendLine(cols.Select(x => $"\"{x?.Replace("\"", "\"\"")}\"").Join(","));
            }

            foreach (var table in DbTables.Values)
            {
                appendLine(new[] { table.Name, table.DisplayName.For(name => name == table.Name ? "" : name) });
                appendLine(new[] { "Field", "Display", "Runtime Type", "Max Length", "Index", "Required", "Reference", "Note" });

                foreach (var field in table.TableFields)
                {
                    appendLine(new[]
                    {
                        field.Name,
                        field.DisplayName,
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

        public string GetHtml()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<body>");
            void appendLine(string[] cols)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(cols.Select(x => $"<td>{x}</td>").Join(""));
                sb.AppendLine("</tr>");
            }

            foreach (var table in DbTables.Values)
            {
                sb.AppendLine(@"<table style=""border-collapse:collapse; width:100%"" border=""1"">");
                sb.AppendLine(@"<col width=""10%"">".Repeat(7));

                appendLine(new[] { table.Name, table.DisplayName.For(name => name == table.Name ? "" : name) });
                appendLine(new[] { "Field", "Display", "Runtime Type", "Max Length", "Index", "Required", "Reference", "Note" });

                foreach (var field in table.TableFields)
                {
                    appendLine(new[]
                    {
                        field.Name,
                        field.DisplayName,
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
                                var note = values.Select(value => $"<li>{$"{value.LongValue}={value.Name}".Flow(StringFlow.HtmlEncode)}</li>").Join("");
                                return $"<ul>{note}</ul>";
                            }
                            else return "";
                        }),
                    });
                }
                sb.AppendLine("</table>");
                sb.AppendLine("<p></p>");
            }
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

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
                    DisplayName = DataAnnotationEx.GetDisplayName(type),
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
                    DisplayName = DataAnnotationEx.GetDisplayName(tableType),
                    TableFields = tableFields,
                };
                DbTables[tableType] = dbTable;
            }

            return DbTables[tableType];
        }

    }
}
