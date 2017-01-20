using System;
using System.IO;

namespace Bluewire.TypeScriptSourceGenerator
{
    class Program
    {
        const string rootOutputDirectory = @"C:\TypeScriptSourceGenerator\Output";
        const string subdirectory = @".\Subdirectory";
        const int modulesToGenerate = 30;

        static void Main(string[] args)
        {
            Directory.CreateDirectory(rootOutputDirectory);
            Directory.CreateDirectory(Path.Combine(rootOutputDirectory, subdirectory));

            CreateMainSourceFile();
            CreateModuleSourceFiles();
        }

        static void CreateMainSourceFile()
        {
            var importLines = new string[modulesToGenerate];
            var invokationLines = new string[modulesToGenerate];

            for (var i = 0; i < modulesToGenerate; i++)
            {
                importLines[i] = $"import {{ lambda{i + 1} }} from './Subdirectory/Module{i + 1}';";
                invokationLines[i] = $"lambda{i + 1}();";
            }

            var importBlock = string.Join(Environment.NewLine, importLines) + Environment.NewLine;
            var invokationBlock = string.Join(Environment.NewLine, invokationLines) + Environment.NewLine;
            var content = importBlock + Environment.NewLine + invokationBlock;

            EmitFile("Main.ts", content);
        }

        static void CreateModuleSourceFiles()
        {
            for (var i = 0; i < modulesToGenerate; i++)
            {
                var filename = $"Module{i + 1}.ts";
                var content = $"export const lambda{i + 1} = () => console.log('lambda{i + 1} invoked');" + Environment.NewLine;

                EmitFile(filename, content, subdirectory);
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
