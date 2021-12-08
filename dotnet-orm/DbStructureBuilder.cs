// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using NStandard.Flows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqSharp.Cli
{
    public class DbStructureBuilder
    {
        readonly Dictionary<Type, DbTable> DbTables = new();

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
            sb.AppendLine(@"<!DOCTYPE html>
<html>
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <style type=""text/css"">
        table {
            font-family: verdana,arial,sans-serif;
            font-size:11px;
            color:#333333;
            border-width: 1px;
            border-color: #666666;
            border-collapse: collapse;
        }
        table th {
            border-width: 1px;
            padding: 8px;
            border-style: solid;
            border-color: #666666;
            background-color: #dedede;
        }
        table td {
            border-width: 1px;
            padding: 8px;
            border-style: solid;
            border-color: #666666;
            background-color: #ffffff;
        }
    </style>
</head>");
            sb.AppendLine("<body>");
            void appendLine(string[] cols, string tag = "td")
            {
                sb.AppendLine("<tr>");
                sb.AppendLine(cols.Select(x => $"<{tag}>{x}</{tag}>").Join(""));
                sb.AppendLine("</tr>");
            }

            foreach (var table in DbTables.Values)
            {
                sb.AppendLine(@"<table style=""border-collapse:collapse; width:100%"" border=""1"">");
                sb.AppendLine(@"<col width=""10%"">".Repeat(7));

                appendLine(new[] { table.Name, table.DisplayName.For(name => name == table.Name ? "" : name) });
                appendLine(new[] { "Field", "Display", "Runtime Type", "Max Length", "Index", "Required", "Reference", "Note" }, "th");

                foreach (var field in table.TableFields)
                {
                    var noteBuilder = new StringBuilder();
                    if (field.ObsoleteLevel is not null)
                    {
                        noteBuilder.AppendLine($"过时级别：{field.ObsoleteLevel}");
                    }
                    if (field.RuntimeType.IsEnum)
                    {
                        var type = field.RuntimeType;
                        var values = type.GetFields().Where(x => x.Name != "value__").Select(x => new
                        {
                            Name = DataAnnotationEx.GetDisplayName(x),
                            LongValue = Convert.ChangeType(Enum.Parse(type, x.Name), typeof(long)),
                        });
                        var note = values.Select(value => $"<li>{$"{value.LongValue}={value.Name}".For(StringFlow.HtmlEncode)}</li>").Join("");
                        noteBuilder.AppendLine($"<ul>{note}</ul>");
                    }

                    appendLine(new[]
                    {
                        field.Name,
                        field.DisplayName.For(StringFlow.HtmlEncode),
                        field.RuntimeType.GetSimplifiedName().For(StringFlow.HtmlEncode),
                        field.MaxLength?.ToString(),
                        field.Index.For(StringFlow.HtmlEncode),
                        field.Required ? "Required" : "",
                        field.ReferenceType?.For(type => DbTables[type].Name),
                        noteBuilder.ToString(),
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
            var dbSetProps = dbContextType.GetProperties().Where(x => x.PropertyType.Name == "DbSet`1");
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
                var tableAttr = tableType.GetAttributeViaName("System.ComponentModel.DataAnnotations.Schema.TableAttribute");
                var tableName = tableAttr?.GetReflector().For(x => $"{x.Property<string>("Schema").Value}.{x.Property<string>("Name").Value}") ?? defaultName;

                var filedTypes = tableType.GetProperties().Where(x => x.CanRead && x.CanWrite).ToArray();
                var tableFields = filedTypes.Select(type => new DbTableField
                {
                    Name = type.Name,
                    DisplayName = DataAnnotationEx.GetDisplayName(type),
                    RuntimeType = type.PropertyType,
                    Index = type.HasAttributeViaName("System.ComponentModel.DataAnnotations.KeyAttribute") ? "Key"
                        : type.HasAttributeViaName($"LinqSharp.EFCore.CPKeyAttribute") ? "CPKey"
                        : type.GetAttributeViaName($"LinqSharp.EFCore.IndexAttribute")?.GetReflector().For(x => $"{x.Property<Enum>("Type").Value} {x.Property<string>("Group").Value?.For(g => $"({g})")}") ?? "",
                    MaxLength = type.GetAttributeViaName("System.ComponentModel.DataAnnotations.StringLengthAttribute")?.GetReflector().For(x => x.Property<int>("MaximumLength").Value) ?? null,
                    Required = type.HasAttributeViaName("System.ComponentModel.DataAnnotations.RequiredAttribute"),
                    ReferenceType = type.GetAttributeViaName("System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute")?.GetReflector().For(x =>
                    {
                        var name = x.Property<string>("Name").Value;
                        return tableType.GetProperty(name).PropertyType;
                    }),
                    ObsoleteLevel = type.GetAttributeViaName("System.ObsoleteAttribute")?.GetReflector().For(x =>
                    {
                        var isError = x.Property<bool>("IsError").Value;
                        return isError ? "已弃用" : "警告";
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
