using AvvalOnline.Shop.Api.Attributes;
using AvvalOnline.Shop.Api.Messaging.Order;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[action]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    [CustomAuthorize(["Admin"])] // فقط ادمین می‌تواند سفارش بسازد
    public async Task<CreateOrderRes> Create(CreateOrderReq req)
    {
        return await _orderService.CreateOrderAsync(req);
    }

    [HttpPost]
    [CustomAuthorize(["Admin"])]
    public async Task<CancelOrderRes> Cancel(CancelOrderReq req)
    {
        return await _orderService.CancelOrderAsync(req);
    }

    [HttpPost]
    [CustomAuthorize(["Admin"])]
    public async Task<UpdateOrderRes> Update(UpdateOrderReq req)
    {
        return await _orderService.UpdateOrderAsync(req);
    }

    [HttpPost]
    [CustomAuthorize(["Admin"])]
    public async Task<UpdateOrderStatusRes> UpdateStatus(UpdateOrderStatusReq req)
    {
        return await _orderService.UpdateOrderStatusAsync(req);
    }

    [HttpGet]
    [CustomAuthorize(["Admin", "User"])] // ادمین یا خود کاربر می‌تواند سفارش را ببیند
    public async Task<GetOrderByIdRes> GetById(int id)
    {
        return await _orderService.GetOrderByIdAsync(new GetOrderByIdReq { Id = id });
    }

    [HttpGet]
    [CustomAuthorize(["Admin"])]
    public async Task<GetAllOrdersRes> GetAll(int index, int size)
    {
        return await _orderService.GetAllOrdersAsync(new GetAllOrdersReq { Page = index, PageSize = size });
    }

    [HttpGet]
    [CustomAuthorize(["Admin", "User"])]
    public async Task<GetOrdersByUserRes> GetByUser(int userId)
    {
        return await _orderService.GetOrdersByUserAsync(new GetOrdersByUserReq { Id = userId });
    }
}
