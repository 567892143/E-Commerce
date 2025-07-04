using System;

namespace ServiceLayer.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Order? Order { get; set; }
        public ProductVariant? Variant { get; set; }
    }
}
