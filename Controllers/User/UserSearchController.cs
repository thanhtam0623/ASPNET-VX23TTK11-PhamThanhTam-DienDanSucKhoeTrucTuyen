using ApiApplication.Models.DTOs.User;
using ApiApplication.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.User
{
    [ApiController]
    [Route("api/user/search")]
    [AllowAnonymous]
    public class UserSearchController : ControllerBase
    {
        private readonly IUserTopicService _topicService;

        public UserSearchController(IUserTopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet]
        public async Task<IActionResult> SearchTopics([FromQuery] string? query = null,
                                                     [FromQuery] int? categoryId = null,
                                                     [FromQuery] List<int>? tagIds = null,
                                                     [FromQuery] bool? hasAnswer = null,
                                                     [FromQuery] string? sortBy = "created_at",
                                                     [FromQuery] string? sortOrder = "desc",
                                                     [FromQuery] int page = 1,
                                                     [FromQuery] int pageSize = 20)
        {
            var request = new SearchRequest
            {
                Query = query,
                CategoryId = categoryId,
                TagIds = tagIds,
                HasAnswer = hasAnswer,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Page = page,
                PageSize = pageSize
            };

            var topicsRequest = new TopicsListRequest
            {
                Query = request.Query,
                CategoryId = request.CategoryId,
                TagIds = request.TagIds,
                HasAnswer = request.HasAnswer,
                SortBy = request.SortBy,
                SortOrder = request.SortOrder,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _topicService.GetTopicsAsync(topicsRequest);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> SearchTopicsPost([FromBody] SearchRequest request)
        {
            var topicsRequest = new TopicsListRequest
            {
                Query = request.Query,
                CategoryId = request.CategoryId,
                TagIds = request.TagIds,
                HasAnswer = request.HasAnswer,
                SortBy = request.SortBy,
                SortOrder = request.SortOrder,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _topicService.GetTopicsAsync(topicsRequest);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
