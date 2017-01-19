using System;
using System.IO;

namespace Bluewire.TypeScriptSourceGenerator
{
    class Program
    {
        const string outputDirectory = @"C:\TypeScriptSourceGenerator\Output";

        static void Main(string[] args)
        {
            var filename = "HelloWorld.js";
            var content = "console.log('Hello, world!');" + Environment.NewLine;
            var path = Path.Combine(outputDirectory, filename);

            Directory.CreateDirectory(outputDirectory);
            File.WriteAllText(path, content);
        }
    }
}
