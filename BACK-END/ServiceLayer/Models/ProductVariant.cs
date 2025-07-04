using System;

namespace ServiceLayer.Models
{
    public class ProductVariant
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Sku { get; set; } = null!;
        public string Size { get; set; } = null!;
        public string Color { get; set; } = null!;
        public decimal Price { get; set; }
        public string Barcode { get; set; } = null!;
        public decimal Weight { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Product? Product { get; set; }
        public Inventory? Inventory { get; set; }
    }
}
