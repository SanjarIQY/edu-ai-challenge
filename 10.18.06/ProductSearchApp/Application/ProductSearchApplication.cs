using ProductSearchApp.Models;
using ProductSearchApp.Services;

namespace ProductSearchApp.Application
{
    public class ProductSearchApplication
    {
        private readonly IProductService _productService;
        private readonly IOpenAIService _openAIService;
        private readonly IDisplayService _displayService;

        public ProductSearchApplication(
            IProductService productService,
            IOpenAIService openAIService,
            IDisplayService displayService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _openAIService = openAIService ?? throw new ArgumentNullException(nameof(openAIService));
            _displayService = displayService ?? throw new ArgumentNullException(nameof(displayService));
        }

        public async Task RunAsync()
        {
            try
            {
                _displayService.ShowWelcome();

                var products = await LoadProductsAsync();
                if (products.Count == 0)
                {
                    _displayService.ShowError("No products available.");
                    return;
                }

                _displayService.ShowLoadedProductsCount(products.Count);

                await RunSearchLoopAsync(products);

                _displayService.ShowGoodbye();
            }
            catch (Exception ex)
            {
                _displayService.ShowError($"Application error: {ex.Message}");
            }
        }

        private async Task<List<Product>> LoadProductsAsync()
        {
            try
            {
                return await _productService.LoadProductsAsync();
            }
            catch (Exception ex)
            {
                _displayService.ShowError($"Failed to load products: {ex.Message}");
                return new List<Product>();
            }
        }

        private async Task RunSearchLoopAsync(List<Product> products)
        {
            while (true)
            {
                _displayService.ShowSearchPrompt();
                var userInput = _displayService.GetUserInput();

                if (ShouldExit(userInput))
                    break;

                if (string.IsNullOrWhiteSpace(userInput))
                    continue;

                await ProcessSearchRequestAsync(userInput, products);
            }
        }

        private bool ShouldExit(string? input)
        {
            return string.IsNullOrWhiteSpace(input) || 
                   input.Equals("quit", StringComparison.OrdinalIgnoreCase) ||
                   input.Equals("exit", StringComparison.OrdinalIgnoreCase);
        }

        private async Task ProcessSearchRequestAsync(string userQuery, List<Product> products)
        {
            try
            {
                var searchCriteria = await _openAIService.ParseSearchQueryAsync(userQuery, products);
                
                if (searchCriteria == null)
                {
                    _displayService.ShowError("I couldn't understand your search criteria. Please try rephrasing your request.");
                    return;
                }

                var filteredProducts = _productService.FilterProducts(products, searchCriteria);
                _displayService.ShowSearchResults(filteredProducts, userQuery);
            }
            catch (Exception ex)
            {
                _displayService.ShowError($"Search failed: {ex.Message}");
            }
        }
    }
} 