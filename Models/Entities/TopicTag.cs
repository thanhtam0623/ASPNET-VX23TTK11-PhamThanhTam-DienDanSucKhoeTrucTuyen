using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class TopicTag
    {
        [Key]
        public int Id { get; set; }

        public int TopicId { get; set; }
        public int TagId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Topic Topic { get; set; } = null!;
        public virtual Tag Tag { get; set; } = null!;
    }
}
