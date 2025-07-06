using ServiceLayer.Models;
using ServiceLayer.Shared.Interfaces;

namespace ServiceLayer.Interface.Reposiory
{
    public interface IInventoryRepository : IRepositoryBase<Models.Inventory>
    {
        Models.Inventory? GetByVariantId(Guid variantId);
        Models.Inventory? GetById(Guid id);
        void Create(Models.Inventory inventory);
        bool Update(Models.Inventory inventory);
    }
}
