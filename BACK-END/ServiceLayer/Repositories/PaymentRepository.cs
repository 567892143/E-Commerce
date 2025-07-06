using Microsoft.EntityFrameworkCore;
using ServiceLayer.dbContext;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Shared.Services;

namespace ServiceLayer.Repository
{
    public class PaymentRepository : RepositoryBase<Models.Payment>, IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

       

        public Models.Payment? GetByOrderId(Guid orderId)
        {
            return _context.Payments
                .AsNoTracking()
                .FirstOrDefault(p => p.OrderId == orderId);
        }

      
    }
}
