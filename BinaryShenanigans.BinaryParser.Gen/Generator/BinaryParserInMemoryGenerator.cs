using System.Collections.Generic;
using System.Reflection;
using BinaryShenanigans.BinaryParser.Gen.Utils;
using BinaryShenanigans.BinaryParser.Interfaces;
using Microsoft.Extensions.Logging;

namespace BinaryShenanigans.BinaryParser.Gen.Generator
{
    internal class BinaryParserInMemoryGenerator : ABinaryParserGenerator, IBinaryParserInMemoryGenerator
    {
        private readonly List<TypeInfo> _types;

        public Dictionary<string, string> GeneratedOutput { get; } = new();

        public BinaryParserInMemoryGenerator(ILogger logger, List<TypeInfo> types) : base(logger)
        {
            _types = types;
        }

        public bool Run()
        {
            foreach (var typeInfo in _types)
            {
                var assemblyName = typeInfo.Assembly.GetName();

                var binaryParserBuilder = ReflectionUtils.InvokeMethod<ABinaryParserBuilder>(typeInfo, nameof(IBinaryParserConfiguration<string>.Configure));
                var baseType = binaryParserBuilder.Type;

                var generatedCode = GenerateParserCodeForType($"{assemblyName.Name}.Generated", typeInfo, binaryParserBuilder);

                GeneratedOutput.Add($"{baseType.Name}Parser.cs", generatedCode);
            }

            return true;
        }
    }
}
