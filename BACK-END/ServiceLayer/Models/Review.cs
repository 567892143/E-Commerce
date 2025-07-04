using System;

namespace ServiceLayer.Models
{
    public class Review
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsVerifiedPurchase { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Product? Product { get; set; }
    }
}
