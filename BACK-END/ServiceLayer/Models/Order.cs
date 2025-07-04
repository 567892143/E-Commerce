using System;
using System.Collections.Generic;

namespace ServiceLayer.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<OrderItem>? Items { get; set; }
        public Payment? Payment { get; set; }
        public Shipping? Shipping { get; set; }
    }
}
