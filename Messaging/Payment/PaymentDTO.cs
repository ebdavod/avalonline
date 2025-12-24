using AvvalOnline.Shop.Api.Model.Entites;

namespace AvvalOnline.Shop.Api.Messaging.Payment
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; } // کد رهگیری/Authority
        public string ReferenceNumber { get; set; } // شماره پیگیری بانکی
        public string GatewayName { get; set; }
        public PaymentStatus Status { get; set; } // وضعیت پرداخت
        public DateTime PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
