using ProductSearchApp.Models;

namespace ProductSearchApp.Services
{
    public interface IDisplayService
    {
        void ShowWelcome();
        void ShowLoadedProductsCount(int count);
        void ShowSearchPrompt();
        void ShowSearchResults(List<Product> products, string originalQuery);
        void ShowError(string message);
        void ShowGoodbye();
        string? GetUserInput();
    }
} 