using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using BinaryShenanigans.BinaryParser.Interfaces;
using Spectre.Console;
using Spectre.Console.Cli;

namespace BinaryShenanigans.BinaryParser.Gen.Commands
{
    public class GenerateCommand : Command<GenerateCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [Description("Path to the project.")]
            [CommandArgument(0, "[PROJECT]")]
            public string ProjectPath { get; set; } = string.Empty;
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            var projectPath = Path.GetFullPath(settings.ProjectPath);
            if (!File.Exists(projectPath))
                throw new Exception($"File does not exist: \"{projectPath}\"");

            var buildResult = NukeUtils.BuildProject(projectPath);
            var assemblyLocation = buildResult.AssemblyLocation;
            var projectName = buildResult.ProjectName;
            //const string assemblyLocation = "E:\\Projects\\BinaryShenanigans\\BinaryShenanigans.Example\\bin\\Debug\\net6.0\\BinaryShenanigans.Example.dll";
            //const string projectName = "BinaryShenanigans.Example";

            if (!File.Exists(assemblyLocation))
                throw new Exception($"Output Assembly does not exist at {assemblyLocation}");

            var loadContext = new AssemblyLoadContext("SomethingSomethingName", true);
            var assembly = loadContext.LoadFromAssemblyPath(assemblyLocation);

            var configurationType = typeof(IBinaryParserConfiguration<>);
            var configurationClasses = ReflectionUtils.FindInterfaceImplementations(assembly, configurationType);

            if (!configurationClasses.Any())
            {
                AnsiConsole.Render(new Markup($"[bold red]Unable to find any Types that implement {configurationType}[/]"));
                return 1;
            }

            AnsiConsole.WriteLine($"Found {configurationClasses.Count} configuration type(s)");

            foreach (var configurationClass in configurationClasses)
            {
                var res = ParserGenerator.Generate(projectPath, projectName, configurationClass);
            }

            loadContext.Unload();

            // TODO: assembly unloading so we don't get "The process cannot access the file"
            //https://github.com/Noggog/CSharpExt/blob/79367c736146b26f96bb0eb0f48d1a0e4db4996f/Noggog.CSharpExt/Utility/AssemblyLoading.cs#L52

            // TODO: build
            //var buildResult = NukeUtils.BuildProject(projectPath);
            //return buildResult.Success;
            return 0;
        }
    }
}
