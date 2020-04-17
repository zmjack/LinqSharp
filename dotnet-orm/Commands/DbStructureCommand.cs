using DotNetCli;
using Ink;
using Microsoft.EntityFrameworkCore;
using NStandard;
using NStandard.Reference;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
  {"-o|--out",20}{"\t"}Specify the output directory path. (default: Typings)
  {"-b|--bom",20}{"\t"}Set BOM of utf-8 for output files.
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

            var outFolder = conArgs["-o"] ?? conArgs["--out"] ?? ".";
            var setBOM = conArgs.Properties.ContainsKey("-b") || conArgs.Properties.ContainsKey("-bom");

            GenerateTypeScript(outFolder, setBOM);
        }

        private static void GenerateTypeScript(string outFolder, bool setBOM)
        {
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var assemblyName = Program.ProjectInfo.AssemblyName;
            var assembly = Assembly.LoadFrom($"{TargetBinFolder}/{assemblyName}.dll");
            AppDomain.CurrentDomain.AssemblyResolve += GAC.CreateAssemblyResolver(Program.ProjectInfo.TargetFramework, GACFolders.All);

            var types = assembly.GetTypesWhichExtends<DbContext>(true);
            foreach (var type in types)
            {
                var outFile = $"{Path.GetFullPath($"{outFolder}/{type.Name}.csv")}";

                var builder = new DbStructureBuilder();
                builder.Cache(type);
                var csvContent = builder.GetCsv();
                var bytes = setBOM
                    ? new byte[] { 0xef, 0xbb, 0xbf }.Concat(csvContent.Bytes(Encoding.UTF8)).ToArray()
                    : csvContent.Bytes(Encoding.UTF8).ToArray();

                File.WriteAllBytes(outFile, bytes);
                Console.WriteLine($"File Saved: {outFile}");
            }
        }

    }
}
