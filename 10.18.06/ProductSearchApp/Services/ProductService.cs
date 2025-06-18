using System.Text.Json;
using ProductSearchApp.Models;

namespace ProductSearchApp.Services
{
    public class ProductService : IProductService
    {
        private const string ProductsFileName = "products.json";

        public async Task<List<Product>> LoadProductsAsync()
        {
            try
            {
                if (!File.Exists(ProductsFileName))
                {
                    throw new FileNotFoundException($"Products file '{ProductsFileName}' not found.");
                }

                var json = await File.ReadAllTextAsync(ProductsFileName);
                var products = JsonSerializer.Deserialize<List<Product>>(json);
                
                return products ?? new List<Product>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load products: {ex.Message}", ex);
            }
        }

        public List<Product> FilterProducts(List<Product> products, SearchCriteria criteria)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            var filtered = products.AsEnumerable();

            filtered = ApplyCategoryFilter(filtered, criteria.Category);
            filtered = ApplyPriceFilters(filtered, criteria.MinPrice, criteria.MaxPrice);
            filtered = ApplyRatingFilter(filtered, criteria.MinRating);
            filtered = ApplyStockFilter(filtered, criteria.InStockOnly);
            filtered = ApplySearchTermFilter(filtered, criteria.SearchTerm);

            return filtered.ToList();
        }

        private IEnumerable<Product> ApplyCategoryFilter(IEnumerable<Product> products, string? category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return products;

            return products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<Product> ApplyPriceFilters(IEnumerable<Product> products, decimal? minPrice, decimal? maxPrice)
        {
            if (minPrice.HasValue)
                products = products.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                products = products.Where(p => p.Price <= maxPrice.Value);

            return products;
        }

        private IEnumerable<Product> ApplyRatingFilter(IEnumerable<Product> products, double? minRating)
        {
            if (minRating.HasValue)
                products = products.Where(p => p.Rating >= minRating.Value);

            return products;
        }

        private IEnumerable<Product> ApplyStockFilter(IEnumerable<Product> products, bool? inStockOnly)
        {
            if (inStockOnly == true)
                products = products.Where(p => p.InStock);

            return products;
        }

        private IEnumerable<Product> ApplySearchTermFilter(IEnumerable<Product> products, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return products;

            return products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }
} 