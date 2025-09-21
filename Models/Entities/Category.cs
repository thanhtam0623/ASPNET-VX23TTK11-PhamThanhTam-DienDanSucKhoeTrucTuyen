using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? Icon { get; set; }
        public string? Color { get; set; }

        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Statistics
        public int TopicCount { get; set; } = 0;
        public int PostCount { get; set; } = 0;

        // Navigation Properties
        public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}
