using System;

namespace ServiceLayer.Models
{
    public enum DiscountRuleType
    {
        Product,
        Category,
        User,
        OrderValue
    }

    public class DiscountRule
    {
        public Guid Id { get; set; }
        public Guid DiscountId { get; set; }
        public DiscountRuleType Type { get; set; }
        public Guid EntityId { get; set; }
        public decimal MinOrderValue { get; set; }
        public int MinQuantity { get; set; }
        public bool IsStackable { get; set; }
        public DateTime CreatedAt { get; set; }

        public Discount? Discount { get; set; }
    }
}
