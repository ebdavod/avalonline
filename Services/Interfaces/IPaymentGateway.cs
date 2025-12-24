using AvvalOnline.Shop.Api.Messaging.Payment;
using AvvalOnline.Shop.Api.Model.Entites;

namespace AvvalOnline.Shop.Api.Services.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentRequestResult> CreatePaymentAsync(Order order, decimal amount, string callbackUrl);
        Task<PaymentVerifyResult> VerifyPaymentAsync(string authority, decimal amount);
    }
}
