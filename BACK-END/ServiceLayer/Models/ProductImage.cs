using System;

namespace ServiceLayer.Models
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid? VariantId { get; set; }
        public string Url { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Product? Product { get; set; }
        public ProductVariant? Variant { get; set; }
    }
}
