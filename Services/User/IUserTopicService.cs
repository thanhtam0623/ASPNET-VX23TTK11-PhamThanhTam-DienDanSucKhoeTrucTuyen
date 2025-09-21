using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.User
{
    public interface IUserTopicService
    {
        Task<ApiResponse<TopicDetailDTO>> GetTopicByIdAsync(int topicId, int? currentUserId = null);
        Task<ApiResponse<TopicDetailDTO>> GetTopicBySlugAsync(string slug, int? currentUserId = null);
        Task<ApiResponse<PaginationResponse<PostDetailDTO>>> GetTopicPostsAsync(int topicId, int? currentUserId = null, int page = 1, int pageSize = 20);
        Task<ApiResponse<TopicDetailDTO>> CreateTopicAsync(int userId, CreateTopicRequest request);
        Task<ApiResponse<TopicDetailDTO>> UpdateTopicAsync(int topicId, int userId, UpdateTopicRequest request);
        Task<ApiResponse> DeleteTopicAsync(int topicId, int userId);

        // Posts
        Task<ApiResponse<PostDetailDTO>> CreatePostAsync(int topicId, int userId, CreatePostRequest request);
        Task<ApiResponse<PostDetailDTO>> UpdatePostAsync(int postId, int userId, UpdatePostRequest request);
        Task<ApiResponse> DeletePostAsync(int postId, int userId);
        Task<ApiResponse> MarkPostAsAnswerAsync(int postId, int userId);

        // Interactions
        Task<ApiResponse> LikeTopicAsync(int topicId, int userId);
        Task<ApiResponse> UnlikeTopicAsync(int topicId, int userId);
        Task<ApiResponse> LikePostAsync(int postId, int userId);
        Task<ApiResponse> UnlikePostAsync(int postId, int userId);
        Task<ApiResponse> ReportTopicAsync(int topicId, int userId, ReportRequest request);
        Task<ApiResponse> ReportPostAsync(int postId, int userId, ReportRequest request);

        // Views
        Task<ApiResponse> RecordTopicViewAsync(int topicId, int? userId, string? ipAddress);

        // Topics Listing
        Task<ApiResponse<PaginationResponse<TopicSummaryDTO>>> GetTopicsAsync(TopicsListRequest request);
    }
}
