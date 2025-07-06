using ServiceLayer.Models;
using ServiceLayer.Shared.Interfaces;

namespace ServiceLayer.Interface.Reposiory
{
    public interface IShippingRepository : IRepositoryBase<Models.Shipping>
    {
        Models.Shipping? GetByOrderId(Guid orderId);
       
    }
}
