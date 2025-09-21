using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class Like
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int? TopicId { get; set; }
        public int? PostId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual UserAccount User { get; set; } = null!;
        public virtual Topic? Topic { get; set; }
        public virtual Post? Post { get; set; }
    }
}
