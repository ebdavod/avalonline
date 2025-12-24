using AvvalOnline.Shop.Api.Messaging.Payment;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvvalOnline.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// ایجاد درخواست پرداخت
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentReq req, [FromQuery] string gatewayName)
        {
            var callbackUrl = $"{Request.Scheme}://{Request.Host}/api/payment/verify/{gatewayName}";
            var result = await _paymentService.CreatePaymentAsync(req, gatewayName, callbackUrl);

            if (!result.IsSuccess)
                return BadRequest(result);

            // برگرداندن URL پرداخت به کاربر
            return Ok(result);
        }

        /// <summary>
        /// تایید پرداخت بعد از بازگشت از درگاه
        /// </summary>
        [HttpGet("verify/{gatewayName}")]
        public async Task<IActionResult> VerifyPayment([FromQuery] string transactionId, [FromQuery] int orderId, string gatewayName)
        {
            var req = new VerifyPaymentReq
            {
                Entity = new PaymentDTO
                {
                    OrderId = orderId,
                    TransactionId = transactionId
                }
            };

            var result = await _paymentService.VerifyPaymentAsync(req, gatewayName);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// گرفتن وضعیت یک پرداخت خاص
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var req = new GetPaymentByIdReq { Id = id };
            var result = await _paymentService.GetPaymentByIdAsync(req);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// گرفتن همه پرداخت‌های یک سفارش
        /// </summary>
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrder(int orderId)
        {
            var req = new GetPaymentsByOrderReq { Id = orderId };
            var result = await _paymentService.GetPaymentsByOrderAsync(req);

            return Ok(result);
        }
    }
}
