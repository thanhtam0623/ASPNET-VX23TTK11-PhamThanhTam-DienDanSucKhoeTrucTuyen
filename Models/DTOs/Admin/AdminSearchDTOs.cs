using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.DTOs.Admin
{
    public class AdminGlobalSearchRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Query cannot exceed 100 characters")]
        public string Query { get; set; } = string.Empty;

        public string? Type { get; set; } // users, topics, experts, categories, all

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, 50, ErrorMessage = "PageSize must be between 1 and 50")]
        public int PageSize { get; set; } = 10;
    }

    public class AdminGlobalSearchResponse
    {
        public List<AdminSearchResultItem> Users { get; set; } = new();
        public List<AdminSearchResultItem> Topics { get; set; } = new();
        public List<AdminSearchResultItem> Experts { get; set; } = new();
        public List<AdminSearchResultItem> Categories { get; set; } = new();
        public int TotalResults { get; set; }
        public string Query { get; set; } = string.Empty;
    }

    public class AdminSearchResultItem
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty; // user, topic, expert, category
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? Description { get; set; }
        public string? Avatar { get; set; }
        public string? Url { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class AdminSearchSuggestionsResponse
    {
        public List<string> Suggestions { get; set; } = new();
        public string Query { get; set; } = string.Empty;
    }
}
