using System.Reflection;
using BinaryShenanigans.BinaryParser.Gen.Utils;
using BinaryShenanigans.BinaryParser.Interfaces;
using CodeWriterUtils;
using Microsoft.Extensions.Logging;

namespace BinaryShenanigans.BinaryParser.Gen.Generator
{
    internal abstract class ABinaryParserGenerator
    {
        private protected readonly ILogger Logger;

        protected ABinaryParserGenerator(ILogger logger)
        {
            Logger = logger;
        }

        private protected string GenerateParserCodeForType(string @namespace, TypeInfo typeInfo, ABinaryParserBuilder binaryParserBuilder)
        {
            Logger.LogDebug("Generating Parser for {ConfigurationClass}", typeInfo.Name);

            //var binaryParserBuilder = ReflectionUtils.InvokeMethod<ABinaryParserBuilder>(typeInfo, nameof(IBinaryParserConfiguration<string>.Configure));
            var baseType = binaryParserBuilder.Type;

            var codeWriter = new CodeWriter(new CodeWriterSettings("\n", 4));
            codeWriter.WriteLine("using System;");
            codeWriter.WriteLine("using BinaryShenanigans.BinaryParser.Interfaces;");
            codeWriter.WriteLine("using BinaryShenanigans.Reader;");
            codeWriter.WriteNewLine();

            using (codeWriter.UseBrackets($"namespace {@namespace}"))
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

            Logger.LogDebug("Finished parser code generation for {ConfigurationClass}", typeInfo.Namespace);
            return codeWriter.ToString();
        }
    }
}
