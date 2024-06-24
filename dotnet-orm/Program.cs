// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using DotNetCli;
using NStandard;
using System;
using System.IO;
using System.Reflection;

namespace LinqSharp.Cli
{
    public class Program
    {
        public static readonly string CLI_VERSION = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static CmdContainer CmdContainer;
        public static Project ProjectInfo { get; private set; }

        static void Main(string[] args)
        {
            CmdContainer = new CmdContainer("orm", Assembly.GetExecutingAssembly(), Project.GetFromDirectory(Directory.GetCurrentDirectory()));
            if (CmdContainer.Project.HasValue)
            {
                ProjectInfo = CmdContainer.Project.Value;
            }
            else throw new InvalidOperationException("No project found.");

            PrintWelcome();

            CmdContainer.PrintProjectInfo();
            CmdContainer.Run(args);
        }

        public static void PrintWelcome()
        {
            Console.WriteLine($@"
{"ヽ(*^▽^)ノ".Center(60)}

LinqSharp .NET Command-line Tools {CLI_VERSION}");
        }

    }
}
