// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LinqSharp
{
    public static class DataAnnotationEx
    {
        public static string GetDisplayName(MemberInfo memberInfo, bool inherit = true)
        {
            var attr_DispalyName = memberInfo.GetAttributeViaName("System.ComponentModel.DisplayNameAttribute", inherit);
            if (attr_DispalyName != null)
                return attr_DispalyName.GetReflector().Property<string>("DisplayName").Value;

            var attr_Dispaly = memberInfo.GetAttributeViaName("System.ComponentModel.DataAnnotations.DisplayAttribute", inherit);
            if (attr_Dispaly != null) return attr_Dispaly.GetReflector().Property<string>("Name").Value;

            return memberInfo.Name;
        }

        public static string GetDisplayString(object model, string propOrFieldName, string defaultReturn = "")
        {
            var parameter = Expression.Parameter(model.GetType());
            var property = Expression.PropertyOrField(parameter, propOrFieldName);
            var lambda = Expression.Lambda(property, parameter);
            return GetDisplayString(model, lambda, defaultReturn);
        }

        public static string GetDisplayString(object model, LambdaExpression expression, string defaultReturn = "")
        {
            var exp = expression.Body as MemberExpression;
            if (exp is null)
                throw new NotSupportedException("This argument 'expression' must be MemberExpression.");

            object value;
            try
            {
                value = expression.Compile().DynamicInvoke(new object[] { model });
            }
            catch { value = null; }

            if (value != null)
            {
                dynamic dValue = value is Nullable ? ((dynamic)value).Value : value;

                var displayFormatAttrType = exp.Member.GetCustomAttributes(false)
                    .FirstOrDefault(x => x.GetType().FullName == "System.ComponentModel.DataAnnotations") as Attribute;

                if (displayFormatAttrType != null)
                {
                    var attrValue_DataFormatString = displayFormatAttrType.GetReflector().Property<string>("DataFormatString ").Value;

                    var ret = attrValue_DataFormatString.Replace("{0}", dValue.ToString());
                    int startat = 0;
                    var regex = new Regex(@"\{0:(.+?)\}");
                    Match match;

                    while ((match = regex.Match(ret, startat)).Success)
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
}
