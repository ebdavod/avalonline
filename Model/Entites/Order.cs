using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } // کد سفارش

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int? VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }

        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    public enum OrderStatus
    {
        Pending = 0,     // سفارش ثبت شده ولی پرداخت نشده
        Processing = 1,  // پرداخت موفق و در حال آماده‌سازی
        Paid = 2,        // پرداخت کامل شده
        Cancelled = 3,   // لغو شده
        Delivered = 4    // تحویل داده شده
    }
}