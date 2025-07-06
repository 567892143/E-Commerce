public class CreateOrderDto
{
    public List<OrderItemDto> Items { get; set; } = new();
    public Guid ShippingContactId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid? DiscountCodeId { get; set; }  // optional
}

public class OrderItemDto
{
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
}