using ServiceLayer.Cart.Dto;
using ServiceLayer.Product.Dto;

namespace ServiceLayer.Interface
{
    public interface ICartService
    {
        // Cart methods
        List<CartItemDto> GetCartByUserId(Guid userId);
        void AddToCart(Guid userId, AddToCartDto dto);
        void RemoveFromCart(Guid userId, Guid variantId);

        // Wishlist methods
        List<WishlistItemDto> GetWishlistByUserId(Guid userId);
        void AddToWishlist(Guid userId, Guid productId);
        void RemoveFromWishlist(Guid userId, Guid productId);
    }
}
