using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class ShippingHistory
    {
        public int Id { get; set; }

        [Required]
        public int ShippingId { get; set; }

        [Required]
        public ShippingStatus Status { get; set; }

        public string Location { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("ShippingId")]
        public Shipping Shipping { get; set; }
    }

    public enum ShippingMethod
    {
        Standard = 1,   // عادی
        Express = 2,    // سریع
        InPerson = 3    // حضوری
    }

    public enum ShippingStatus
    {
        Pending = 1,            // در انتظار
        Processing = 2,         // در حال آماده‌سازی
        Shipped = 3,            // ارسال شده
        InTransit = 4,          // در مسیر
        OutForDelivery = 5,     // در حال تحویل
        Delivered = 6,          // تحویل داده شده
        Failed = 7,             // ناموفق
        Returned = 8            // مرجوعی
    }
}


namespace AvvalOnline.Shop.Api.Model.Entites
{


}
