using System;

namespace ServiceLayer.Models
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid ContactId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
