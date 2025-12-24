using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }
        public decimal TotalPrice => (UnitPrice - Discount) * Quantity;

        // برای محصولات خاص مثل لاستیک
        public string SelectedSize { get; set; }
        public string InstallationType { get; set; } // نصب، تعویض، etc

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}