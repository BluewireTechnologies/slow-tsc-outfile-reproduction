using System;
using System.IO;

namespace Bluewire.TypeScriptSourceGenerator
{
    class Program
    {
        const string rootOutputDirectory = @"C:\TypeScriptSourceGenerator\Output";
        const int modulesPerTier = 30;

        static void Main(string[] args)
        {
            CreateMainSourceFile();
            CreateFirstTierModules();
        }

        static void CreateMainSourceFile()
        {
            Directory.CreateDirectory(rootOutputDirectory);

            var importLines = new string[modulesPerTier];
            var invokationLines = new string[modulesPerTier];

            for (var i = 0; i < modulesPerTier; i++)
            {
                importLines[i] = $"import {{ tier1Lambda{i + 1} }} from './FirstTierModules/Tier1Module{i + 1}';";
                invokationLines[i] = $"tier1Lambda{i + 1}();";
            }

            var importBlock = string.Join(Environment.NewLine, importLines) + Environment.NewLine;
            var invokationBlock = string.Join(Environment.NewLine, invokationLines) + Environment.NewLine;
            var content = importBlock + Environment.NewLine + invokationBlock;

            EmitFile("Main.ts", content);
        }

        static void CreateFirstTierModules()
        {
            var directoryPath = @".\FirstTierModules";

            Directory.CreateDirectory(Path.Combine(rootOutputDirectory, directoryPath));

            for (var i = 0; i < modulesPerTier; i++)
            {
                EmitFile(
                    $"Tier1Module{i + 1}.ts",
                    $"export const tier1Lambda{i + 1} = () => console.log('tier1Lambda{i + 1} invoked');" + Environment.NewLine,
                    directoryPath);
            }
        }

        static void EmitFile(string filename, string content, string subdirectory = @".\")
        {
            var directory = Path.Combine(rootOutputDirectory, subdirectory);
            var path = Path.Combine(directory, filename);

            File.WriteAllText(path, content);
        }
    }
}
