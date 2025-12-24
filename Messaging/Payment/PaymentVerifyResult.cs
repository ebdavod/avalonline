namespace AvvalOnline.Shop.Api.Messaging.Payment
{
    public class PaymentVerifyResult
    {
        public bool IsSuccess { get; set; }
        public string RefId { get; set; } // ReferenceNumber
        public string Message { get; set; }
    }
}
