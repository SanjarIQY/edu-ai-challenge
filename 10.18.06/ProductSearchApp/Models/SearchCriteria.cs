namespace ProductSearchApp.Models
{
    public class SearchCriteria
    {
        public string? Category { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public double? MinRating { get; set; }
        public bool? InStockOnly { get; set; }
        public string? SearchTerm { get; set; }
    }
} 