using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public ProdcutCategory Category { get; set; }

        public bool IsAvailable { get; set; }

        // تصاویر
        public List<ProductImage> Images { get; set; } = new List<ProductImage>();

        // سفارشات
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}

