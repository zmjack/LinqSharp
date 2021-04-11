// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace LinqSharp.EFCore
{
    public static class LinqSharpEFRegister
    {
        private static readonly Dictionary<DatabaseProviderName, BulkCopyEngine> BulkCopyEngineDict = new();
        public static void RegisterBulkCopyEngine(DatabaseProviderName name, BulkCopyEngine engine) => BulkCopyEngineDict[name] = engine;
        public static void UnregisterBulkCopyEngine(DatabaseProviderName name) => BulkCopyEngineDict.Remove(name);
        public static bool TryGetBulkCopyEngine(DatabaseProviderName name, out BulkCopyEngine engine)
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

        public static bool AllowUnsafeCode = false;

    }
}
