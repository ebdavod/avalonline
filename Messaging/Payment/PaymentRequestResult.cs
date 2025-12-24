namespace AvvalOnline.Shop.Api.Messaging.Payment
{
    public class PaymentRequestResult
    {
        public bool IsSuccess { get; set; }
        public string PaymentUrl { get; set; }
        public string Authority { get; set; } // یا TransactionId
        public string Message { get; set; }
    }
}
