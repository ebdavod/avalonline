using AvvalOnline.Shop.Api.Messaging.Payment;

namespace AvvalOnline.Shop.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreatePaymentRes> CreatePaymentAsync(CreatePaymentReq req, string gatewayName, string callbackUrl);
        Task<VerifyPaymentRes> VerifyPaymentAsync(VerifyPaymentReq req, string gatewayName);
        Task<GetPaymentByIdRes> GetPaymentByIdAsync(GetPaymentByIdReq req);
        Task<GetPaymentsByOrderRes> GetPaymentsByOrderAsync(GetPaymentsByOrderReq req);
    }
}
