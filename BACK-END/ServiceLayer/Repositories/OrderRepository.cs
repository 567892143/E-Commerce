using Microsoft.EntityFrameworkCore;
using ServiceLayer.dbContext;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Shared.Services;

namespace ServiceLayer.Repository
{
    public class OrderRepository : RepositoryBase<Models.Order>, IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        

        public List<Models.Order> GetOrdersByUserId(Guid userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();
        }

        public Models.Order? GetOrderWithItems(Guid orderId, Guid userId)
        {
            return _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Variant)
                        .ThenInclude(v => v.Product)
                .FirstOrDefault(o => o.Id == orderId && o.UserId == userId);
        }

        public decimal GetVariantPrice(Guid variantId)
        {
            var variant = _context.ProductVariants.FirstOrDefault(v => v.Id == variantId);
            return variant?.Price ?? 0;
        }
    }
}
