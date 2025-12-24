using AvvalOnline.Shop.Api.Messaging.Payment;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Model.Entites;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AvvalOnline.Shop.Api.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ShopDbContext _db;
        private readonly Dictionary<string, IPaymentGateway> _gateways;

        public PaymentService(ShopDbContext db, IEnumerable<IPaymentGateway> gateways)
        {
            _db = db;
            _gateways = gateways.ToDictionary(
                g => g.GetType().Name.Replace("Gateway", ""),
                g => g
            );
        }

        /// <summary>
        /// ایجاد درخواست پرداخت
        /// </summary>
        public async Task<CreatePaymentRes> CreatePaymentAsync(CreatePaymentReq req, string gatewayName, string callbackUrl)
        {
            if (!_gateways.ContainsKey(gatewayName))
                return new CreatePaymentRes { IsSuccess = false, Message = "درگاه نامعتبر است" };

            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == req.Entity.OrderId);
            if (order == null)
                return new CreatePaymentRes { IsSuccess = false, Message = "سفارش یافت نشد" };

            var gateway = _gateways[gatewayName];
            var result = await gateway.CreatePaymentAsync(order, order.FinalAmount, callbackUrl);

            if (!result.IsSuccess)
                return new CreatePaymentRes { IsSuccess = false, Message = result.Message };

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.FinalAmount,
                Method = PaymentMethod.Online,
                Status = PaymentStatus.Pending,
                GatewayName = gatewayName,
                TransactionId = result.Authority,
                PaymentDate = DateTime.UtcNow
            };

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            return new CreatePaymentRes
            {
                IsSuccess = true,
                Message = result.Message,
                Entity = new PaymentDTO
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    GatewayName = payment.GatewayName,
                    TransactionId = payment.TransactionId,
                    Amount = payment.Amount,
                    Status = payment.Status,
                    CreatedAt = payment.CreatedAt
                }
            };
        }

        /// <summary>
        /// تایید پرداخت
        /// </summary>
        public async Task<VerifyPaymentRes> VerifyPaymentAsync(VerifyPaymentReq req, string gatewayName)
        {
            if (!_gateways.ContainsKey(gatewayName))
                return new VerifyPaymentRes { IsSuccess = false, Message = "درگاه نامعتبر است" };

            var payment = await _db.Payments.FirstOrDefaultAsync(p => p.TransactionId == req.Entity.TransactionId);
            if (payment == null)
                return new VerifyPaymentRes { IsSuccess = false, Message = "پرداخت یافت نشد" };

            var gateway = _gateways[gatewayName];
            var result = await gateway.VerifyPaymentAsync(payment.TransactionId, payment.Amount);

            // ثبت تاریخچه تغییر وضعیت
            var history = new PaymentHistory
            {
                PaymentId = payment.Id,
                OldStatus = payment.Status,
                NewStatus = result.IsSuccess ? PaymentStatus.Success : PaymentStatus.Failed,
                Description = result.Message,
                ChangedAt = DateTime.UtcNow
            };
            _db.PaymentHistories.Add(history);

            payment.Status = history.NewStatus;
            payment.ReferenceNumber = result.RefId;
            payment.UpdatedAt = DateTime.UtcNow;

            // 🔑 تغییر وضعیت سفارش
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == payment.OrderId);
            if (order != null)
            {
                if (result.IsSuccess)
                {
                    order.Status = OrderStatus.Processing; // یا Paid
                    order.DeliveryDate = DateTime.UtcNow.AddDays(3); // مثال: تحویل 3 روز بعد

                    // مدیریت تخفیف
                    if (order.DiscountAmount > 0)
                    {
                        var discount = await _db.Discounts
                            .FirstOrDefaultAsync(d => d.Orders.Any(o => o.Id == order.Id));

                        if (discount != null)
                        {
                            discount.UsedCount++;
                            if (discount.UsedCount >= discount.UsageLimit)
                                discount.IsActive = false;
                        }
                    }
                }
                else
                {
                    order.Status = OrderStatus.Cancelled;
                }
            }

            await _db.SaveChangesAsync();

            return new VerifyPaymentRes
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Entity = new PaymentDTO
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    GatewayName = payment.GatewayName,
                    TransactionId = payment.TransactionId,
                    ReferenceNumber = payment.ReferenceNumber,
                    Amount = payment.Amount,
                    Status = payment.Status,
                    CreatedAt = payment.CreatedAt,
                    UpdatedAt = payment.UpdatedAt
                }
            };
        }


        public async Task<GetPaymentByIdRes> GetPaymentByIdAsync(GetPaymentByIdReq req)
        {
            var payment = await _db.Payments.Include(p => p.Order).FirstOrDefaultAsync(p => p.Id == req.Id);
            if (payment == null)
                return new GetPaymentByIdRes { IsSuccess = false, Message = "پرداخت یافت نشد" };

            return new GetPaymentByIdRes
            {
                IsSuccess = true,
                Entity = new PaymentDTO
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    GatewayName = payment.GatewayName,
                    TransactionId = payment.TransactionId,
                    ReferenceNumber = payment.ReferenceNumber,
                    Amount = payment.Amount,
                    Status = payment.Status,
                    CreatedAt = payment.CreatedAt,
                    UpdatedAt = payment.UpdatedAt
                }
            };
        }

        public async Task<GetPaymentsByOrderRes> GetPaymentsByOrderAsync(GetPaymentsByOrderReq req)
        {
            var payments = await _db.Payments.Where(p => p.OrderId == req.Id).ToListAsync();

            return new GetPaymentsByOrderRes
            {
                IsSuccess = true,
                Entities = payments.Select(p => new PaymentDTO
                {
                    Id = p.Id,
                    OrderId = p.OrderId,
                    GatewayName = p.GatewayName,
                    TransactionId = p.TransactionId,
                    ReferenceNumber = p.ReferenceNumber,
                    Amount = p.Amount,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList()
            };
        }
    }
}
