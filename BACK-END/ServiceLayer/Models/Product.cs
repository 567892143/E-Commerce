using System;
using System.Collections.Generic;

namespace ServiceLayer.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public Guid CategoryId { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation Properties
        public Category? Category { get; set; }
        public ICollection<ProductVariant>? Variants { get; set; }
        public ICollection<Models.ProductImage>? Images { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
