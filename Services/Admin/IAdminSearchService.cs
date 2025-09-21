using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.Admin
{
    public interface IAdminSearchService
    {
        Task<ApiResponse<AdminGlobalSearchResponse>> GlobalSearchAsync(AdminGlobalSearchRequest request);
        Task<ApiResponse<PaginationResponse<AdminUserResponse>>> SearchUsersAsync(AdminUserSearchRequest request);
        Task<ApiResponse<PaginationResponse<AdminTopicResponse>>> SearchTopicsAsync(AdminTopicSearchRequest request);
        Task<ApiResponse<PaginationResponse<AdminExpertResponse>>> SearchExpertsAsync(AdminExpertSearchRequest request);
        Task<ApiResponse<List<AdminCategoryResponse>>> SearchCategoriesAsync(string query);
        Task<ApiResponse<AdminSearchSuggestionsResponse>> GetSearchSuggestionsAsync(string query, int limit);
    }
}
