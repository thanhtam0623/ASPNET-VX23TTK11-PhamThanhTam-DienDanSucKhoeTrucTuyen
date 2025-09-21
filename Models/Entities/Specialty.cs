using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class Specialty
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for experts with this specialty
        public virtual ICollection<UserAccount> Experts { get; set; } = new List<UserAccount>();
    }
}
