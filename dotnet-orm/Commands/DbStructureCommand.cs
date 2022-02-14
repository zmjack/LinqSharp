// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using DotNetCli;
using NStandard;
using NStandard.Runtime;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace LinqSharp.Cli
{
    [Command("DbStructure", Abbreviation = "ds", Description = "Generate database structure csv.")]
    public class DbStructureCommand : Command
    {
        private static readonly string TargetBinFolder = Path.GetFullPath($"{Program.ProjectInfo.ProjectRoot}/bin/Debug/{Program.ProjectInfo.TargetFramework}");

        public DbStructureCommand(CmdContainer container, string[] args) : base(container, args) { }

        [CmdProperty("out", Abbreviation = "o", Description = "Specify the output directory path. (default: Typings)")]
        public string OutFolder { get; set; } = ".";

        [CmdProperty("bom", Abbreviation = "b", Description = "Set BOM of utf-8 for output files.")]
        public bool SetBOM { get; set; }

        public override void Run()
        {
            if (!Directory.Exists(OutFolder)) Directory.CreateDirectory(OutFolder);

            var targetAssemblyName = Program.ProjectInfo.AssemblyName;
            var assemblyContext = new AssemblyContext(DotNetFramework.Parse(Program.ProjectInfo.TargetFramework), Program.ProjectInfo.Sdk);
            assemblyContext.LoadMain($"{TargetBinFolder}/{targetAssemblyName}.dll");

            var dbContextType = assemblyContext.GetType($"Microsoft.EntityFrameworkCore.DbContext,Microsoft.EntityFrameworkCore");
            var types = assemblyContext.MainAssembly.GetTypesWhichExtends(dbContextType, true);
            foreach (var type in types)
            {
                var outFile = $"{Path.GetFullPath($"{OutFolder}/{type.Name}.html")}";

                var builder = new DbStructureBuilder();
                builder.Cache(type);
                var csvContent = builder.GetHtml();
                var bytes = SetBOM
                    ? new byte[] { 0xef, 0xbb, 0xbf }.Concat(csvContent.Bytes(Encoding.UTF8)).ToArray()
                    : csvContent.Bytes(Encoding.UTF8).ToArray();

                File.WriteAllBytes(outFile, bytes);
                Console.WriteLine($"File Saved: {outFile}");
            }
        }

    }
}
