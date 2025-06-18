using ProductSearchApp.Models;

namespace ProductSearchApp.Services
{
    public interface IProductService
    {
        Task<List<Product>> LoadProductsAsync();
        List<Product> FilterProducts(List<Product> products, SearchCriteria criteria);
    }
} 