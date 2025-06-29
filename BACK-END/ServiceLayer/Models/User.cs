using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

 namespace ServiceLayer.Models;


public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public long Role { get; set; }

    [Required]
    public Guid ContactId { get; set; }

    [ForeignKey("ContactId")]
    public Contact Contact { get; set; }

    [Required]
    public string Password { get; set; }  // Assumed to be hashed

    public DateTime? LastLogin { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }  // Soft delete
}
