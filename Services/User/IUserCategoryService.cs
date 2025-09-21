using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.User
{
    public interface IUserCategoryService
    {
        Task<ApiResponse<CategoryPageDTO>> GetCategoryPageAsync(string categorySlug, CategoryTopicsRequest request);
        Task<ApiResponse<CategorySummaryDTO>> GetCategoryBySlugAsync(string slug);
        Task<ApiResponse<List<CategorySummaryDTO>>> GetAllCategoriesAsync();
        Task<ApiResponse<List<TagSummaryDTO>>> GetCategoryTagsAsync(int categoryId);
    }
}
