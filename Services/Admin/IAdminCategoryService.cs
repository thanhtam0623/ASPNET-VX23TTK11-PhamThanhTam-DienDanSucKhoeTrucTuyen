using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.Admin
{
    public interface IAdminCategoryService
    {
        Task<ApiResponse<List<AdminCategoryDTO>>> GetCategoriesAsync();
        Task<ApiResponse<AdminCategoryDTO>> GetCategoryByIdAsync(int categoryId);
        Task<ApiResponse<AdminCategoryDTO>> CreateCategoryAsync(AdminCreateCategoryRequest request);
        Task<ApiResponse<AdminCategoryDTO>> UpdateCategoryAsync(int categoryId, AdminUpdateCategoryRequest request);
        Task<ApiResponse> ToggleCategoryStatusAsync(int categoryId);
        Task<ApiResponse> DeleteCategoryAsync(int categoryId);
        Task<ApiResponse> ReorderCategoriesAsync(List<int> categoryIds);

        // Tag management
        Task<ApiResponse<List<AdminTagDTO>>> GetTagsAsync();
        Task<ApiResponse<AdminTagDTO>> GetTagByIdAsync(int tagId);
        Task<ApiResponse<AdminTagDTO>> CreateTagAsync(AdminCreateTagRequest request);
        Task<ApiResponse<AdminTagDTO>> UpdateTagAsync(int tagId, AdminUpdateTagRequest request);
        Task<ApiResponse> DeleteTagAsync(int tagId);
        Task<ApiResponse<AdminTagDTO>> MergeTagsAsync(AdminMergeTagsRequest request);
        Task<ApiResponse> RecountTagsAsync();
    }
}
