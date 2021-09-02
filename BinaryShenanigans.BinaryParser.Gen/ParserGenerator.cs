using System.IO;
using System.Reflection;
using System.Text;
using BinaryShenanigans.BinaryParser.Interfaces;
using CodeWriterUtils;
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

            var codeWriter = new CodeWriter(new CodeWriterSettings("\n", 4));
            codeWriter.WriteLine("using System;");
            codeWriter.WriteLine("using BinaryShenanigans.BinaryParser.Interfaces;");
            codeWriter.WriteLine("using BinaryShenanigans.Reader;");
            codeWriter.WriteNewLine();

            using (codeWriter.UseBrackets($"namespace {projectName}.Generated"))
            {
                using (codeWriter.UseBrackets($"public class {baseType.Name}Parser : IBinaryParser<{baseType.FullName}>"))
                {
                    using (codeWriter.UseBrackets($"public static {baseType.FullName} ParseStatic(ReadOnlySpan<byte> span)"))
                    {
                        codeWriter.WriteLine($"var res = new {baseType.FullName}();");
                        codeWriter.WriteLine("var reader = new SpanReader(0, span.Length);");
                        codeWriter.WriteNewLine();

                        binaryParserBuilder.WriteCode(codeWriter);

                        codeWriter.WriteNewLine();
                        codeWriter.WriteLine("return res;");
                    }

                    codeWriter.WriteNewLine();
                    codeWriter.WriteLine($"public {baseType.FullName} Parse(ReadOnlySpan<byte> span) => ParseStatic(span);");
                }
            }

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

            File.WriteAllText(outputFile, codeWriter.ToString(), Encoding.UTF8);
            return true;
        }
    }
}
