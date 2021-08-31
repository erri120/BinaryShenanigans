using System;
using System.IO;
using System.Linq;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Spectre.Console;

namespace BinaryShenanigans.BinaryParser.Gen
{
    public static class NukeUtils
    {
        public record BuildProjectResult(bool Success, string ProjectName, string AssemblyLocation);

        public static BuildProjectResult BuildProject(string projectPath)
        {
            AnsiConsole.WriteLine($"Building project {projectPath}");
            var build = DotNetTasks.DotNetBuild(x => x
                .SetProjectFile(projectPath));

            var buildErrors = build.Where(x => x.Type == OutputType.Err).ToList();
            if (buildErrors.Any())
            {
                AnsiConsole.WriteLine("Build finished with errors:");
                foreach (var errOutput in buildErrors)
                {
                    AnsiConsole.Render(new Markup($"[bold red]{errOutput.Text}[/]"));
                }

                return new BuildProjectResult(false, string.Empty, string.Empty);
            }

            AnsiConsole.WriteLine("Build finished successfully");

            var projectName = projectPath.Split(".csproj")[0].Split(Path.DirectorySeparatorChar).Last();
            var assemblyLocation = build
                .First(x => x.Text.Contains($"{projectName} -> ", StringComparison.OrdinalIgnoreCase))
                .Text
                .Replace($"{projectName} -> ", "")
                .Trim();

            AnsiConsole.WriteLine($"Project Name: {projectName}");
            AnsiConsole.WriteLine($"Assembly Location: {assemblyLocation}");
            return new BuildProjectResult(true, projectName, assemblyLocation);
        }
    }
}
