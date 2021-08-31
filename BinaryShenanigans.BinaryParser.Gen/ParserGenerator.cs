using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using BinaryShenanigans.BinaryParser.Interfaces;
using BinaryShenanigans.BinaryParser.Steps;
using BinaryShenanigans.Reader;
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
            var spanReader = new SpanReader(0, data.Length);
            var span = new ReadOnlySpan<byte>(data, 0, data.Length);");

            foreach (var step in binaryParserBuilder.Steps)
            {
                WriteStepCode(sb, step);
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

        private static void WriteStepCode(StringBuilder sb, AStep step)
        {
            switch (step.StepType)
            {
                case StepType.ReadNumerics:
                    WriteReadNumericsStep(sb, (ReadNumericsStep)step);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void WriteReadNumericsStep(StringBuilder sb, ReadNumericsStep step)
        {
            var expression = step.Expression;
            if (expression.NodeType != ExpressionType.Lambda)
                throw new NotImplementedException();

            var lambdaExpression = (LambdaExpression)expression;
            var lambdaBody = lambdaExpression.Body;

            if (lambdaBody.NodeType != ExpressionType.MemberAccess)
                throw new NotImplementedException();

            var memberExpression = (MemberExpression)lambdaBody;
            var memberName = memberExpression.Member.Name;

            var readerFunction = step.NumericType switch
            {
                NumericType.Int16 => nameof(SpanReader.ReadInt16),
                NumericType.UInt16 => nameof(SpanReader.ReadUInt16),
                NumericType.Int32 => nameof(SpanReader.ReadInt32),
                NumericType.UInt32 => nameof(SpanReader.ReadUInt16),
                NumericType.Int64 => nameof(SpanReader.ReadInt64),
                NumericType.UInt64 => nameof(SpanReader.ReadUInt16),
                NumericType.Single => nameof(SpanReader.ReadSingle),
                NumericType.Double => nameof(SpanReader.ReadDouble),
                NumericType.Half => nameof(SpanReader.ReadHalf),
                _ => throw new ArgumentOutOfRangeException()
            };

            sb.Append($@"
            res.{memberName} = spanReader.{readerFunction}(span, {(step.LittleEndian ? "true" : "false")});");
        }
    }
}
