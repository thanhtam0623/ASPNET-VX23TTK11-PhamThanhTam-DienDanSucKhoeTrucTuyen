using ApiApplication.Models.DTOs.User;
using ApiApplication.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiApplication.Controllers.User
{
    [ApiController]
    [Route("api/user/topics")]
    public class UserTopicController : ControllerBase
    {
        private readonly IUserTopicService _topicService;

        public UserTopicController(IUserTopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopicById(int id)
        {
            var currentUserId = GetCurrentUserId();
            var result = await _topicService.GetTopicByIdAsync(id, currentUserId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTopicBySlug(string slug)
        {
            var currentUserId = GetCurrentUserId();
            var result = await _topicService.GetTopicBySlugAsync(slug, currentUserId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{topicId}/posts")]
        public async Task<IActionResult> GetTopicPosts(int topicId, 
                                                      [FromQuery] int page = 1, 
                                                      [FromQuery] int pageSize = 20)
        {
            // Validate parameters
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 100) pageSize = 100; // Giới hạn tối đa 100 posts per page
            
            var currentUserId = GetCurrentUserId();
            var result = await _topicService.GetTopicPostsAsync(topicId, currentUserId, page, pageSize);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTopics([FromQuery] string? query = null,
                                                  [FromQuery] int? categoryId = null,
                                                  [FromQuery] List<int>? tagIds = null,
                                                  [FromQuery] bool? hasAnswer = null,
                                                  [FromQuery] bool? isAnswered = null,
                                                  [FromQuery] string? authorRole = null,
                                                  [FromQuery] string? sortBy = "latest",
                                                  [FromQuery] string? sortOrder = "desc",
                                                  [FromQuery] int page = 1,
                                                  [FromQuery] int pageSize = 20)
        {
            var request = new TopicsListRequest
            {
                Query = query,
                CategoryId = categoryId,
                TagIds = tagIds,
                HasAnswer = hasAnswer,
                IsAnswered = isAnswered,
                AuthorRole = authorRole,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Page = page,
                PageSize = pageSize
            };

            var result = await _topicService.GetTopicsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchTopics([FromBody] TopicsListRequest request)
        {
            var result = await _topicService.GetTopicsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CreateTopic([FromBody] CreateTopicRequest request)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.CreateTopicAsync(userId, request);
            
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetTopicBySlug), new { slug = result.Data?.Slug }, result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("{topicId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateTopic(int topicId, [FromBody] UpdateTopicRequest request)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.UpdateTopicAsync(topicId, userId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("{topicId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> DeleteTopic(int topicId)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.DeleteTopicAsync(topicId, userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        // Posts
        [HttpPost("{topicId}/posts")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> CreatePost(int topicId, [FromBody] CreatePostRequest request)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.CreatePostAsync(topicId, userId, request);
            
            if (result.Success)
            {
                return Created($"posts/{result.Data?.Id}", result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("posts/{postId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdatePost(int postId, [FromBody] UpdatePostRequest request)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.UpdatePostAsync(postId, userId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("posts/{postId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.DeletePostAsync(postId, userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("posts/{postId}/mark-as-answer")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> MarkPostAsAnswer(int postId)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.MarkPostAsAnswerAsync(postId, userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        // Interactions
        [HttpPost("{topicId}/like")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> LikeTopic(int topicId)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.LikeTopicAsync(topicId, userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("{topicId}/like")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UnlikeTopic(int topicId)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.UnlikeTopicAsync(topicId, userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("posts/{postId}/like")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> LikePost(int postId)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.LikePostAsync(postId, userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("posts/{postId}/like")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UnlikePost(int postId)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.UnlikePostAsync(postId, userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        // Reports
        [HttpPost("{topicId}/report")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> ReportTopic(int topicId, [FromBody] ReportRequest request)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.ReportTopicAsync(topicId, userId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("posts/{postId}/report")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> ReportPost(int postId, [FromBody] ReportRequest request)
        {
            var userId = GetCurrentUserId()!.Value;
            var result = await _topicService.ReportPostAsync(postId, userId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        // Views
        [HttpPost("{topicId}/view")]
        public async Task<IActionResult> RecordTopicView(int topicId)
        {
            var currentUserId = GetCurrentUserId();
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var result = await _topicService.RecordTopicViewAsync(topicId, currentUserId, ipAddress);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        private int? GetCurrentUserId()
        {
            // First try standard claims (when endpoint has [Authorize])
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }

            // For AllowAnonymous endpoints, manually parse JWT if present
            var authHeader = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                try
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    var jwtService = HttpContext.RequestServices.GetRequiredService<Services.Common.IJwtService>();
                    
                    // Validate token and get userId directly
                    if (jwtService.ValidateToken(token, false)) // false = user token
                    {
                        var userIdFromToken = jwtService.GetUserIdFromToken(token);
                        return userIdFromToken;
                    }
                }
                catch
                {
                    // Token invalid, continue as anonymous
                }
            }

            return null;
        }
    }
}
