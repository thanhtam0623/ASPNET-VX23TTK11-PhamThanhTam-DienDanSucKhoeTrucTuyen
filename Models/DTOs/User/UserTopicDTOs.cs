using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.DTOs.User
{
    public class TopicDetailDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public CategorySummaryDTO Category { get; set; } = null!;
        public UserSummaryDTO Author { get; set; } = null!;
        public bool IsLocked { get; set; }
        public bool HasAnswer { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int PostCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TagSummaryDTO> Tags { get; set; } = new List<TagSummaryDTO>();
        public bool IsLikedByCurrentUser { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

    public class PostDetailDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public UserSummaryDTO Author { get; set; } = null!;
        public bool IsAnswer { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PostDetailDTO> Replies { get; set; } = new List<PostDetailDTO>();
        public bool IsLikedByCurrentUser { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanMarkAsAnswer { get; set; }
    }

    public class UserSummaryDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int TopicCount { get; set; }
        public int PostCount { get; set; }
    }

    public class CreateTopicRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        public List<string>? Tags { get; set; }
    }

    public class UpdateTopicRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        public List<string>? Tags { get; set; }
    }

    public class CreatePostRequest
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        public int? ParentPostId { get; set; }
    }

    public class UpdatePostRequest
    {
        [Required]
        public string Content { get; set; } = string.Empty;
    }

    public class ReportRequest
    {
        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }

    public class CategoryPageDTO
    {
        public CategorySummaryDTO Category { get; set; } = null!;
        public List<TopicSummaryDTO> Topics { get; set; } = new List<TopicSummaryDTO>();
        public int TotalTopics { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<TagSummaryDTO> PopularTags { get; set; } = new List<TagSummaryDTO>();
    }

    public class CategoryTopicsRequest
    {
        public string? SortBy { get; set; } = "created_at"; // created_at, last_activity, view_count, like_count
        public string? Filter { get; set; } = "all"; // all, answered, unanswered, popular
        public List<int>? TagIds { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
