using ServiceLayer.Models;
using ServiceLayer.Shared.Interfaces;

namespace ServiceLayer.Interface.Reposiory
{
    public interface IPaymentRepository : IRepositoryBase<Models.Payment>
    {
      
        Models.Payment? GetByOrderId(Guid orderId);
       
    }
}
