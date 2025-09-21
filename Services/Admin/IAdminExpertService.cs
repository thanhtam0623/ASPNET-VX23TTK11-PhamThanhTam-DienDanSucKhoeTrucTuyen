using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.Admin
{
    public interface IAdminExpertService
    {
        Task<ApiResponse<PaginationResponse<AdminExpertResponse>>> GetExpertsAsync(PaginationRequest pagination, AdminExpertFilterRequest? filter = null);
        Task<ApiResponse<AdminExpertResponse>> GetExpertByIdAsync(int expertId);
        Task<ApiResponse<AdminExpertStatsResponse>> GetExpertStatsAsync();
        Task<ApiResponse<bool>> VerifyExpertAsync(int expertId, AdminVerifyExpertRequest request);
        Task<ApiResponse<bool>> ToggleExpertStatusAsync(int expertId);
        Task<ApiResponse<bool>> UpdateExpertSpecialtyAsync(int expertId, AdminUpdateExpertSpecialtyRequest request);
        Task<ApiResponse<List<AdminSpecialtyResponse>>> GetSpecialtiesAsync();
        Task<ApiResponse<AdminSpecialtyResponse>> CreateSpecialtyAsync(AdminCreateSpecialtyRequest request);
        Task<ApiResponse<AdminSpecialtyResponse>> UpdateSpecialtyAsync(int specialtyId, AdminUpdateSpecialtyRequest request);
        Task<ApiResponse<bool>> DeleteSpecialtyAsync(int specialtyId);
        Task<ApiResponse<PaginationResponse<AdminExpertReviewResponse>>> GetExpertReviewsAsync(int expertId, PaginationRequest pagination);
        Task<ApiResponse<bool>> DeleteReviewAsync(int reviewId);
    }
}
