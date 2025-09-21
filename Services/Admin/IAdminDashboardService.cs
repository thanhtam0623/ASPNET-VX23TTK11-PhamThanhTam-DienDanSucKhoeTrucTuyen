using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.Admin
{
    public interface IAdminDashboardService
    {
        Task<ApiResponse<DashboardStatsDTO>> GetStatsAsync();
        Task<ApiResponse<DashboardChartDataDTO>> GetChartDataAsync(int days = 30);
        Task<ApiResponse<List<QuickActionDTO>>> GetQuickActionsAsync();
    }
}
