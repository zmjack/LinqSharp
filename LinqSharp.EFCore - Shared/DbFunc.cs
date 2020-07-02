// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;

#pragma warning disable IDE0060 // Remove unused parameter

namespace LinqSharp.EFCore
{
    public static class DbFunc
    {
        private static readonly Random SRandom = new Random();

        public static double Random() => SRandom.NextDouble();
        public static string Concat(string str0, string str1) => $"{str0}{str1}";
        public static string Concat(string str0, string str1, string str2) => $"{str0}{str1}{str2}";
        public static string Concat(string str0, string str1, string str2, string str3) => $"{str0}{str1}{str2}{str3}";
        public static string Concat(string str0, string str1, string str2, string str3, string str4) => $"{str0}{str1}{str2}{str3}{str4}";
        public static string Concat(string str0, string str1, string str2, string str3, string str4, string str5) => $"{str0}{str1}{str2}{str3}{str4}{str5}";
        public static string Concat(string str0, string str1, string str2, string str3, string str4, string str5, string str6) => $"{str0}{str1}{str2}{str3}{str4}{str5}{str6}";
        public static string Concat(string str0, string str1, string str2, string str3, string str4, string str5, string str6, string str7) => $"{str0}{str1}{str2}{str3}{str4}{str5}{str6}{str7}";
        public static DateTime DateTime(int year, int month, int day) => new DateTime(year, month, day);
        public static DateTime DateTime(int year, int month, int day, int hour, int minute, int second) => new DateTime(year, month, day, hour, minute, second);

#if DEBUG
        public static string Test(string s0) => throw new NotSupportedException();
#endif
    }

}

#pragma warning restore IDE0060 // Remove unused parameter
