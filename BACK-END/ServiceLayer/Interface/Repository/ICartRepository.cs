using ServiceLayer.Models;
using ServiceLayer.Shared.Interfaces;

namespace ServiceLayer.Interface.Reposiory
{
    public interface ICartRepository : IRepositoryBase<Models.Cart>
    {
        List<Models.Cart> GetCartByUserId(Guid userId);
        Models.Cart? FindByUserAndVariant(Guid userId, Guid variantId);
    }
}
