using System;
using System.IO;

namespace Bluewire.TypeScriptSourceGenerator
{
    class Program
    {
        const string rootOutputDirectory = @"C:\TypeScriptSourceGenerator\Output";
        const string subdirectory = @".\Subdirectory";

        static void Main(string[] args)
        {
            Directory.CreateDirectory(rootOutputDirectory);
            Directory.CreateDirectory(Path.Combine(rootOutputDirectory, subdirectory));

            var mainSource =
                "import { helloWorld } from './Subdirectory/Module';" + Environment.NewLine +
                "console.log(helloWorld);" + Environment.NewLine;

            EmitFile("Main.ts", mainSource);
            EmitFile("Module.ts", "export const helloWorld = 'Hello, world!';" + Environment.NewLine, subdirectory);
        }

        static void EmitFile(string filename, string content, string subdirectory = @".\")
        {
            var directory = Path.Combine(rootOutputDirectory, subdirectory);
            var path = Path.Combine(directory, filename);

            File.WriteAllText(path, content);
        }
    }
}
