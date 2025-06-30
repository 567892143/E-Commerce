using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace   ServiceLayer.Models
{

    public class Role
    {
        /// <summary>
        /// Primary key for the role.
        /// </summary>
        [Key]

        public long Id { get; set; }

        /// <summary>
        /// Name of the role (e.g., Admin, User).
        /// </summary>
        [Required]
        [MaxLength(100)] // Adjust length if needed
        public string Name { get; set; } = string.Empty;
    }
}
