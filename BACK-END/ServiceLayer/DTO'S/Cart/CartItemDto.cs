
namespace ServiceLayer.Cart.Dto;
public class CartItemDto
{
    public Guid VariantId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}

public class AddToCartDto
{
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
}

public class WishlistItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}
