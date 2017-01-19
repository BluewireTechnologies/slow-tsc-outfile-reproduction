using System;
using System.IO;

namespace Bluewire.TypeScriptSourceGenerator
{
    class Program
    {
        const string outputDirectory = @"C:\TypeScriptSourceGenerator\Output";

        static void Main(string[] args)
        {
            Directory.CreateDirectory(outputDirectory);

            var mainSource =
                "import { helloWorld } from './ExportedConstant';" + Environment.NewLine +
                "console.log(helloWorld);" + Environment.NewLine;

            EmitFile("Main.ts", mainSource);
            EmitFile("ExportedConstant.ts", "export const helloWorld = 'Hello, world!';" + Environment.NewLine);
        }

        static void EmitFile(string filename, string content)
        {
            var path = Path.Combine(outputDirectory, filename);
            File.WriteAllText(path, content);
        }
    }
}
