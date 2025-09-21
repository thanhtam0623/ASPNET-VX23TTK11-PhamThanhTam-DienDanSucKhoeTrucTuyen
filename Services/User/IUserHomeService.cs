using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.User
{
    public interface IUserHomeService
    {
        Task<ApiResponse<HomePageDTO>> GetHomePageDataAsync();
        Task<ApiResponse<PaginationResponse<TopicSummaryDTO>>> SearchTopicsAsync(SearchRequest request);
        Task<ApiResponse<List<CategorySummaryDTO>>> GetCategoriesAsync();
        Task<ApiResponse<List<TopicSummaryDTO>>> GetPinnedTopicsAsync();
        Task<ApiResponse<List<TopicSummaryDTO>>> GetLatestTopicsAsync(int count = 10);
        Task<ApiResponse<List<TagSummaryDTO>>> GetPopularTagsAsync(int count = 20);
        Task<ApiResponse<SiteStatsDTO>> GetSiteStatsAsync();
    }
}
