// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LinqSharp;

public static class DataAnnotation
{
    public static string GetDisplayName<TEntity, TRet>(Expression<Func<TEntity, TRet>> expression)
    {
        if (expression.Body is not MemberExpression exp) throw new NotSupportedException("This argument 'expression' must be MemberExpression.");
        return GetDisplayName(exp.Member);
    }

    public static string GetDisplayName(MemberInfo memberInfo, bool inherit = true)
    {
        var attr_DispalyName = memberInfo.GetCustomAttribute<DisplayNameAttribute>(inherit);
        if (attr_DispalyName is not null) return attr_DispalyName.DisplayName;

        var attr_Dispaly = memberInfo.GetCustomAttribute<DisplayAttribute>(inherit);
        if (attr_Dispaly is not null) return attr_Dispaly.Name;

        return memberInfo.Name;
    }

    public static string GetDisplayShortName<TEntity, TRet>(Expression<Func<TEntity, TRet>> expression)
    {
        if (expression.Body is not MemberExpression exp) throw new NotSupportedException("This argument 'expression' must be MemberExpression.");
        return exp.Member.GetCustomAttribute<DisplayAttribute>()?.ShortName ?? exp.Member.Name;
    }

    public static string GetDisplayString(object model, string propOrFieldName, string defaultReturn = "")
    {
        var parameter = Expression.Parameter(model.GetType());
        var property = Expression.PropertyOrField(parameter, propOrFieldName);
        var lambda = Expression.Lambda(property, parameter);
        return GetDisplay(model, lambda, defaultReturn);
    }

    public static string GetDisplay<TEntity, TRet>(object model, Expression<Func<TEntity, TRet>> expression, string defaultReturn = "")
    {
        return GetDisplay(model, expression as LambdaExpression, defaultReturn);
    }

    private static readonly Regex _formatCheckRegex = new(@"\{0:(.+?)\}");

    public static string GetDisplay(object model, LambdaExpression expression, string defaultReturn = "")
    {
        if (expression.Body is not MemberExpression exp) throw new NotSupportedException("This argument 'expression' must be MemberExpression.");

        object value;
        try
        {
            value = expression.Compile().DynamicInvoke(new object[] { model });
        }
        catch { value = null; }

        if (value is not null)
        {
            dynamic dValue = value;

            var displayFormatAttrType = exp.Member.GetCustomAttributes(false).FirstOrDefault(x => x is DisplayFormatAttribute) as DisplayFormatAttribute;

            if (displayFormatAttrType is not null)
            {
                var attrValue_DataFormatString = displayFormatAttrType.DataFormatString as string;

                var ret = attrValue_DataFormatString.Replace("{0}", dValue.ToString());
                int startat = 0;
                Match match;

                while ((match = _formatCheckRegex.Match(ret, startat)).Success)
                {
                    var group = match.Groups[1];
                    var stringValue = dValue.ToString(group.Value);
                    ret = ret.Replace($"{{0:{group.Value}}}", stringValue);
                    startat = group.Index - 3 + stringValue.Length;             // 3 = {0:
                }
                return ret;
            }
            else
            {
                if (value.GetType().IsEnum)
                    return GetDisplayName(value.GetType().GetFields().First(x => x.Name == value.ToString()));
                else return value.ToString();
            }
        }
        else return defaultReturn;
    }
}
