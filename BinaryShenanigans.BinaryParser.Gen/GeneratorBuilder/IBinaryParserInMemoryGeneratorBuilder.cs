using System;
using System.Reflection;
using BinaryShenanigans.BinaryParser.Gen.Generator;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace BinaryShenanigans.BinaryParser.Gen.GeneratorBuilder
{
    [PublicAPI]
    public interface IBinaryParserInMemoryGeneratorBuilder
    {
        IBinaryParserInMemoryGeneratorBuilder WithLogger(ILogger logger);

        IBinaryParserInMemoryGeneratorBuilder AddType(TypeInfo type);

        IBinaryParserInMemoryGeneratorBuilder AddTypes(params TypeInfo[] types);

        IBinaryParserInMemoryGenerator Build();
    }
}
