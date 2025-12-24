using AvvalOnline.Shop.Api.Model.Entites;

public class OrderDTO
{
    public int Id { get; set; }
    public string Code { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int? VehicleId { get; set; }
    public string VehicleModel { get; set; }

    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }

    public string ShippingAddress { get; set; }
    public string ShippingCity { get; set; }

    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
}

public class OrderItemDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
}
