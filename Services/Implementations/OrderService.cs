using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.Order;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Model.Entites;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AvvalOnline.Shop.Api.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ShopDbContext _db;

        public OrderService(ShopDbContext db)
        {
            _db = db;
        }

        public async Task<CreateOrderRes> CreateOrderAsync(CreateOrderReq req)
        {
            var dto = req.Entity;
            if (dto == null || dto.Items == null || !dto.Items.Any())
                return new CreateOrderRes { IsSuccess = false, Message = "آیتم‌های سفارش نامعتبر هستند" };

            if (req.Entity.VehicleId is not null)
            {
                var vehicle = await _db.Vehicles.FindAsync(req.Entity.VehicleId);
                if (vehicle is null)
                    return new CreateOrderRes { IsSuccess = false, Message = "وسیله مورد نظر یافت نشد" };
            }

            var order = new Order
            {
                Code = Guid.NewGuid().ToString().Substring(0, 8),
                UserId = dto.UserId,
                VehicleId = dto.VehicleId,
                Status = OrderStatus.Pending,
                ShippingAddress = dto.ShippingAddress,
                ShippingCity = dto.ShippingCity,
                OrderDate = DateTime.UtcNow,
                TotalAmount = dto.Items.Sum(i => i.Quantity * i.UnitPrice - i.Discount),
                DiscountAmount = dto.DiscountAmount,
                FinalAmount = dto.Items.Sum(i => i.Quantity * i.UnitPrice - i.Discount) - dto.DiscountAmount,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount
                    // TotalPrice محاسبه میشه در Entity
                }).ToList()
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            dto.Id = order.Id;
            dto.Code = order.Code;
            dto.Status = order.Status;
            dto.OrderDate = order.OrderDate;
            dto.FinalAmount = order.FinalAmount;

            return new CreateOrderRes { IsSuccess = true, Entity = dto };
        }

        public async Task<CancelOrderRes> CancelOrderAsync(CancelOrderReq req)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == req.Id);
            if (order == null)
                return new CancelOrderRes { IsSuccess = false, Message = "سفارش یافت نشد" };

            order.Status = OrderStatus.Cancelled;
            await _db.SaveChangesAsync();

            return new CancelOrderRes { IsSuccess = true, Message = "سفارش لغو شد" };
        }

        public async Task<UpdateOrderStatusRes> UpdateOrderStatusAsync(UpdateOrderStatusReq req)
        {
            var dto = req.Entity;
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == dto.Int);
            if (order == null)
                return new UpdateOrderStatusRes { IsSuccess = false, Message = "سفارش یافت نشد" };

            order.Status = dto.Status;
            await _db.SaveChangesAsync();

            return new UpdateOrderStatusRes { IsSuccess = true, Message = "وضعیت سفارش تغییر کرد" };
        }

        public async Task<GetOrderByIdRes> GetOrderByIdAsync(GetOrderByIdReq req)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .Include(o => o.Vehicle)
                .FirstOrDefaultAsync(o => o.Id == req.Id);

            if (order == null)
                return new GetOrderByIdRes { IsSuccess = false, Message = "سفارش یافت نشد" };

            var dto = new OrderDTO
            {
                Id = order.Id,
                Code = order.Code,
                UserId = order.UserId,
                UserName = order.User?.Username,
                VehicleId = order.VehicleId,
                VehicleModel = order.Vehicle?.Model,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                DiscountAmount = order.DiscountAmount,
                FinalAmount = order.FinalAmount,
                ShippingAddress = order.ShippingAddress,
                ShippingCity = order.ShippingCity,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Items = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            };

            return new GetOrderByIdRes { IsSuccess = true, Entity = dto };
        }

        public async Task<GetAllOrdersRes> GetAllOrdersAsync(GetAllOrdersReq req)
        {
            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            var dtos = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                Code = order.Code,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                DiscountAmount = order.DiscountAmount,
                FinalAmount = order.FinalAmount,
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            }).ToList();

            return new GetAllOrdersRes { IsSuccess = true, Entities = dtos, TotalCount = await _db.Orders.CountAsync() };
        }

        public async Task<GetOrdersByUserRes> GetOrdersByUserAsync(GetOrdersByUserReq req)
        {
            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == req.Id)
                .ToListAsync();

            var dtos = orders.Select(order => new OrderDTO
            {
                Id = order.Id,
                Code = order.Code,
                UserId = order.UserId,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                DiscountAmount = order.DiscountAmount,
                FinalAmount = order.FinalAmount,
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Discount = oi.Discount,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            }).ToList();

            return new GetOrdersByUserRes { IsSuccess = true, Entities = dtos };
        }

        public async Task<UpdateOrderRes> UpdateOrderAsync(UpdateOrderReq req)
        {
            var dto = req.Entity;
            var order = await _db.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == dto.Id);
            if (order == null)
                return new UpdateOrderRes { IsSuccess = false, Message = "سفارش یافت نشد" };

            order.ShippingAddress = dto.ShippingAddress;
            order.ShippingCity = dto.ShippingCity;
            order.VehicleId = dto.VehicleId;
            order.DiscountAmount = dto.DiscountAmount;
            order.FinalAmount = dto.Items.Sum(i => i.TotalPrice) - dto.DiscountAmount;

            // بروزرسانی آیتم‌ها
            order.OrderItems.Clear();
            foreach (var i in dto.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                });
            }

            await _db.SaveChangesAsync();
            return new UpdateOrderRes { IsSuccess = true, Message = "سفارش بروزرسانی شد" };
        }
    }
}
