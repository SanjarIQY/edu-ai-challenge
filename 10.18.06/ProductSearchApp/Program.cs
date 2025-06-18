using Microsoft.Extensions.Configuration;
using ProductSearchApp.Application;
using ProductSearchApp.Models;
using ProductSearchApp.Services;

namespace ProductSearchApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var configuration = BuildConfiguration();
                var apiKey = GetApiKey(configuration);
                var application = ConfigureApplication(apiKey);
                
                await application.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static string GetApiKey(IConfiguration configuration)
        {
            // Try to get from appsettings.json first
            var appSettings = configuration.Get<AppSettings>();
            var apiKey = appSettings?.OpenAI?.ApiKey;

            // Fallback to environment variable
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                apiKey = configuration["OPENAI_API_KEY"];
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.WriteLine("Error: OpenAI API key not found.");
                Console.WriteLine("Please set it in one of these ways:");
                Console.WriteLine("1. Add it to appsettings.json:");
                Console.WriteLine("   {");
                Console.WriteLine("     \"OpenAI\": {");
                Console.WriteLine("       \"ApiKey\": \"your-api-key-here\"");
                Console.WriteLine("     }");
                Console.WriteLine("   }");
                Console.WriteLine();
                Console.WriteLine("2. Set as environment variable:");
                Console.WriteLine("   export OPENAI_API_KEY=\"your-api-key-here\"");
                Environment.Exit(1);
            }

            return apiKey;
        }

        private static ProductSearchApplication ConfigureApplication(string apiKey)
        {
            // Simple dependency injection without external container
            var productService = new ProductService();
            var openAIService = new OpenAIService(apiKey);
            var displayService = new ConsoleDisplayService();

            return new ProductSearchApplication(productService, openAIService, displayService);
        }
    }
}
