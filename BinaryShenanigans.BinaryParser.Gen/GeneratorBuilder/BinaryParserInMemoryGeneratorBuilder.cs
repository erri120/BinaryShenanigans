using System;
using System.Collections.Generic;
using System.Reflection;
using BinaryShenanigans.BinaryParser.Gen.Generator;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace BinaryShenanigans.BinaryParser.Gen.GeneratorBuilder
{
    [PublicAPI]
    public class BinaryParserInMemoryGeneratorBuilder : IBinaryParserInMemoryGeneratorBuilder
    {
        private ILogger _logger = NullLogger.Instance;
        private readonly List<TypeInfo> _types = new();

        public static IBinaryParserInMemoryGeneratorBuilder CreateBuilder()
        {
            return new BinaryParserInMemoryGeneratorBuilder();
        }

        private BinaryParserInMemoryGeneratorBuilder() { }

        public IBinaryParserInMemoryGeneratorBuilder WithLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public IBinaryParserInMemoryGeneratorBuilder AddType(TypeInfo type)
        {
            // TODO: check if type implements IBinaryParserConfiguration
            _types.Add(type);
            return this;
        }

        public IBinaryParserInMemoryGeneratorBuilder AddTypes(params TypeInfo[] types)
        {
            foreach (var type in types)
            {
                AddType(type);
            }

            return this;
        }

        public IBinaryParserInMemoryGenerator Build()
        {
            return new BinaryParserInMemoryGenerator(_logger, _types);
        }
    }
}
