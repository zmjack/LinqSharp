// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Design;
using NStandard;

namespace LinqSharp.EFCore.Scopes;

public class TimestampScope : Scope<TimestampScope>
{
    private readonly ITimestampable _context;
    private readonly bool _origin;

    public TimestampScope(ITimestampable context, bool origin)
    {
        _context = context;
        _origin = origin;

        context.IgnoreTimestamp = true;
    }

    public override void Disposing()
    {
        _context.IgnoreTimestamp = _origin;
    }
}
