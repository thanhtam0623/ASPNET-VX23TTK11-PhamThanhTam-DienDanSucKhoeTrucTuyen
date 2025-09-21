using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.User
{
    public interface IUserExpertService
    {
        Task<ApiResponse<PaginationResponse<ExpertSummaryDTO>>> GetExpertsAsync(ExpertSearchRequest request);
        Task<ApiResponse<ExpertDetailDTO>> GetExpertByUsernameAsync(string username);
        Task<ApiResponse<List<SpecialtyDTO>>> GetExpertSpecialtiesAsync();
        Task<ApiResponse<List<TopicSummaryDTO>>> GetExpertTopicsAsync(int expertId, int page = 1, int pageSize = 20);
        Task<ApiResponse<ExpertStatsDTO>> GetExpertStatsAsync(int expertId);
    }
}
