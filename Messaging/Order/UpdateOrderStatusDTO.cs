using AvvalOnline.Shop.Api.Model.Entites;

namespace AvvalOnline.Shop.Api.Messaging.Order
{
    public class UpdateOrderStatusDTO
    {
        public int Int { get; set; }
        public OrderStatus Status { get; set; }
    }

}

