// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSharp.EFCore.Functions
{
    public interface IDbFunc
    {
        double Random();

        string Concat(string str0, string str1);
        string Concat(string str0, string str1, string str2);
        string Concat(string str0, string str1, string str2, string str3);
        string Concat(string str0, string str1, string str2, string str3, string str4);
        string Concat(string str0, string str1, string str2, string str3, string str4, string str5);
        string Concat(string str0, string str1, string str2, string str3, string str4, string str5, string str6);
        string Concat(string str0, string str1, string str2, string str3, string str4, string str5, string str6, string str7);

        DateTime DateTime(int year, int month, int day);
        DateTime DateTime(int year, int month, int day, int hour, int minute, int second);

        double ToDouble(int number);
    }
}
