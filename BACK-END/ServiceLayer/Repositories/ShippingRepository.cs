using Microsoft.EntityFrameworkCore;
using ServiceLayer.dbContext;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Shared.Services;

namespace ServiceLayer.Repository
{
    public class ShippingRepository : RepositoryBase<Models.Shipping>, IShippingRepository
    {
        private readonly AppDbContext _context;

        public ShippingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Models.Shipping? GetByOrderId(Guid orderId)
        {
            return _context.Shippings
                .AsNoTracking()
                .FirstOrDefault(s => s.OrderId == orderId);
        }

     
    }
}
