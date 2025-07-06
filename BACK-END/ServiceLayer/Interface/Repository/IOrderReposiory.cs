using ServiceLayer.Models;
using ServiceLayer.Shared.Interfaces;

namespace ServiceLayer.Interface.Reposiory
{
    public interface IOrderRepository : IRepositoryBase<Models.Order>
    {
        List<Models.Order> GetOrdersByUserId(Guid userId);
        Models.Order? GetOrderWithItems(Guid orderId, Guid userId);
        decimal GetVariantPrice(Guid variantId);
    }
}
