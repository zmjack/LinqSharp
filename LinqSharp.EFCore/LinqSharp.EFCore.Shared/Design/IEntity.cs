// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp.EFCore.Design;

/// <summary>
/// Use <see cref="IEntity"/> to define entity classes to get some useful extension methods.
/// </summary>
public interface IEntity { }

/// <summary>
/// Use <see cref="IEntity"/> to define entity classes to get some useful extension methods.
/// </summary>
public interface IEntity<TSelf> : IEntity where TSelf : class, IEntity<TSelf>, new()
{
}
