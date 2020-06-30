using DotNetCli;
using Ink;
using Microsoft.EntityFrameworkCore;
using NStandard;
using NStandard.Reference;
using NStandard.Runtime;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace LinqSharp.Cli
{
    [Command("DbStructure", "ds", Description = "Generate database structure csv.")]
    public class DbStructureCommand : ICommand
    {
        private static readonly string TargetBinFolder = Path.GetFullPath($"{Program.ProjectInfo.ProjectRoot}/bin/Debug/{Program.ProjectInfo.TargetFramework}");

        public void PrintUsage()
        {
            Console.WriteLine($@"
Usage: dotnet orm (ds|DbStructure) [Options]

Options:
  {"-o|--out",-20}{"\t"}Specify the output directory path. (default: Typings)
  {"-b|--bom",-20}{"\t"}Set BOM of utf-8 for output files.
");
        }

        public void Run(string[] args)
        {
            var conArgs = new ConArgs(args, "-");
            if (conArgs.Properties.For(x => x.ContainsKey("-h") || x.ContainsKey("--help")))
            {
                PrintUsage();
                return;
            }

            var outFolder = conArgs["-o"]?.FirstOrDefault() ?? conArgs["--out"].FirstOrDefault() ?? ".";
            var setBOM = conArgs.Properties.ContainsKey("-b") || conArgs.Properties.ContainsKey("--bom");

            GenerateTypeScript(outFolder, setBOM);
        }

        private static void GenerateTypeScript(string outFolder, bool setBOM)
        {
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var targetAssemblyName = Program.ProjectInfo.AssemblyName;
            var assemblyContext = new AssemblyContext($"{TargetBinFolder}/{targetAssemblyName}.dll", DotNetFramework.Parse(Program.ProjectInfo.TargetFramework));

            var types = assemblyContext.RootAssembly.GetTypesWhichExtends<DbContext>(true);
            foreach (var type in types)
            {
                var outFile = $"{Path.GetFullPath($"{outFolder}/{type.Name}.html")}";

                var builder = new DbStructureBuilder();
                builder.Cache(type);
                var csvContent = builder.GetHtml();
                var bytes = setBOM
                    ? new byte[] { 0xef, 0xbb, 0xbf }.Concat(csvContent.Bytes(Encoding.UTF8)).ToArray()
                    : csvContent.Bytes(Encoding.UTF8).ToArray();

                File.WriteAllBytes(outFile, bytes);
                Console.WriteLine($"File Saved: {outFile}");
            }
        }

    }
}
