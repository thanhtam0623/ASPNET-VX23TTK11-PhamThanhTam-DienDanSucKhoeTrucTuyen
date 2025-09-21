using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/topics")]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminTopicController : ControllerBase
    {
        private readonly IAdminTopicService _topicService;

        public AdminTopicController(IAdminTopicService topicService)
        {
            _topicService = topicService;
        }

        // Topics
        [HttpGet]
        public async Task<IActionResult> GetTopics([FromQuery] PaginationRequest pagination, [FromQuery] AdminTopicFilterRequest? filter = null)
        {
            var result = await _topicService.GetTopicsAsync(pagination, filter);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchTopics([FromBody] AdminTopicSearchRequest request)
        {
            var pagination = new PaginationRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            };
            
            var filter = new AdminTopicFilterRequest
            {
                CategoryId = request.CategoryId,
                Status = request.Status,
                AuthorId = request.AuthorId,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                IsPinned = request.IsPinned,
                IsLocked = request.IsLocked
            };
            
            var result = await _topicService.GetTopicsAsync(pagination, filter);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{topicId}")]
        public async Task<IActionResult> GetTopic(int topicId)
        {
            var result = await _topicService.GetTopicByIdAsync(topicId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetTopicStats()
        {
            var result = await _topicService.GetTopicStatsAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("{topicId}/status")]
        public async Task<IActionResult> UpdateTopicStatus(int topicId, [FromBody] AdminUpdateTopicStatusRequest request)
        {
            var result = await _topicService.UpdateTopicStatusAsync(topicId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("{topicId}/toggle-pin")]
        public async Task<IActionResult> ToggleTopicPin(int topicId)
        {
            var result = await _topicService.ToggleTopicPinAsync(topicId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("{topicId}")]
        public async Task<IActionResult> UpdateTopic(int topicId, [FromBody] AdminUpdateTopicRequest request)
        {
            var result = await _topicService.UpdateTopicAsync(topicId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("{topicId}")]
        public async Task<IActionResult> DeleteTopic(int topicId)
        {
            var result = await _topicService.DeleteTopicAsync(topicId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        // Posts
        [HttpGet("posts")]
        public async Task<IActionResult> GetPosts([FromQuery] PaginationRequest pagination, [FromQuery] int? topicId = null)
        {
            var result = await _topicService.GetPostsAsync(pagination, topicId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("posts/{postId}")]
        public async Task<IActionResult> GetPost(int postId)
        {
            var result = await _topicService.GetPostByIdAsync(postId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("posts/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var result = await _topicService.DeletePostAsync(postId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("posts/{postId}/mark-as-answer")]
        public async Task<IActionResult> MarkPostAsAnswer(int postId)
        {
            var result = await _topicService.MarkPostAsAnswerAsync(postId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        // Reports
        [HttpGet("reports")]
        public async Task<IActionResult> GetReports([FromQuery] AdminReportsRequest request)
        {
            var result = await _topicService.GetReportsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("reports/{reportId}/status")]
        public async Task<IActionResult> UpdateReportStatus(int reportId, [FromBody] AdminUpdateReportStatusRequest request)
        {
            var result = await _topicService.UpdateReportStatusAsync(reportId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
