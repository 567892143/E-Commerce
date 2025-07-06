using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Cart.Dto;
using ServiceLayer.Interface;

[ApiController]
[Route("api/v1")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
        
    }

    // ----------- CART ------------

    [HttpGet("cart")]
    public ActionResult<List<CartItemDto>> GetCart()
    {
        var userId = GetUserId();
        var cart = _cartService.GetCartByUserId(userId);
        return Ok(cart);
    }

    [HttpPost("cart")]
    public IActionResult AddToCart([FromBody] AddToCartDto dto)
    {
        var userId = GetUserId();
        _cartService.AddToCart(userId, dto);
        return Ok();
    }

    [HttpDelete("cart/{variantId}")]
    public IActionResult RemoveFromCart(Guid variantId)
    {
        var userId = GetUserId();
        _cartService.RemoveFromCart(userId, variantId);
        return NoContent();
    }

    // ----------- WISHLIST ------------

    [HttpGet("wishlist")]
    public ActionResult<List<WishlistItemDto>> GetWishlist()
    {
        var userId = GetUserId();
        var wishlist = _cartService.GetWishlistByUserId(userId);
        return Ok(wishlist);
    }

    [HttpPost("wishlist/{productId}")]
    public IActionResult AddToWishlist(Guid productId)
    {
        var userId = GetUserId();
        _cartService.AddToWishlist(userId, productId);
        return Ok();
    }

    [HttpDelete("wishlist/{productId}")]
    public IActionResult RemoveFromWishlist(Guid productId)
    {
        var userId = GetUserId();
        _cartService.RemoveFromWishlist(userId, productId);
        return NoContent();
    }

    private Guid GetUserId()
    {
        // JWT logic - adjust based on your auth scheme
        return Guid.Parse(User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException("User ID not found"));
    }
}
