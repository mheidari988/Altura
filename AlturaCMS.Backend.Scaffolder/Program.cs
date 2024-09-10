using Microsoft.Extensions.Configuration;

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



            Console.WriteLine("Scaffolding completed.");
        }
    }
}
