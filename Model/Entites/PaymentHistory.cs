using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class PaymentHistory
    {
        public int Id { get; set; }

        [Required]
        public int PaymentId { get; set; }

        [Required]
        public PaymentStatus OldStatus { get; set; }

        [Required]
        public PaymentStatus NewStatus { get; set; }

        public string Description { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public int? ChangedByUserId { get; set; }

        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }
    }

    public enum PaymentMethod
    {
        Online = 1,
        CashOnDelivery = 2,
        BankTransfer = 3,
        Wallet = 4
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Processing = 1,
        Success = 2,
        Failed = 3,
        Cancelled = 4,
        Refunded = 5
    }
}