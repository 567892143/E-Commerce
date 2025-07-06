using ServiceLayer.dbContext;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Shared.Services;

namespace ServiceLayer.Repository
{
    public class WishlistRepository : RepositoryBase<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(AppDbContext context) : base(context) { }

        public List<Wishlist> GetWishlistByUserId(Guid userId)
        {
            return _context.Wishlists
                .Include(w => w.Product)
                    .ThenInclude(p => p.Images)
                .Where(w => w.UserId == userId)
                .ToList();
        }

        public Wishlist? FindByUserAndProduct(Guid userId, Guid productId)
        {
            return _context.Wishlists
                .FirstOrDefault(w => w.UserId == userId && w.ProductId == productId);
        }

        public bool Exists(Guid userId, Guid productId)
        {
            return _context.Wishlists
                .Any(w => w.UserId == userId && w.ProductId == productId);
        }
    }
}
