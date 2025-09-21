namespace ApiApplication.Models.DTOs.User
{
    public class HomePageDTO
    {
        public List<CategorySummaryDTO> Categories { get; set; } = new List<CategorySummaryDTO>();
        public List<TopicSummaryDTO> PinnedTopics { get; set; } = new List<TopicSummaryDTO>();
        public List<TopicSummaryDTO> LatestTopics { get; set; } = new List<TopicSummaryDTO>();
        public List<TagSummaryDTO> PopularTags { get; set; } = new List<TagSummaryDTO>();
        public SiteStatsDTO SiteStats { get; set; } = new SiteStatsDTO();
    }

    public class CategorySummaryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int TopicCount { get; set; }
        public int PostCount { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public string? LastTopicTitle { get; set; }
        public string? LastAuthorName { get; set; }
    }

    public class TopicSummaryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string CategorySlug { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string? AuthorAvatar { get; set; }
        public string AuthorRole { get; set; } = string.Empty;
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

    public class TagSummaryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int TopicCount { get; set; }
    }

    public class SiteStatsDTO
    {
        public int TotalTopics { get; set; }
        public int TotalPosts { get; set; }
        public int TotalUsers { get; set; }
        public int OnlineUsers { get; set; }
    }

    public class SearchRequest
    {
        public string? Query { get; set; }
        public int? CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
        public bool? HasAnswer { get; set; }
        public string? SortBy { get; set; } = "created_at"; // created_at, updated_at, view_count, like_count
        public string? SortOrder { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
