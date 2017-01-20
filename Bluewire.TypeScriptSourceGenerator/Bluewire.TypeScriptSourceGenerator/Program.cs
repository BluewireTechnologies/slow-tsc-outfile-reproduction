using System;
using System.IO;

namespace Bluewire.TypeScriptSourceGenerator
{
    class Program
    {
        const string rootOutputDirectory = @"C:\TypeScriptSourceGenerator\Output";
        const int modulesPerSubdirectory = 30;

        static void Main(string[] args)
        {
            CreateMainSourceFile();
            CreateFirstTierModules();
            CreateSecondTierModules();
        }

        static void CreateMainSourceFile()
        {
            Directory.CreateDirectory(rootOutputDirectory);

            var importLines = new string[modulesPerSubdirectory];
            var invokationLines = new string[modulesPerSubdirectory];

            for (var i = 0; i < modulesPerSubdirectory; i++)
            {
                importLines[i] = $"import {{ tier1Lambda{i + 1} }} from './Tier1/Tier1Module{i + 1}';";
                invokationLines[i] = $"tier1Lambda{i + 1}();";
            }

            var filename = "Main.ts";
            var logStatement = "console.log('Executing Main.js');" + Environment.NewLine;
            var importBlock = string.Join(Environment.NewLine, importLines) + Environment.NewLine;
            var invokationBlock = string.Join(Environment.NewLine, invokationLines) + Environment.NewLine;
            var content = string.Join(Environment.NewLine, new[] { importBlock, logStatement, invokationBlock });

            EmitFile(filename, content);
        }

        static void CreateFirstTierModules()
        {
            var directoryPath = @".\Tier1";

            Directory.CreateDirectory(Path.Combine(rootOutputDirectory, directoryPath));

            for (var i = 0; i < modulesPerSubdirectory; i++)
            {
                var importLines = new string[modulesPerSubdirectory];
                var invokationLines = new string[modulesPerSubdirectory];

                for (var j = 0; j < modulesPerSubdirectory; j++)
                {
                    var lambdaName = $"tier2Group{i + 1}Lambda{j + 1}";
                    var moduleLocation = $"./Tier2Group{i + 1}/Tier2Group{i + 1}Module{j + 1}";

                    importLines[j] = $"import {{ {lambdaName} }} from '{moduleLocation}';";
                    invokationLines[j] = $"    tier2Group{i + 1}Lambda{j + 1}();";
                }

                var filename = $"Tier1Module{i + 1}.ts";
                var logStatement = $"    console.log('tier1Lambda{i + 1} invoked');" + Environment.NewLine;
                var importBlock = string.Join(Environment.NewLine, importLines) + Environment.NewLine;
                var invokationBlock = string.Join(Environment.NewLine, invokationLines);
                var content = string.Join(Environment.NewLine, new[]
                {
                    importBlock,
                    $"export const tier1Lambda{i + 1} = () => {{",
                    logStatement,
                    invokationBlock,
                    "}" + Environment.NewLine
                });

                EmitFile(filename, content, directoryPath);
            }
        }

        static void CreateSecondTierModules()
        {
            for (var i = 0; i < modulesPerSubdirectory; i++)
            {
                var directoryPath = $@".\Tier1\Tier2Group{i + 1}";

                Directory.CreateDirectory(Path.Combine(rootOutputDirectory, directoryPath));

                for (var j = 0; j < modulesPerSubdirectory; j++)
                {
                    var filename = $"Tier2Group{i + 1}Module{j + 1}.ts";
                    var lambdaName = $"tier2Group{i + 1}Lambda{j + 1}";
                    var content =
                        $"export const {lambdaName} = () => console.log('{lambdaName} invoked');" +
                        Environment.NewLine;

                    EmitFile(filename, content, directoryPath);
                }
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
