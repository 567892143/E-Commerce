
using ServiceLayer.Models;

namespace ServiceLayer.Order.Dto;
public class OrderDetailsDto
{
    public Guid Id { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } 
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDetailsDto> Items { get; set; } = new();
}

public class OrderItemDetailsDto
{
    public Guid VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; } 
}

