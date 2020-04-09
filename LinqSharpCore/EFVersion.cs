using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp
{
    public static class EFVersion
    {
        public static readonly Version Version = typeof(DbContext).Assembly.GetName().Version;

        public static bool AtLeast(int major, int minor) => Version >= new Version(major, minor);
        public static bool AtLeast(int major, int minor, int build) => Version >= new Version(major, minor, build);
        public static bool AtLeast(int major, int minor, int build, int revision) => Version >= new Version(major, minor, build, revision);

        public static NotSupportedException NotSupportedException => new NotSupportedException($"The version({Version}) of EntityFramework is not supported.");

    }
}
