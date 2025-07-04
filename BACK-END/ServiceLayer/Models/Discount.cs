using System;
using System.Collections.Generic;

namespace ServiceLayer.Models
{
    public enum DiscountType
    {
        Percentage,
        FixedAmount
    }

    public class Discount
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DiscountType Type { get; set; }
        public decimal Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<DiscountRule>? Rules { get; set; }
    }
}
