using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using AvvalOnline.Shop.Api.Messaging.Payment;
using AvvalOnline.Shop.Api.Model.Entites;
using AvvalOnline.Shop.Api.Services.Interfaces;

namespace AvvalOnline.Shop.Api.Services.Implementations.Payments
{
    public class ZarinPalGateway : IPaymentGateway
    {
        private readonly HttpClient _httpClient;
        private readonly string _merchantId;
        private readonly bool _sandboxMode;

        public ZarinPalGateway(HttpClient httpClient, string merchantId, bool sandboxMode = false)
        {
            _httpClient = httpClient;
            _merchantId = merchantId;
            _sandboxMode = sandboxMode;
        }

        /// <summary>
        /// ایجاد درخواست پرداخت در زرین‌پال
        /// </summary>
        public async Task<PaymentRequestResult> CreatePaymentAsync(Order order, decimal amount, string callbackUrl)
        {
            var requestPayload = new
            {
                merchant_id = _merchantId,
                amount = (int)amount,
                callback_url = callbackUrl,
                description = $"پرداخت سفارش شماره {order.Code}",
                metadata = new { email = order.User?.Email, phone = order.User?.PhoneNumber }
            };

            var url = _sandboxMode
                ? "https://sandbox.zarinpal.com/pg/v4/payment/request.json"
                : "https://api.zarinpal.com/pg/v4/payment/request.json";

            var response = await _httpClient.PostAsJsonAsync(url, requestPayload);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var data = doc.RootElement.GetProperty("data");
            var errors = doc.RootElement.TryGetProperty("errors", out var err) ? err : default;

            if (response.IsSuccessStatusCode && data.TryGetProperty("authority", out var authority))
            {
                var authorityCode = authority.GetString();
                var paymentUrl = _sandboxMode
                    ? $"https://sandbox.zarinpal.com/pg/StartPay/{authorityCode}"
                    : $"https://www.zarinpal.com/pg/StartPay/{authorityCode}";

                return new PaymentRequestResult
                {
                    IsSuccess = true,
                    Authority = authorityCode,
                    PaymentUrl = paymentUrl,
                    Message = "درخواست پرداخت زرین‌پال موفق بود"
                };
            }

            return new PaymentRequestResult
            {
                IsSuccess = false,
                Message = $"خطا در ایجاد پرداخت: {errors}"
            };
        }

        /// <summary>
        /// تایید پرداخت در زرین‌پال
        /// </summary>
        public async Task<PaymentVerifyResult> VerifyPaymentAsync(string transactionId, decimal amount)
        {
            var requestPayload = new
            {
                merchant_id = _merchantId,
                amount = (int)amount,
                authority = transactionId
            };

            var url = _sandboxMode
                ? "https://sandbox.zarinpal.com/pg/v4/payment/verify.json"
                : "https://api.zarinpal.com/pg/v4/payment/verify.json";

            var response = await _httpClient.PostAsJsonAsync(url, requestPayload);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var data = doc.RootElement.GetProperty("data");
            var errors = doc.RootElement.TryGetProperty("errors", out var err) ? err : default;

            if (response.IsSuccessStatusCode && data.TryGetProperty("ref_id", out var refId))
            {
                return new PaymentVerifyResult
                {
                    IsSuccess = true,
                    RefId = refId.GetRawText(),
                    Message = "پرداخت تایید شد"
                };
            }

            return new PaymentVerifyResult
            {
                IsSuccess = false,
                Message = $"خطا در تایید پرداخت: {errors}"
            };
        }
    }
}
