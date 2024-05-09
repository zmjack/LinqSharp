// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using NStandard;

namespace LinqSharp.EFCore.Scopes;

public sealed class TimestampScope<TContext>(AutoMode mode) : Scope<TimestampScope<TContext>>, IAutoModeScope
    where TContext : DbContext
{
    public AutoMode Mode { get; } = mode;
}
