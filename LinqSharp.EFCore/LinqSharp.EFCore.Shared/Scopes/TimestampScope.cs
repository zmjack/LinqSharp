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
    private readonly FieldOption _origin;

    public TimestampScope(ITimestampable context, FieldOption value)
    {
        _context = context;
        _origin = context.TimestampOption;

        context.TimestampOption = value;
    }

    public override void Disposing()
    {
        _context.TimestampOption = _origin;
    }
}
