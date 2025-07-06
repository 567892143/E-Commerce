using ServiceLayer.Inventory.Dto;

namespace ServiceLayer.Interface
{
    public interface IInventoryService
    {
        InventoryDto? GetInventoryByVariantId(Guid variantId);
        void CreateInventory(CreateInventoryDto dto);
        bool UpdateInventory(Guid id, UpdateInventoryDto dto);
    }
}
