using ServiceLayer.Models;

public class OrderSummaryDto
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } 
    public DateTime CreatedAt { get; set; }
}
