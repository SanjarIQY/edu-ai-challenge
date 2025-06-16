using OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        Console.WriteLine("=== Service Analyzer ===");
        Console.WriteLine("Analyze services and get comprehensive business reports!");
        Console.WriteLine("Type 'exit' or 'quit' to close the application.\n");

        while (true)
        {
            Console.Write("Enter a service name or description: ");
            var userInput = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Please enter a service name or description.\n");
                continue;
            }

            if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit")
            {
                Console.WriteLine("Thank you for using Service Analyzer! Goodbye!");
                break;
            }

            Console.WriteLine($"\nGenerating report for: \"{userInput}\"");
            Console.WriteLine("Please wait, this may take a moment...\n");

            try
            {
                var report = await GenerateReportAsync(userInput);
                Console.WriteLine("--- Generated Report ---");
                Console.WriteLine(report);
                Console.WriteLine("------------------------\n");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}\n");
                Console.ResetColor();
            }
        }
    }

    private static async Task<string> GenerateReportAsync(string serviceInfo)
    {
        // Build configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Try environment variable first, then appsettings.json
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? configuration["OpenAI:ApiKey"];
        
        if (string.IsNullOrEmpty(apiKey) || apiKey == "your-openai-api-key-here")
        {
            throw new InvalidOperationException("OpenAI API key is not set. Please either:\n" +
                "1. Set environment variable: OPENAI_API_KEY=your-key\n" +
                "2. Update 'OpenAI:ApiKey' in appsettings.json with your actual API key.");
        }

        var client = new ChatClient("gpt-4.1-mini", apiKey);
        
        const string systemPrompt = @"
You are an expert business and technology analyst. Your task is to generate a comprehensive, markdown-formatted report on a given service or product.
The user will provide either a well-known service name or a raw description of a service. Based on this input, generate a report with the following sections exactly as listed:

- **Brief History**: Founding year, key milestones, and founders.
- **Target Audience**: Describe the primary user segments and their needs.
- **Core Features**: List the top 2-4 key functionalities of the service.
- **Unique Selling Points**: What makes the service different from its main competitors.
- **Business Model**: Explain how the service generates revenue (e.g., subscriptions, ads, freemium).
- **Tech Stack Insights**: Infer or state the known technologies used (frontend, backend, database, etc.).
- **Perceived Strengths**: Mention widely recognized positive aspects or standout features.
- **Perceived Weaknesses**: Cite common criticisms, potential drawbacks, or limitations.

Your response must be ONLY the markdown report. Do not include any introductory or concluding sentences outside of the report structure.";

        var messages = new ChatMessage[]
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(serviceInfo)
        };

        var completion = await client.CompleteChatAsync(messages);
        return completion.Value.Content[0].Text;
    }
}
