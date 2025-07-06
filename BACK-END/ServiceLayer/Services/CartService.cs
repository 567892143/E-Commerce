using ServiceLayer.Interface;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Product.Dto;
using Shared.Services.Exceptions;
using Microsoft.Extensions.Logging;
using ServiceLayer.Cart.Dto;

namespace ServiceLayer.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IProductRepository _productRepo;
        private readonly ILogger<CartService> _logger;

        public CartService(
            ICartRepository cartRepo,
            IWishlistRepository wishlistRepo,
            IProductRepository productRepo,
            ILogger<CartService> logger)
        {
            _cartRepo = cartRepo;
            _wishlistRepo = wishlistRepo;
            _productRepo = productRepo;
            _logger = logger;
        }

        // -------------------- CART ----------------------

        public List<CartItemDto> GetCartByUserId(Guid userId)
        {
            var items = _cartRepo.GetCartByUserId(userId);
            return items.Select(item => new CartItemDto
            {
                VariantId = item.VariantId,
                Quantity = item.Quantity,
                Price = item.Variant.Price,
                ProductName = item.Variant.Product.Name,
                Size = item.Variant.Size,
                Color = item.Variant.Color,
                ImageUrl = item.Variant.Product.Images.FirstOrDefault(i => i.IsPrimary)?.Url ?? ""
            }).ToList();
        }

        public void AddToCart(Guid userId, AddToCartDto dto)
        {
            var existing = _cartRepo.FindByUserAndVariant(userId, dto.VariantId);
            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                _cartRepo.Update(existing);
            }
            else
            {
                var newItem = new Models.Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    VariantId = dto.VariantId,
                    Quantity = dto.Quantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _cartRepo.Create(newItem);
            }
        }

        public void RemoveFromCart(Guid userId, Guid variantId)
        {
            var item = _cartRepo.FindByUserAndVariant(userId, variantId);
            if (item != null)
            {
                _cartRepo.Delete(item);
            }
        }

        // -------------------- WISHLIST ----------------------

        public List<WishlistItemDto> GetWishlistByUserId(Guid userId)
        {
            var items = _wishlistRepo.GetWishlistByUserId(userId);

            return items.Select(w => new WishlistItemDto
            {
                ProductId = w.ProductId,
                ProductName = w.Product.Name,
                ImageUrl = w.Product.Images.FirstOrDefault(i => i.IsPrimary)?.Url ?? ""
            }).ToList();
        }

        public void AddToWishlist(Guid userId, Guid productId)
        {
            if (_wishlistRepo.Exists(userId, productId)) return;

            var wish = new Wishlist
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProductId = productId,
                CreatedAt = DateTime.UtcNow
            };

            _wishlistRepo.Create(wish);
        }

        public void RemoveFromWishlist(Guid userId, Guid productId)
        {
            var item = _wishlistRepo.FindByUserAndProduct(userId, productId);
            if (item != null)
            {
                _wishlistRepo.Delete(item);
            }
        }
    }
}
