using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.Entities
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public string? Avatar { get; set; }
        
        [MaxLength(500)]
        public string? Bio { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "Member"; // Member, Doctor, Moderator

        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = false;

        // Privacy Settings
        public bool ShowEmail { get; set; } = false;
        public bool ShowBio { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        // Reset Password
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpires { get; set; }

        // Navigation Properties
        public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
