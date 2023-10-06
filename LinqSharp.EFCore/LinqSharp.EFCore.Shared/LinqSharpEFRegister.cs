// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace LinqSharp.EFCore;

public static class LinqSharpEFRegister
{
    private static readonly Dictionary<ProviderName, BulkCopyEngine> BulkCopyEngineDict = new();
    public static void RegisterBulkCopyEngine(ProviderName name, BulkCopyEngine engine) => BulkCopyEngineDict[name] = engine;
    public static void UnregisterBulkCopyEngine(ProviderName name) => BulkCopyEngineDict.Remove(name);
    public static bool TryGetBulkCopyEngine(ProviderName name, out BulkCopyEngine engine)
    {
        if (BulkCopyEngineDict.ContainsKey(name))
        {
            engine = BulkCopyEngineDict[name];
            return true;
        }
        else
        {
            engine = null;
            return false;
        }
    }
}
