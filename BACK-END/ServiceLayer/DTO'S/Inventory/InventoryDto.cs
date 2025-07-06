
namespace ServiceLayer.Inventory.Dto;
public class InventoryDto
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateInventoryDto
{
    public Guid VariantId { get; set; }
    public Guid WarehouseId { get; set; }
    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }
}

public class UpdateInventoryDto
{
    public int Quantity { get; set; }
    public int ReorderLevel { get; set; }
}

