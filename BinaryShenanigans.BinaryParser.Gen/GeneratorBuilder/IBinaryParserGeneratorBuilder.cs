using BinaryShenanigans.BinaryParser.Gen.Generator;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace BinaryShenanigans.BinaryParser.Gen.GeneratorBuilder
{
    [PublicAPI]
    public interface IBinaryParserGeneratorBuilder
    {
        IBinaryParserGeneratorBuilder WithLogger(ILogger logger);

        IBinaryParserGeneratorBuilderAddProjectBranch AddProject(string projectPath);

        IBinaryParserGenerator Build();
    }

    [PublicAPI]
    public interface IBinaryParserGeneratorBuilderAddProjectBranch : IBinaryParserGeneratorBuilder
    {
        IBinaryParserGeneratorBuilderAddProjectBranch WithGeneratedSourcesOutputFolder(string generatedSourcesOutputFolder);

        IBinaryParserGeneratorBuilderAddProjectBranch WithNamespace(string @namespace);
    }
}
