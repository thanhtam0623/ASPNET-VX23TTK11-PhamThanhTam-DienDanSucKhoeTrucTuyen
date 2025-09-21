using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int? TopicId { get; set; }
        public int? PostId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty; // Spam, Inappropriate, Copyright, etc.

        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Reviewed, Resolved, Dismissed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
        public int? ReviewedByAdminId { get; set; }

        // Navigation Properties
        public virtual UserAccount User { get; set; } = null!;
        public virtual Topic? Topic { get; set; }
        public virtual Post? Post { get; set; }
    }
}
