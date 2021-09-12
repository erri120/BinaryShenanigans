using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using BinaryShenanigans.BinaryParser.Gen.GeneratorBuilder;
using BinaryShenanigans.BinaryParser.Gen.Utils;
using BinaryShenanigans.BinaryParser.Interfaces;
using Microsoft.Extensions.Logging;

namespace BinaryShenanigans.BinaryParser.Gen.Generator
{
    internal class BinaryParserGenerator : ABinaryParserGenerator, IBinaryParserGenerator
    {
        private readonly List<ProjectSettings> _projects;

        public BinaryParserGenerator(ILogger logger, List<ProjectSettings> projects) : base(logger)
        {
            _projects = projects;
        }

        public bool Run()
        {
            return _projects.All(BuildForProject);
        }

        private bool BuildForProject(ProjectSettings projectSettings)
        {
            var projectPath = projectSettings.ProjectPath;
            using var projectLoggingScope = Logger.BeginScope(projectPath);

            var (buildSuccess, projectName, assemblyLocation) = NukeUtils.BuildProject(Logger, projectPath);
            if (!buildSuccess)
            {
                Logger.LogError("Build was not successful!");
                return false;
            }

            if (!File.Exists(assemblyLocation))
            {
                Logger.LogError("Output Assembly does not exist at {AssemblyLocation}", assemblyLocation);
                return false;
            }

            var loadContext = new AssemblyLoadContext("SomethingSomethingName", true);
            var assembly = loadContext.LoadFromAssemblyPath(assemblyLocation);

            var configurationType = typeof(IBinaryParserConfiguration<>);
            var configurationClasses = ReflectionUtils.FindInterfaceImplementations(assembly, configurationType);

            if (!configurationClasses.Any())
            {
                Logger.LogError("Unable to find any Types that implement {ConfigurationType}", configurationType);
                return false;
            }

            Logger.LogDebug("Found {Count} configuration type(s)", configurationClasses.Count);
            foreach (var configurationClass in configurationClasses)
            {
                if (Generate(projectSettings, projectName, configurationClass)) continue;
                Logger.LogError("Unable to generate Parser for {ConfigurationClass}", configurationClass.Name);
                return false;
            }

            Logger.LogInformation("Finished generating Parsers for Project {ProjectName}", projectName);
            loadContext.Unload();

            // TODO: assembly unloading so we don't get "The process cannot access the file"
            //https://github.com/Noggog/CSharpExt/blob/79367c736146b26f96bb0eb0f48d1a0e4db4996f/Noggog.CSharpExt/Utility/AssemblyLoading.cs#L52

            // TODO: build
            //var buildResult = NukeUtils.BuildProject(projectPath);
            //return buildResult.Success;
            return true;
        }

        private bool Generate(ProjectSettings projectSettings, string projectName, TypeInfo typeInfo)
        {
            var projectPath = projectSettings.ProjectPath;
            var projectDirectory = Path.GetDirectoryName(projectPath)!;

            var binaryParserBuilder = ReflectionUtils.InvokeMethod<ABinaryParserBuilder>(typeInfo, nameof(IBinaryParserConfiguration<string>.Configure));
            var baseType = binaryParserBuilder.Type;

            var generatedCode = GenerateParserCodeForType($"{projectName}.{projectSettings.Namespace}", typeInfo, binaryParserBuilder);

            var outputFile = Path.Combine(projectDirectory, projectSettings.GeneratedSourcesOutputFolder, baseType.Name + "Parser.cs");
            if (File.Exists(outputFile))
            {
                // TODO: overwrite option
                Logger.LogDebug("Removing existing Parser at {Path}", outputFile);
                File.Delete(outputFile);
            }

            var outputFileDirectory = Path.GetDirectoryName(outputFile)!;
            if (!Directory.Exists(outputFileDirectory))
                Directory.CreateDirectory(outputFileDirectory);

            Logger.LogDebug("Writing Parser to {File}", outputFile);
            File.WriteAllText(outputFile, generatedCode, Encoding.UTF8);
            return true;
        }
    }
}
