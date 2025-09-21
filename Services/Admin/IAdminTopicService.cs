using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.Admin
{
    public interface IAdminTopicService
    {
        Task<ApiResponse<PaginationResponse<AdminTopicDTO>>> GetTopicsAsync(PaginationRequest pagination, AdminTopicFilterRequest? filter = null);
        Task<ApiResponse<AdminTopicDTO>> GetTopicByIdAsync(int topicId);
        Task<ApiResponse<AdminTopicStatsDTO>> GetTopicStatsAsync();
        Task<ApiResponse<AdminTopicDTO>> UpdateTopicAsync(int topicId, AdminUpdateTopicRequest request);
        Task<ApiResponse> UpdateTopicStatusAsync(int topicId, AdminUpdateTopicStatusRequest request);
        Task<ApiResponse> ToggleTopicPinAsync(int topicId);
        Task<ApiResponse> DeleteTopicAsync(int topicId);
        
        // Post management
        Task<ApiResponse<PaginationResponse<AdminPostDTO>>> GetPostsAsync(PaginationRequest pagination, int? topicId = null);
        Task<ApiResponse<AdminPostDTO>> GetPostByIdAsync(int postId);
        Task<ApiResponse> DeletePostAsync(int postId);
        Task<ApiResponse> MarkPostAsAnswerAsync(int postId);

        // Report management  
        Task<ApiResponse<PaginationResponse<AdminReportDTO>>> GetReportsAsync(AdminReportsRequest request);
        Task<ApiResponse> UpdateReportStatusAsync(int reportId, AdminUpdateReportStatusRequest request);
    }
}
