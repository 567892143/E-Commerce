using ServiceLayer.Models;
using ServiceLayer.Shared.Interfaces;

namespace ServiceLayer.Interface.Reposiory
{
    public interface IWishlistRepository : IRepositoryBase<Wishlist>
    {
        List<Wishlist> GetWishlistByUserId(Guid userId);
        Wishlist? FindByUserAndProduct(Guid userId, Guid productId);
        bool Exists(Guid userId, Guid productId);
    }
}
