using ServiceLayer.Interface;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using ServiceLayer.Inventory.Dto;

namespace ServiceLayer.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepo;

        public InventoryService(IInventoryRepository inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;
        }

        public InventoryDto? GetInventoryByVariantId(Guid variantId)
        {
            var inv = _inventoryRepo.GetByVariantId(variantId);
            return inv == null ? null : new InventoryDto
            {
                Id = inv.Id,
                VariantId = inv.VariantId,
                WarehouseId = inv.WarehouseId,
                Quantity = inv.Quantity,
                ReorderLevel = inv.ReorderLevel,
                CreatedAt = inv.CreatedAt,
                UpdatedAt = inv.UpdatedAt
            };
        }

        public void CreateInventory(CreateInventoryDto dto)
        {
            var inventory = new Models.Inventory
            {
                Id = Guid.NewGuid(),
                VariantId = dto.VariantId,
                WarehouseId = dto.WarehouseId,
                Quantity = dto.Quantity,
                ReorderLevel = dto.ReorderLevel,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _inventoryRepo.Create(inventory);
        }

        public bool UpdateInventory(Guid id, UpdateInventoryDto dto)
        {
            var existing = _inventoryRepo.GetById(id);
            if (existing == null) return false;

            existing.Quantity = dto.Quantity;
            existing.ReorderLevel = dto.ReorderLevel;
            existing.UpdatedAt = DateTime.UtcNow;

            return _inventoryRepo.Update(existing);
        }
    }
}
