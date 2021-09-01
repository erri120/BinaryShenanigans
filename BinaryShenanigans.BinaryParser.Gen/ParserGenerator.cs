using System.IO;
using System.Reflection;
using System.Text;
using BinaryShenanigans.BinaryParser.Interfaces;
using Spectre.Console;

namespace BinaryShenanigans.BinaryParser.Gen
{
    public static class ParserGenerator
    {
        public static bool Generate(string projectPath, string projectName, TypeInfo typeInfo)
        {
            AnsiConsole.WriteLine($"Generating Parser for {typeInfo}");
            var projectDirectory = Path.GetDirectoryName(projectPath)!;

            var binaryParserBuilder = ReflectionUtils.InvokeMethod<ABinaryParserBuilder>(typeInfo, nameof(IBinaryParserConfiguration<string>.Configure));
            var baseType = binaryParserBuilder.Type;

            var sb = new StringBuilder($@"using System;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.Reader;

namespace {projectName}.Generated
{{
    public class {baseType.Name}Parser : IBinaryParser<{baseType.FullName}>
    {{
        public {baseType.FullName} Parse(byte[] data)
        {{
            var res = new {baseType.FullName}();
            var reader = new SpanReader(0, data.Length);
            var span = new ReadOnlySpan<byte>(data, 0, data.Length);
");

            foreach (var step in binaryParserBuilder.Steps)
            {
                step.WriteCode(sb);
            }

            sb.Append(@"

            return res;
        }
    }
}");

            var outputFile = Path.Combine(projectDirectory, "Generated", baseType.Name + "Parser.cs");
            if (File.Exists(outputFile))
            {
                // TODO: overwrite option
                AnsiConsole.WriteLine($"Removing existing Parser at {outputFile}");
                File.Delete(outputFile);
            }

            var outputFileDirectory = Path.GetDirectoryName(outputFile)!;
            if (!Directory.Exists(outputFileDirectory))
                Directory.CreateDirectory(outputFileDirectory);

            File.WriteAllText(outputFile, sb.ToString(), Encoding.UTF8);
            return true;
        }
    }
}
