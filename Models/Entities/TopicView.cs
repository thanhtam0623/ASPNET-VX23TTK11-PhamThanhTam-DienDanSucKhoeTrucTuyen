using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class TopicView
    {
        [Key]
        public int Id { get; set; }

        public int TopicId { get; set; }
        public int? UserId { get; set; } // null for anonymous views
        public string? IpAddress { get; set; }

        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Topic Topic { get; set; } = null!;
        public virtual UserAccount? User { get; set; }
    }
}
