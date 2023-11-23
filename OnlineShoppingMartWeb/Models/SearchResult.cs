namespace OnlineShoppingMartWeb.Models
{
    public class SearchResult
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }

        public string ProductType { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductColor { get; set; }
        public Dictionary<string, List<string>> ProductImages { get; set; }
    }
}