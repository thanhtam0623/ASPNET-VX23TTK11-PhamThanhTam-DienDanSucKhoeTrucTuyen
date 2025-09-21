using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.DTOs.Admin
{
    public class AdminUserListDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int TopicCount { get; set; }
        public int PostCount { get; set; }
    }

    public class AdminUserFilterRequest
    {
        public string? Role { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEmailVerified { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
    }

    public class AdminUserSearchRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Role { get; set; }
        public string? Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class AdminCreateUserRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string FullName { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        public string Role { get; set; } = "Member"; // Member, Doctor, Moderator
    }

    public class AdminUpdateUserRequest
    {
        public string? FullName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Role { get; set; } // Member, Doctor, Moderator
        public bool? IsActive { get; set; }
    }

    public class AdminUpdateUserRoleRequest
    {
        [Required]
        public string Role { get; set; } = string.Empty; // Member, Doctor, Moderator
    }

    public class AdminResetPasswordRequest
    {
        public int UserId { get; set; }
        
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class AdminUserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int TopicCount { get; set; }
        public int PostCount { get; set; }
    }

    public class AdminUserStatsDTO
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int MemberCount { get; set; }
        public int DoctorCount { get; set; }
        public int ModeratorCount { get; set; }
        public int NewUsersThisMonth { get; set; }
    }
}
