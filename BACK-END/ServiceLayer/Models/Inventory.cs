using System;

namespace ServiceLayer.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public Guid VariantId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ProductVariant? Variant { get; set; }
        public Warehouse? Warehouse { get; set; }
    }
}
