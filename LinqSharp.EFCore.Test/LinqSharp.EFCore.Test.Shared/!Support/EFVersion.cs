﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;

namespace LinqSharp.EFCore;

public static class EFVersion
{
    public static readonly Version Version = typeof(DbContext).Assembly.GetName().Version;

    public static bool AtLeast(int major, int minor) => Version >= new Version(major, minor);
    public static bool AtLeast(int major, int minor, int build) => Version >= new Version(major, minor, build);
    public static bool AtLeast(int major, int minor, int build, int revision) => Version >= new Version(major, minor, build, revision);

    public static NotSupportedException NotSupportedException => new($"The version({Version}) of EntityFramework is not supported.");
    public static NotSupportedException NeedNewerVersionException => new($"Please use the newer version of LinqSharp.EFCore instead.");

}
