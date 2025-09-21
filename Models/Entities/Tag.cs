using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Slug { get; set; } = string.Empty;

        public string? Color { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Statistics
        public int TopicCount { get; set; } = 0;

        // Navigation Properties
        public virtual ICollection<TopicTag> TopicTags { get; set; } = new List<TopicTag>();
    }
}
