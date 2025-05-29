// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Annotations;
using LinqSharp.EFCore.Design;
using LinqSharp.Utils;
using NStandard;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class IEntityExtensions
{
    private static readonly Type AutoAttributeType = typeof(AutoAttribute);
    internal static TEntity InnerAccept<TEntity>(TEntity entity, TEntity model)
    {
        var type = typeof(TEntity);
        var props = type.GetProperties()
            .Where(x => x.CanRead && x.CanWrite)
            .Where(x =>
            {
                var attrs = x.GetCustomAttributes(false).OfType<Attribute>();
                return !attrs.Any(attr => attr is KeyAttribute || attr is NotAcceptableAttribute || attr.GetType().BaseType == AutoAttributeType);
            })
            .Where(x => x.PropertyType.IsBasicType(true) || x.PropertyType.IsValueType)
            .ToArray();

        return InnerAccept(entity, model, props);
    }

    internal static TEntity InnerAccept<TEntity>(TEntity entity, TEntity model, string[] properties)
    {
        var type = typeof(TEntity);
        var props = type.GetProperties().Where(x => properties.Contains(x.Name));
        return InnerAccept(entity, model, props);
    }

    internal static TEntity InnerAccept<TEntity>(TEntity entity, TEntity model, IEnumerable<PropertyInfo> properties)
    {
        foreach (var prop in properties)
        {
            prop.SetValue(entity, prop.GetValue(model));
        }
        return entity;
    }

    /// <summary>
    /// Accept all property values which are can be read and write from another model.
    ///     (Only ValueTypes, exclude 'KeyAttribute' and attributes which are extends <see cref="AutoAttribute"/>.)
    /// </summary>
    /// <typeparam name="TEntity">Instance of IEntity</typeparam>
    /// <param name="this">Source model</param>
    /// <param name="model">The model which provide values</param>
    /// <returns></returns>
    public static TEntity Accept<TEntity>(this TEntity @this, TEntity model) where TEntity : class, IEntity => InnerAccept(@this, model);

    /// <summary>
    /// Accept the specified property values from another model.
    /// </summary>
    /// <typeparam name="TEntity">Instance of IEntity</typeparam>
    /// <param name="this">Source model</param>
    /// <param name="model">The model which provide values</param>
    /// <param name="includes">Specifies properties that are applied to the source model.
    /// <para>A lambda expression representing the property(s) (x => x.Url).</para>
    /// <para>
    ///     If the value is made up of multiple properties then specify an anonymous
    ///     type including the properties. For example, (x => new { x.Title, x.BlogId }).
    /// </para>
    /// </param>
    /// <returns></returns>
    public static TEntity Accept<TEntity>(this TEntity @this, TEntity model, Expression<Func<TEntity, object>> includes)
        where TEntity : class, IEntity
    {
        var props = PropertyExplorer.GetProperties(includes);
        return InnerAccept(@this, model, props);
    }

    /// <summary>
    /// Accept the specified property values from another model.
    /// </summary>
    /// <typeparam name="TEntity">Instance of IEntity</typeparam>
    /// <param name="this">Source model</param>
    /// <param name="model">The model which provide values</param>
    /// <param name="includes_MemberOrNewExp">Specifies properties that are applied to the source model.
    /// <para>A lambda expression representing the property(s) (x => x.Url).</para>
    /// <para>
    ///     If the value is made up of multiple properties then specify an anonymous
    ///     type including the properties (x => new { x.Title, x.BlogId }).
    /// </para>
    /// </param>
    /// <returns></returns>
    public static TEntity Accept<TEntity>(this TEntity @this, TEntity model, string[] properties)
        where TEntity : class, IEntity
    {
        return InnerAccept(@this, model, properties);
    }

    public static void SetValue(this IEntity @this, string propName, object value) => @this.GetType().GetProperty(propName)!.SetValue(@this, value);
    public static object? GetValue(this IEntity @this, string propName) => @this.GetType().GetProperty(propName)!.GetValue(@this);

    public static Dictionary<string, string?> ToDisplayDictionary(this IEntity @this)
    {
        var type = @this.GetType();
        var props = type.GetProperties();

        var dict = new Dictionary<string, string?>();
        foreach (var prop in props)
        {
            var parameter = Expression.Parameter(type);
            var property = Expression.Property(parameter, prop.Name);
            var lambda = Expression.Lambda(property, parameter);

            dict.Add(prop.Name, DataAnnotation.GetDisplay(@this, lambda));
        }

        return dict;
    }

    public static Dictionary<string, string?> ToDisplayDictionary(this IEntity @this, params string[] propNames)
    {
        var type = @this.GetType();
        var props = type.GetProperties().Where(x => propNames.Contains(x.Name));

        var dict = new Dictionary<string, string?>();
        foreach (var prop in props)
        {
            dict.Add(prop.Name, DataAnnotation.GetDisplayString(@this, prop.Name));
        }

        return dict;
    }

#pragma warning disable IDE0060 // Remove unused parameter
    public static string? DisplayName<TEntity, TRet>(this IEnumerable<IEntity<TEntity>> @this, Expression<Func<TEntity, TRet>> expression)
        where TEntity : class, IEntity<TEntity>, new()
    {
        return DataAnnotation.GetDisplayName(expression);
    }

    public static string? DisplayName<TEntity, TRet>(this IEntity<TEntity> @this, Expression<Func<TEntity, TRet>> expression)
        where TEntity : class, IEntity<TEntity>, new()
    {
        return DataAnnotation.GetDisplayName(expression);
    }

    public static string? DisplayShortName<TEntity, TRet>(this IEnumerable<IEntity<TEntity>> @this, Expression<Func<TEntity, TRet>> expression)
        where TEntity : class, IEntity<TEntity>, new()
    {
        return DataAnnotation.GetDisplayShortName(expression);
    }

    public static string? DisplayShortName<TEntity, TRet>(this IEntity<TEntity> @this, Expression<Func<TEntity, TRet>> expression)
        where TEntity : class, IEntity<TEntity>, new()
    {
        return DataAnnotation.GetDisplayShortName(expression);
    }
#pragma warning restore IDE0060 // Remove unused parameter

    public static string? Display<TEntity, TRet>(this IEntity<TEntity> @this, Expression<Func<TEntity, TRet>> expression, string defaultReturn = "")
        where TEntity : class, IEntity<TEntity>, new()
    {
        return DataAnnotation.GetDisplay(@this, expression, defaultReturn);
    }
}
