using ApiApplication.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _dashboardService;

        public AdminDashboardController(IAdminDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _dashboardService.GetStatsAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("chart-data")]
        public async Task<IActionResult> GetChartData([FromQuery] int days = 30)
        {
            var result = await _dashboardService.GetChartDataAsync(days);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("quick-actions")]
        public async Task<IActionResult> GetQuickActions()
        {
            var result = await _dashboardService.GetQuickActionsAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var stats = await _dashboardService.GetStatsAsync();
            var chartData = await _dashboardService.GetChartDataAsync(30);
            var quickActions = await _dashboardService.GetQuickActionsAsync();
            
            if (stats.Success && chartData.Success && quickActions.Success)
            {
                var dashboard = new
                {
                    Stats = stats.Data,
                    ChartData = chartData.Data,
                    QuickActions = quickActions.Data
                };
                
                return Ok(new { Success = true, Data = dashboard });
            }
            
            return BadRequest(new { Success = false, Message = "Failed to load dashboard data" });
        }
    }
}
