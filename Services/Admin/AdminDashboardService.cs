using ApiApplication.Data;
using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.Admin
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly AppDbContext _context;

        public AdminDashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<DashboardStatsDTO>> GetStatsAsync()
        {
            try
            {
                var today = DateTime.UtcNow.Date;

                var stats = new DashboardStatsDTO
                {
                    TotalUsers = await _context.UserAccounts.CountAsync(),
                    TotalTopics = await _context.Topics.CountAsync(),
                    TotalPosts = await _context.Posts.CountAsync(),
                    TotalReports = await _context.Reports.CountAsync(),
                    TotalCategories = await _context.Categories.CountAsync(),
                    NewUsersToday = await _context.UserAccounts.CountAsync(u => u.CreatedAt >= today),
                    NewTopicsToday = await _context.Topics.CountAsync(t => t.CreatedAt >= today),
                    NewPostsToday = await _context.Posts.CountAsync(p => p.CreatedAt >= today),
                    PendingReports = await _context.Reports.CountAsync(r => r.Status == "Chờ xử lý")
                };

                return ApiResponse<DashboardStatsDTO>.SuccessResult(stats);
            }
            catch (Exception ex)
            {
                return ApiResponse<DashboardStatsDTO>.ErrorResult($"Failed to get stats: {ex.Message}");
            }
        }

        public async Task<ApiResponse<DashboardChartDataDTO>> GetChartDataAsync(int days = 30)
        {
            try
            {
                var startDate = DateTime.UtcNow.AddDays(-365).Date;

                // Topics chart data
                var topicsData = await _context.Topics
                    .Where(t => t.CreatedAt >= startDate)
                    .GroupBy(t => t.CreatedAt.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .OrderBy(x => x.Date)
                    .ToListAsync();

                var topicsChartData = topicsData.Select(x => new ChartDataPoint
                {
                    Date = x.Date.ToString("yyyy-MM-dd"),
                    Count = x.Count
                }).ToList();

                // Users chart data
                var usersData = await _context.UserAccounts
                    .Where(u => u.CreatedAt >= startDate)
                    .GroupBy(u => u.CreatedAt.Date)
                    .Select(g => new { Date = g.Key, Count = g.Count() })
                    .OrderBy(x => x.Date)
                    .ToListAsync();

                var usersChartData = usersData.Select(x => new ChartDataPoint
                {
                    Date = x.Date.ToString("yyyy-MM-dd"),
                    Count = x.Count
                }).ToList();

                // Reports chart data
                var reportsData = await _context.Reports
                    .Where(r => r.CreatedAt >= startDate)
                    .GroupBy(r => r.Category)
                    .Select(g => new ReportCategoryChart
                    {
                        Category = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                var chartData = new DashboardChartDataDTO
                {
                    TopicsChart = topicsChartData,
                    UsersChart = usersChartData,
                    ReportsChart = reportsData
                };

                return ApiResponse<DashboardChartDataDTO>.SuccessResult(chartData);
            }
            catch (Exception ex)
            {
                return ApiResponse<DashboardChartDataDTO>.ErrorResult($"Failed to get chart data: {ex.Message}");
            }
        }

        public Task<ApiResponse<List<QuickActionDTO>>> GetQuickActionsAsync()
        {
            try
            {
                var actions = new List<QuickActionDTO>
                {
                    new QuickActionDTO
                    {
                        Title = "Quản lý người dùng",
                        Description = "Xem và quản lý tài khoản người dùng",
                        Icon = "users",
                        Url = "/admin/users",
                        Color = "primary"
                    },
                    new QuickActionDTO
                    {
                        Title = "Quản lý danh mục",
                        Description = "Tổ chức các danh mục thảo luận",
                        Icon = "folder",
                        Url = "/admin/categories",
                        Color = "success"
                    },
                    new QuickActionDTO
                    {
                        Title = "Quản lý chủ đề",
                        Description = "Kiểm duyệt chủ đề và bài viết",
                        Icon = "message-square",
                        Url = "/admin/topics",
                        Color = "info"
                    },
                    new QuickActionDTO
                    {
                        Title = "Báo cáo",
                        Description = "Xem xét các báo cáo từ người dùng",
                        Icon = "flag",
                        Url = "/admin/reports",
                        Color = "warning"
                    },
                    new QuickActionDTO
                    {
                        Title = "Cài đặt hệ thống",
                        Description = "Cấu hình các thiết lập hệ thống",
                        Icon = "settings",
                        Url = "/admin/settings",
                        Color = "secondary"
                    }
                };

                return Task.FromResult(ApiResponse<List<QuickActionDTO>>.SuccessResult(actions));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ApiResponse<List<QuickActionDTO>>.ErrorResult($"Failed to get quick actions: {ex.Message}"));
            }
        }
    }
}
