using ProductSearchApp.Models;

namespace ProductSearchApp.Services
{
    public interface IOpenAIService
    {
        Task<SearchCriteria?> ParseSearchQueryAsync(string userQuery, List<Product> availableProducts);
    }
} 