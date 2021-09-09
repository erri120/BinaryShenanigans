using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using BinaryShenanigans.BinaryParser.Gen.GeneratorBuilder;
using BinaryShenanigans.BinaryParser.Gen.Utils;
using BinaryShenanigans.BinaryParser.Interfaces;
using CodeWriterUtils;
using Microsoft.Extensions.Logging;

namespace BinaryShenanigans.BinaryParser.Gen.Generator
{
    internal class BinaryParserGenerator : IBinaryParserGenerator
    {
        private readonly ILogger _logger;
        private readonly List<ProjectSettings> _projects;

        public BinaryParserGenerator(ILogger logger, List<ProjectSettings> projects)
        {
            _logger = logger;
            _projects = projects;
        }

        public bool Run()
        {
            return _projects.All(BuildForProject);
        }

        private bool BuildForProject(ProjectSettings projectSettings)
        {
            var projectPath = projectSettings.ProjectPath;
            using var projectLoggingScope = _logger.BeginScope(projectPath);

            var (buildSuccess, projectName, assemblyLocation) = NukeUtils.BuildProject(_logger, projectPath);
            if (!buildSuccess)
            {
                _logger.LogError("Build was not successful!");
                return false;
            }

            if (!File.Exists(assemblyLocation))
            {
                _logger.LogError("Output Assembly does not exist at {AssemblyLocation}", assemblyLocation);
                return false;
            }

            var loadContext = new AssemblyLoadContext("SomethingSomethingName", true);
            var assembly = loadContext.LoadFromAssemblyPath(assemblyLocation);

            var configurationType = typeof(IBinaryParserConfiguration<>);
            var configurationClasses = ReflectionUtils.FindInterfaceImplementations(assembly, configurationType);

            if (!configurationClasses.Any())
            {
                _logger.LogError("Unable to find any Types that implement {ConfigurationType}", configurationType);
                return false;
            }

            _logger.LogDebug("Found {Count} configuration type(s)", configurationClasses.Count);
            foreach (var configurationClass in configurationClasses)
            {
                if (Generate(projectSettings, projectName, configurationClass)) continue;
                _logger.LogError("Unable to generate Parser for {ConfigurationClass}", configurationClass.Name);
                return false;
            }

            _logger.LogInformation("Finished generating Parsers for Project {ProjectName}", projectName);
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

            _logger.LogDebug("Generating Parser for {ConfigurationClass}", typeInfo.Name);
            var projectDirectory = Path.GetDirectoryName(projectPath)!;

            var binaryParserBuilder = ReflectionUtils.InvokeMethod<ABinaryParserBuilder>(typeInfo, nameof(IBinaryParserConfiguration<string>.Configure));
            var baseType = binaryParserBuilder.Type;

            var codeWriter = new CodeWriter(new CodeWriterSettings("\n", 4));
            codeWriter.WriteLine("using System;");
            codeWriter.WriteLine("using BinaryShenanigans.BinaryParser.Interfaces;");
            codeWriter.WriteLine("using BinaryShenanigans.Reader;");
            codeWriter.WriteNewLine();

            using (codeWriter.UseBrackets($"namespace {projectName}.{projectSettings.Namespace}"))
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

            var outputFile = Path.Combine(projectDirectory, projectSettings.GeneratedSourcesOutputFolder, baseType.Name + "Parser.cs");
            if (File.Exists(outputFile))
            {
                // TODO: overwrite option
                _logger.LogDebug("Removing existing Parser at {Path}", outputFile);
                File.Delete(outputFile);
            }

            var outputFileDirectory = Path.GetDirectoryName(outputFile)!;
            if (!Directory.Exists(outputFileDirectory))
                Directory.CreateDirectory(outputFileDirectory);

            _logger.LogDebug("Writing Parser to {File}", outputFile);
            File.WriteAllText(outputFile, codeWriter.ToString(), Encoding.UTF8);
            return true;
        }
    }
}
