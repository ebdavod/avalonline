using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class Shipping
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public ShippingMethod Method { get; set; }

        [Required]
        public ShippingStatus Status { get; set; } = ShippingStatus.Pending;

        public string TrackingNumber { get; set; }
        public string Carrier { get; set; } // پست، تیپاکس، etc

        public decimal ShippingCost { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }

        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }

        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public List<ShippingHistory> ShippingHistory { get; set; } = new List<ShippingHistory>();
    }


}