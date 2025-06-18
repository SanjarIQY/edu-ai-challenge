using ProductSearchApp.Models;

namespace ProductSearchApp.Services
{
    public class ConsoleDisplayService : IDisplayService
    {
        private const string Separator = "--------------------------------------------------";

        public void ShowWelcome()
        {
            Console.WriteLine("=== Product Search Application ===");
            Console.WriteLine("This app uses OpenAI function calling to find products based on your preferences.");
            Console.WriteLine();
        }

        public void ShowLoadedProductsCount(int count)
        {
            Console.WriteLine($"Loaded {count} products from database.");
            Console.WriteLine();
        }

        public void ShowSearchPrompt()
        {
            Console.WriteLine("Enter your product search preferences (or 'quit' to exit):");
            Console.WriteLine("Examples:");
            Console.WriteLine("- 'I need a smartphone under $800'");
            Console.WriteLine("- 'Show me fitness products with rating above 4.5'");
            Console.WriteLine("- 'Find kitchen appliances under $100 that are in stock'");
            Console.WriteLine();
            Console.Write("Your request: ");
        }

        public void ShowSearchResults(List<Product> products, string originalQuery)
        {
            Console.WriteLine("\nSearching products...");
            Console.WriteLine($"Results for: \"{originalQuery}\"");
            Console.WriteLine();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found matching your criteria.");
            }
            else
            {
                Console.WriteLine($"Found {products.Count} product(s):");
                Console.WriteLine();

                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];
                    var stockStatus = product.InStock ? "In Stock" : "Out of Stock";
                    
                    Console.WriteLine($"{i + 1}. {product.Name} - ${product.Price:F2}, Rating: {product.Rating:F1}, {stockStatus}");
                    Console.WriteLine($"   Category: {product.Category}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine(Separator);
            Console.WriteLine();
        }

        public void ShowError(string message)
        {
            Console.WriteLine($"Error: {message}");
            Console.WriteLine();
        }

        public void ShowGoodbye()
        {
            Console.WriteLine("Thank you for using Product Search Application!");
        }

        public string? GetUserInput()
        {
            return Console.ReadLine();
        }
    }
} 