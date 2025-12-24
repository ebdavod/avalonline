using System.ComponentModel.DataAnnotations;

namespace AvvalOnline.Shop.Api.Messaging.Product
{
    public class ProductDTO
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryFullName { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsActive { get; set; }
        public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
        //public List<ProductInventoryDto> Inventory { get; set; } = new List<ProductInventoryDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
