namespace ApiApplication.Models.DTOs.Admin
{
    public class DashboardStatsDTO
    {
        public int TotalUsers { get; set; }
        public int TotalTopics { get; set; }
        public int TotalPosts { get; set; }
        public int TotalReports { get; set; }
        public int TotalCategories { get; set; }
        public int NewUsersToday { get; set; }
        public int NewTopicsToday { get; set; }
        public int NewPostsToday { get; set; }
        public int PendingReports { get; set; }
    }

    public class DashboardChartDataDTO
    {
        public List<ChartDataPoint> TopicsChart { get; set; } = new List<ChartDataPoint>();
        public List<ChartDataPoint> UsersChart { get; set; } = new List<ChartDataPoint>();
        public List<ReportCategoryChart> ReportsChart { get; set; } = new List<ReportCategoryChart>();
    }

    public class ChartDataPoint
    {
        public string Date { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class ReportCategoryChart
    {
        public string Category { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class QuickActionDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
