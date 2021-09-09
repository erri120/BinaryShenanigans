using System;
using BinaryShenanigans.BinaryParser.Gen.GeneratorBuilder;
using Microsoft.Extensions.Logging;

namespace BinaryShenanigans.BinaryParser.GenRunner
{
    public static class Program
    {
        public static void Main()
        {
            using var factory = LoggerFactory.Create(
                b => b
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Trace));

            var generator = BinaryParserGeneratorBuilder
                .CreateBuilder()
                .WithLogger(factory.CreateLogger("BinaryParserGenerator"))
                .AddProject("../../../../BinaryShenanigans.Example/BinaryShenanigans.Example.csproj")
                .Build();

            if (!generator.Run())
                throw new NotImplementedException();
        }
    }
}
