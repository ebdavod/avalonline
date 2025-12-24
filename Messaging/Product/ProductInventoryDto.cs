namespace AvvalOnline.Shop.Api.Messaging.Product
{
    public class ProductInventoryDto
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public int StockQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int AvailableQuantity => StockQuantity - ReservedQuantity;
        public bool IsLowStock { get; set; }
    }
}
