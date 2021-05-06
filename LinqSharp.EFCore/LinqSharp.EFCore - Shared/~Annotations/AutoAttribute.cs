// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LinqSharp.EFCore
{
    public abstract class AutoAttribute : Attribute
    {
        private EntityState[] _states;
        private readonly EntityState[] _validStates = new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };

        public AutoAttribute()
        {
            _states = _validStates;
        }

        public AutoAttribute(params EntityState[] states)
        {
            _states = states;
        }

        public EntityState[] States
        {
            get => _states;
            set
            {
                if (!value.All(x => _validStates.Contains(x))) throw new ArgumentException($"Only {nameof(EntityState.Added)}, {nameof(EntityState.Modified)}, {nameof(EntityState.Deleted)} are supported.");
                _states = value;
            }
        }

        public abstract object Format(object value);
    }
}
