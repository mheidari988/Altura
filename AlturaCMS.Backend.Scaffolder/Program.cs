using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Scriban;

namespace AlturaCMS.Backend.Scaffolder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var solutionName = "MyTestBackend";
            var projectName = "AlturaExample.Domain";
            var projectPath = Path.Combine(solutionName, projectName);  // Project path inside the solution folder
            var vsVersion = "17.11.35222.181";
            var projectTypeGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"; // .NET Project type GUID
            var targetFramework = "net8.0";

            // NuGet references (if any)
            var nugetReferences = new List<NugetPackage>
            {
                new NugetPackage { Name = "Newtonsoft.Json", Version = "13.0.1" },
                new NugetPackage { Name = "Dapper", Version = "2.0.78" }
            };

            // Internal project references (if any)
            var projectReferences = new List<ProjectReference>
            {
                new ProjectReference { Path = "..\\AlturaExample.Core\\AlturaExample.Core.csproj" }
            };

            // Generate the solution and the project files
            GenerateSolutionFile(solutionName, projectName, projectPath, vsVersion, projectTypeGuid);
            GenerateProjectFile(projectName, projectPath, targetFramework, nugetReferences, projectReferences);

            Console.WriteLine("Scaffolding completed.");
        }

        private static void GenerateSolutionFile(string solutionName, string projectName, string projectPath, string vsVersion, string projectTypeGuid)
        {
            // Generate Solution file
            var solutionTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Solution.sbn");
            var solutionTemplateContent = File.ReadAllText(solutionTemplatePath);

            // Generate the necessary GUIDs
            var solutionGuid = Guid.NewGuid().ToString().ToUpper();
            var projectGuid = Guid.NewGuid().ToString().ToUpper();

            // Parse and render the Scriban template
            var solutionTemplate = Scriban.Template.Parse(solutionTemplateContent);
            var renderedSolution = solutionTemplate.Render(new
            {
                vs_version = vsVersion,
                project_type_guid = projectTypeGuid,
                project_name = projectName,
                project_path = projectPath.Replace("\\", "\\\\"), // Escape backslashes for Scriban
                project_guid = projectGuid,
                solution_guid = solutionGuid
            });

            // Create a directory for the solution
            Directory.CreateDirectory(solutionName);

            // Write the solution file
            var solutionFilePath = Path.Combine(solutionName, $"{solutionName}.sln");
            File.WriteAllText(solutionFilePath, renderedSolution);
        }

        private static void GenerateProjectFile(string projectName, string projectPath, string targetFramework, List<NugetPackage> nugetReferences, List<ProjectReference> projectReferences)
        {
            // Load the Scriban template for the .csproj file
            var projectTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Project.sbn");
            var projectTemplateContent = File.ReadAllText(projectTemplatePath);

            // Parse the Scriban template
            var projectTemplate = Scriban.Template.Parse(projectTemplateContent);

            // Render the template with dynamic data
            var renderedProject = projectTemplate.Render(new
            {
                project_name = projectName,
                target_framework = targetFramework,
                nuget_references = nugetReferences,
                project_references = projectReferences
            });

            // Create a directory for the project inside the solution folder
            Directory.CreateDirectory(projectPath);

            // Write the project file (.csproj)
            var projectFilePath = Path.Combine(projectPath, $"{projectName}.csproj");
            File.WriteAllText(projectFilePath, renderedProject);
        }
    }

    // Classes to represent NuGet and project references
    public class NugetPackage
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class ProjectReference
    {
        public string Path { get; set; }
    }
}
