using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace BinaryShenanigans.BinaryParser.Gen.Utils
{
    internal static class NukeUtils
    {
        public record BuildProjectResult(bool Success, string ProjectName, string AssemblyLocation);

        public static BuildProjectResult BuildProject(ILogger logger, string projectPath)
        {
            logger.LogInformation("Building project {Path}", projectPath);
            var build = DotNetTasks.DotNetBuild(x => x
                .SetProjectFile(projectPath));

            var buildErrors = build.Where(x => x.Type == OutputType.Err).ToList();
            if (buildErrors.Any())
            {
                logger.LogWarning("Build finished with {ErrorCount} errors", buildErrors.Count);
                foreach (var errOutput in buildErrors)
                {
                    logger.LogError("Error: {Error}", errOutput.Text);
                }

                return new BuildProjectResult(false, string.Empty, string.Empty);
            }

            logger.LogInformation("Build finished successfully");

            var projectName = projectPath.Split(".csproj")[0].Split(Path.DirectorySeparatorChar).Last();
            var assemblyLocation = build
                .First(x => x.Text.Contains($"{projectName} -> ", StringComparison.OrdinalIgnoreCase))
                .Text
                .Replace($"{projectName} -> ", "")
                .Trim();

            logger.LogDebug("Project Name: {ProjectName}", projectName);
            logger.LogDebug("Assembly Location: {AssemblyLocation}", assemblyLocation);
            return new BuildProjectResult(true, projectName, assemblyLocation);
        }
    }
}
