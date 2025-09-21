using System.ComponentModel.DataAnnotations;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Models.DTOs.Admin
{
    public class AdminReportsRequest : PaginationRequest
    {
        public string? Status { get; set; } // Pending, Reviewed, Resolved, Dismissed
        public string? Category { get; set; }
    }

    public class AdminReportDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? TopicTitle { get; set; }
        public string? PostContent { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? ReviewedByAdmin { get; set; }
    }

    public class AdminUpdateReportStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty; // Reviewed, Resolved, Dismissed
    }
}
