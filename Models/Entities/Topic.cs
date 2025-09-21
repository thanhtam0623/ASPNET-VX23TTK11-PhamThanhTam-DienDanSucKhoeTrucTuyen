using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        // Foreign Keys
        public int CategoryId { get; set; }
        public int UserId { get; set; }

        // Status
        public bool IsActive { get; set; } = true;
        public bool IsPinned { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public bool HasAnswer { get; set; } = false;

        // Statistics
        public int ViewCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public int PostCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastActivityAt { get; set; }

        // Navigation Properties
        public virtual Category Category { get; set; } = null!;
        public virtual UserAccount User { get; set; } = null!;
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<TopicTag> TopicTags { get; set; } = new List<TopicTag>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<TopicView> Views { get; set; } = new List<TopicView>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
