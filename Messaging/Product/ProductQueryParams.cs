namespace AvvalOnline.Shop.Api.Messaging.Product
{
    public class ProductQueryParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? CategoryId { get; set; }
        public bool? IsAvailable { get; set; }
        public string VehicleType { get; set; }
        public string Season { get; set; }
        public string Search { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Size { get; set; }
    }
}