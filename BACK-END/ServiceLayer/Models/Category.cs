using System;
using System.Collections.Generic;

namespace ServiceLayer.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public Guid? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Category? ParentCategory { get; set; }
        public ICollection<Category>? SubCategories { get; set; }
    }
}
