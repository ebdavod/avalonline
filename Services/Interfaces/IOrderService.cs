using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.Order;
using AvvalOnline.Shop.Api.Model.Entites;

public interface IOrderService
{
    Task<GetOrderByIdRes> GetOrderByIdAsync(GetOrderByIdReq req);
    Task<GetAllOrdersRes> GetAllOrdersAsync(GetAllOrdersReq req);
    Task<CreateOrderRes> CreateOrderAsync(CreateOrderReq req);
    Task<UpdateOrderRes> UpdateOrderAsync(UpdateOrderReq req);
    Task<CancelOrderRes> CancelOrderAsync(CancelOrderReq req);
    Task<UpdateOrderStatusRes> UpdateOrderStatusAsync(UpdateOrderStatusReq req);
    Task<GetOrdersByUserRes> GetOrdersByUserAsync(GetOrdersByUserReq req);
    //Task<OrderDto> GetOrderByCodeAsync(string code);
    //Task<List<OrderDto>> GetOrdersByVehicleAsync(int vehicleId);
    //Task<OrderStatisticsDto> GetOrderStatisticsAsync();
}
