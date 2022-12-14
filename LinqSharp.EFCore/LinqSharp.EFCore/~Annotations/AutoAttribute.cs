// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Globalization;
using System.Linq;

namespace LinqSharp.EFCore
{
    public abstract class AutoAttribute : Attribute
    {
        private readonly EntityState[] SupportedStates = new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };
        private readonly EntityState[] _states;

        private ArgumentException Exception_NotSupportedStates(string paramName)
        {
            throw new ArgumentException($"Only {SupportedStates.Join(", ")} are supported.", paramName);
        }

        protected ArgumentException Exception_NotSupportedTypes(Type propertyType, string paramName)
        {
            throw new ArgumentException($"The {propertyType} is not supported for {GetType()}.", paramName);
        }

        public AutoAttribute()
        {
            _states = SupportedStates;
        }

        public AutoAttribute(params EntityState[] states)
        {
            if (!states.All(x => SupportedStates.Contains(x))) throw Exception_NotSupportedStates(nameof(states));

            _states = states;
        }

        public EntityState[] States => _states;

        public abstract object Format(object entity, Type propertyType, object value);
    }
}
