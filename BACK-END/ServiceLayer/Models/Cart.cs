using System;

namespace ServiceLayer.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid VariantId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ProductVariant? Variant { get; set; }
    }
}
