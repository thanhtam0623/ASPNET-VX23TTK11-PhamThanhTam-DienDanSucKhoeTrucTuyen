using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        // Foreign Keys
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public int? ParentPostId { get; set; } // For nested replies

        // Status
        public bool IsActive { get; set; } = true;
        public bool IsAnswer { get; set; } = false; // Marked as answer by topic author or moderator

        // Statistics
        public int LikeCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Topic Topic { get; set; } = null!;
        public virtual UserAccount User { get; set; } = null!;
        public virtual Post? ParentPost { get; set; }
        public virtual ICollection<Post> Replies { get; set; } = new List<Post>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
