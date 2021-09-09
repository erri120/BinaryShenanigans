using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinaryShenanigans.BinaryParser.Gen.Generator;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace BinaryShenanigans.BinaryParser.Gen.GeneratorBuilder
{
    [PublicAPI]
    public class BinaryParserGeneratorBuilder : IBinaryParserGeneratorBuilderAddProjectBranch
    {
        private ILogger _logger = NullLogger.Instance;
        private readonly List<ProjectSettings> _projects = new();

        private BinaryParserGeneratorBuilder() { }

        public static IBinaryParserGeneratorBuilder CreateBuilder()
        {
            return new BinaryParserGeneratorBuilder();
        }

        public IBinaryParserGeneratorBuilder WithLogger(ILogger logger)
        {
            _logger = logger;
            return this;
        }

        public IBinaryParserGeneratorBuilderAddProjectBranch AddProject(string projectPath)
        {
            projectPath = Path.GetFullPath(projectPath);
            if (!File.Exists(projectPath))
                throw new ArgumentException($"Project at {projectPath} does not exist!", nameof(projectPath));

            _projects.Add(new ProjectSettings(projectPath));
            return this;
        }

        public IBinaryParserGeneratorBuilderAddProjectBranch WithGeneratedSourcesOutputFolder(string generatedSourcesOutputFolder)
        {
            _projects.Last().GeneratedSourcesOutputFolder = generatedSourcesOutputFolder;
            return this;
        }

        public IBinaryParserGeneratorBuilderAddProjectBranch WithNamespace(string @namespace)
        {
            _projects.Last().Namespace = @namespace;
            return this;
        }

        public IBinaryParserGenerator Build()
        {
            return new BinaryParserGenerator(_logger, _projects);
        }
    }

    internal record ProjectSettings(string ProjectPath)
    {
        public string GeneratedSourcesOutputFolder { get; set; } = "Generated";

        public string Namespace { get; set; } = "Generated";
    };
}
