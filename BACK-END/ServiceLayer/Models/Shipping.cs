using System;

namespace ServiceLayer.Models
{
    public enum ShippingStatus
    {
        Processing,
        Shipped,
        Delivered,
        Failed
    }

    public class Shipping
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ContactId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public ShippingStatus Status { get; set; }
        public string Carrier { get; set; } = null!;
        public DateTime EstimatedDelivery { get; set; }
        public DateTime? ActualDelivery { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Order? Order { get; set; }
    }
}
