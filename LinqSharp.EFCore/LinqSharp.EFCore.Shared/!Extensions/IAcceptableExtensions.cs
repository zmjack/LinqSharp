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
public static class IAcceptableExtensions
{
    private static readonly Type AutoAttributeType = typeof(AutoAttribute);
    internal static void InnerAccept<T>(T entity, T model) where T : class, IAcceptable
    {
        var type = typeof(T);
        var props = type.GetProperties()
            .Where(x => x.CanRead && x.CanWrite)
            .Where(x =>
            {
                var attrs = x.GetCustomAttributes(false).OfType<Attribute>();
                return !attrs.Any(attr => (attr is KeyAttribute or NotAcceptableAttribute) || attr.GetType().BaseType == AutoAttributeType);
            })
            .Where(x => x.PropertyType.IsBasicType(true) || x.PropertyType.IsValueType)
            .ToArray();
        InnerAccept(entity, model, props);
    }

    internal static void InnerAccept<T>(T entity, T model, string[] properties) where T : class, IAcceptable
    {
        var type = typeof(T);
        var props = type.GetProperties().Where(x => properties.Contains(x.Name));
        InnerAccept(entity, model, props);
    }

    internal static void InnerAccept<T>(T entity, T model, IEnumerable<PropertyInfo> properties) where T : class, IAcceptable
    {
        foreach (var prop in properties)
        {
            prop.SetValue(entity, prop.GetValue(model));
        }
    }

    /// <summary>
    /// Accept all property values which are can be read and write from another model.
    ///     (Only ValueTypes, exclude 'KeyAttribute' and attributes which are extends <see cref="AutoAttribute"/>.)
    /// </summary>
    /// <typeparam name="TEntity">Instance of IEntity</typeparam>
    /// <param name="this">Source model</param>
    /// <param name="model">The model which provide values</param>
    /// <returns></returns>
    public static void Accept<TEntity>(this TEntity @this, TEntity model)
        where TEntity : class, IAcceptable
    {
        InnerAccept(@this, model);
    }

    /// <summary>
    /// Accept the specified property values from another model.
    /// </summary>
    /// <typeparam name="TEntity">Instance of IAcceptable</typeparam>
    /// <param name="this">Source model</param>
    /// <param name="model">The model which provide values</param>
    /// <param name="properties">Specifies properties that are applied to the source model.
    /// <para>A lambda expression representing the property(s) (x => x.Url).</para>
    /// <para>
    ///     If the value is made up of multiple properties then specify an anonymous
    ///     type including the properties. For example, (x => new { x.Title, x.BlogId }).
    /// </para>
    /// </param>
    /// <returns></returns>
    public static void Accept<TEntity>(this TEntity @this, TEntity model, Expression<Func<TEntity, object>> properties)
        where TEntity : class, IAcceptable
    {
        var props = PropertyExplorer.GetProperties(properties);
        InnerAccept(@this, model, props);
    }

    /// <summary>
    /// Accept the specified property values from another model.
    /// </summary>
    /// <typeparam name="TEntity">Instance of IEntity</typeparam>
    /// <param name="this">Source model</param>
    /// <param name="model">The model which provide values</param>
    /// <param name="properties">Specifies properties that are applied to the source model.
    /// <para>A lambda expression representing the property(s) (x => x.Url).</para>
    /// <para>
    ///     If the value is made up of multiple properties then specify an anonymous
    ///     type including the properties (x => new { x.Title, x.BlogId }).
    /// </para>
    /// </param>
    /// <returns></returns>
    public static void Accept<TEntity>(this TEntity @this, TEntity model, string[] properties)
        where TEntity : class, IAcceptable
    {
        InnerAccept(@this, model, properties);
    }
}
