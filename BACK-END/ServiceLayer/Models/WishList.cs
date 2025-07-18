using System;

namespace ServiceLayer.Models
{
    public class Wishlist
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Product? Product { get; set; }
    }
}
