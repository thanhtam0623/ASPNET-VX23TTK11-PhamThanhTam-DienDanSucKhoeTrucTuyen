using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.User
{
    public interface IUserProfileService
    {
        Task<ApiResponse<UserProfileDTO>> GetProfileAsync(int userId);
        Task<ApiResponse<UserProfileDTO>> GetPublicProfileAsync(string username);
        Task<ApiResponse<UserProfileDTO>> UpdateProfileAsync(int userId, UserUpdateProfileRequest request);
        Task<ApiResponse<PaginationResponse<TopicSummaryDTO>>> GetUserTopicsAsync(int userId, PaginationRequest pagination);
        Task<ApiResponse<List<PostDetailDTO>>> GetUserPostsAsync(int userId, PaginationRequest pagination);
    }
}
