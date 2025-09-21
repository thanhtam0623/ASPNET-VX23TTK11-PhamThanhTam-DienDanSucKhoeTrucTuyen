using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.DTOs.Admin
{
    public class AdminTopicDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
        public bool HasAnswer { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int PostCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class AdminTopicFilterRequest
    {
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }
        public int? AuthorId { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPinned { get; set; }
        public bool? IsLocked { get; set; }
        public bool? HasAnswer { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string? Author { get; set; }
    }

    public class AdminTopicSearchRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public string? Status { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? IsPinned { get; set; }
        public bool? IsLocked { get; set; }
    }

    public class AdminUpdateTopicRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class AdminUpdateTopicStatusRequest
    {
        public bool? IsActive { get; set; }
        public bool? IsPinned { get; set; }
        public bool? IsLocked { get; set; }
    }

    public class AdminPostDTO
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int TopicId { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsAnswer { get; set; }
        public int LikeCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AdminTopicResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsPinned { get; set; }
        public bool IsLocked { get; set; }
        public bool HasAnswer { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int PostCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    public class AdminTopicStatsDTO
    {
        public int TotalTopics { get; set; }
        public int ActiveTopics { get; set; }
        public int PinnedTopics { get; set; }
        public int LockedTopics { get; set; }
        public int TopicsWithAnswers { get; set; }
        public int NewTopicsToday { get; set; }
        public int NewTopicsThisWeek { get; set; }
        public int NewTopicsThisMonth { get; set; }
    }
}
