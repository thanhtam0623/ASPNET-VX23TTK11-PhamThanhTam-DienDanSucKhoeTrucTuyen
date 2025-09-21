using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.DTOs.Admin
{
    public class AdminExpertFilterRequest
    {
        public string? Specialty { get; set; }
        public string? VerificationStatus { get; set; }
        public string? Status { get; set; }
        public double? MinRating { get; set; }
        public bool? IsOnline { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class AdminExpertSearchRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        public string? Specialty { get; set; }
        public string? VerificationStatus { get; set; }
        public string? Status { get; set; }
        public double? MinRating { get; set; }
        public bool? IsOnline { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

    public class AdminExpertResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public string? Specialty { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public bool IsVerified { get; set; }
        public bool IsOnline { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int TopicCount { get; set; }
        public int PostCount { get; set; }
    }

    public class AdminVerifyExpertRequest
    {
        [Required]
        public bool IsVerified { get; set; }
        
        public string? VerificationNotes { get; set; }
    }

    public class AdminUpdateExpertSpecialtyRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Specialty cannot exceed 100 characters")]
        public string Specialty { get; set; } = string.Empty;
    }

    public class AdminCreateSpecialtyRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }

    public class AdminUpdateSpecialtyRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }

    public class AdminSpecialtyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ExpertCount { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
    }

    public class AdminExpertStatsResponse
    {
        public int TotalExperts { get; set; }
        public int VerifiedExperts { get; set; }
        public int OnlineExperts { get; set; }
        public int NewExpertsThisMonth { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public List<AdminSpecialtyStatsResponse> TopSpecialties { get; set; } = new();
    }

    public class AdminSpecialtyStatsResponse
    {
        public string Name { get; set; } = string.Empty;
        public int ExpertCount { get; set; }
    }

    public class AdminExpertReviewResponse
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsReported { get; set; }
    }
}
