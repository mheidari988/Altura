using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.IO;

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

            // Read the connection string
            var connectionString = configuration.GetConnectionString("AlturaCMS");

            // Read scaffold settings
            var scaffoldSettings = configuration.GetSection("ScaffoldSettings");

            string outputDir = scaffoldSettings.GetValue<string>("OutputDir") ?? throw new ArgumentNullException("OutputDir is required.");
            string contextName = scaffoldSettings.GetValue<string>("ContextName") ?? throw new ArgumentNullException("ContextName is required.");
            string schema = scaffoldSettings.GetValue<string>("Schema") ?? throw new ArgumentNullException("Schema is required.");
            string workingDir = scaffoldSettings.GetValue<string>("WorkingDir") ?? throw new ArgumentNullException("WorkingDir is required.");
            string project = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, scaffoldSettings.GetValue<string>("Project") ?? throw new ArgumentNullException("OutputDir is required.")));
            string contextDir = scaffoldSettings.GetValue<string>("ContextDir") ?? throw new ArgumentNullException("OutputDir is required.");
            string @namespace = scaffoldSettings.GetValue<string>("Namespace") ?? throw new ArgumentNullException("OutputDir is required.");
            string provider = scaffoldSettings.GetValue<string>("Provider") ?? throw new ArgumentNullException("OutputDir is required.");
            bool force = scaffoldSettings.GetValue<bool>("Force");
            bool noOnConfiguring = scaffoldSettings.GetValue<bool>("NoOnConfiguring");

            Console.WriteLine($"Project Path: {project}");

            // Construct the arguments for the scaffold command
            string arguments = $"ef dbcontext scaffold \"{connectionString}\" {provider} " +
                               $"--output-dir {outputDir} " +
                               $"--context {contextName} " +
                               $"--schema {schema} " +
                               $"--project \"{project}\" " +
                               $"--context-dir {contextDir} " +
                               $"--namespace {@namespace} " +
                               $"{(force ? "--force" : "")} " +
                               $"{(noOnConfiguring ? "--no-onconfiguring" : "")} " +
                               "--use-database-names";  // Add this flag

            Console.WriteLine($"Scaffold Arguments: {arguments}");
            Console.WriteLine();

            // Execute the dotnet ef command using the Process class
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetFullPath($"{workingDir}")
                }
            };

            process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
            process.ErrorDataReceived += (sender, e) => Console.WriteLine("ERROR: " + e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            Console.WriteLine("Scaffolding completed.");
        }
    }
}
