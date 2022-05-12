// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;

namespace LinqSharp.EFCore
{
    public class DbFunc
    {
        private static readonly Random RandomInstance = new();

        public static double Random() => RandomInstance.NextDouble();
        public static string Concat(string str0, string str1) => $"{str0}{str1}";
        public static string Concat(string str0, string str1, string str2) => $"{str0}{str1}{str2}";
        public static string Concat(string str0, string str1, string str2, string str3) => $"{str0}{str1}{str2}{str3}";
        public static string Concat(string str0, string str1, string str2, string str3, string str4) => $"{str0}{str1}{str2}{str3}{str4}";
        public static string Concat(string str0, string str1, string str2, string str3, string str4, string str5) => $"{str0}{str1}{str2}{str3}{str4}{str5}";
        public static string Concat(string str0, string str1, string str2, string str3, string str4, string str5, string str6) => $"{str0}{str1}{str2}{str3}{str4}{str5}{str6}";
        public static string Concat(string str0, string str1, string str2, string str3, string str4, string str5, string str6, string str7) => $"{str0}{str1}{str2}{str3}{str4}{str5}{str6}{str7}";
        public static DateTime DateTime(int year, int month, int day) => new(year, month, day);
        public static DateTime DateTime(int year, int month, int day, int hour, int minute, int second) => new(year, month, day, hour, minute, second);
        public static double ToDouble(int number) => number;
    }

}
