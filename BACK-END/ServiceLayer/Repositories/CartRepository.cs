using ServiceLayer.dbContext;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Shared.Services;

namespace ServiceLayer.Repository
{
    public class CartRepository : RepositoryBase<Models.Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context) { }

        public List<Models.Cart> GetCartByUserId(Guid userId)
        {
            return _context.Carts
                .Include(c => c.Variant)
                    .ThenInclude(v => v.Product)
                        .ThenInclude(p => p.Images)
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public Models.Cart? FindByUserAndVariant(Guid userId, Guid variantId)
        {
            return _context.Carts
                .FirstOrDefault(c => c.UserId == userId && c.VariantId == variantId);
        }
    }
}
