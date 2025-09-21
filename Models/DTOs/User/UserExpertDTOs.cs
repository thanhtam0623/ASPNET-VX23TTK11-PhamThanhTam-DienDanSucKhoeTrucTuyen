namespace ApiApplication.Models.DTOs.User
{
    public class SpecialtyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ExpertSummaryDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public List<string> Specialties { get; set; } = new List<string>();
        public string? Location { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public int AnswerCount { get; set; }
        public string Experience { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeen { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ExpertDetailDTO : ExpertSummaryDTO
    {
        public string? Education { get; set; }
        public string? Certifications { get; set; }
        public string? WorkHistory { get; set; }
        public List<TopicSummaryDTO> RecentTopics { get; set; } = new List<TopicSummaryDTO>();
        public List<ReviewDTO> RecentReviews { get; set; } = new List<ReviewDTO>();
        public ExpertStatsDTO Stats { get; set; } = new ExpertStatsDTO();
    }

    public class ExpertStatsDTO
    {
        public int TotalAnswers { get; set; }
        public int AcceptedAnswers { get; set; }
        public int TotalTopics { get; set; }
        public int TotalViews { get; set; }
        public int TotalLikes { get; set; }
        public double AcceptanceRate { get; set; }
        public double AverageRating { get; set; }
        public string JoinedDate { get; set; } = string.Empty;
        public string LastSeenAt { get; set; } = string.Empty;
        public string VerifiedAt { get; set; } = string.Empty;
        public List<string> PopularTopics { get; set; } = new List<string>();
    }

    public class ReviewDTO
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string? ReviewerAvatar { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public string TopicSlug { get; set; } = string.Empty;
    }

    public class ExpertSearchRequest
    {
        public string? Query { get; set; }
        public string? Specialty { get; set; }
        public double? MinRating { get; set; }
        public bool? IsOnline { get; set; }
        public string? SortBy { get; set; } = "rating"; // rating, experience, answers, reviews
        public string? SortOrder { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class TopicsListRequest
    {
        public string? Query { get; set; }
        public int? CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
        public bool? HasAnswer { get; set; }
        public bool? IsAnswered { get; set; }
        public string? AuthorRole { get; set; } // expert, member, all
        public string? SortBy { get; set; } = "latest"; // latest, popular, discussed, answered
        public string? SortOrder { get; set; } = "desc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
