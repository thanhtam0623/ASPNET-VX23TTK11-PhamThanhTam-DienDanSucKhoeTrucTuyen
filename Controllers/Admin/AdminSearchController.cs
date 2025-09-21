using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/search")]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminSearchController : ControllerBase
    {
        private readonly IAdminSearchService _searchService;

        public AdminSearchController(IAdminSearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> GlobalSearch([FromQuery] string query, [FromQuery] string? type = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var request = new AdminGlobalSearchRequest
            {
                Query = query,
                Type = type,
                Page = page,
                PageSize = pageSize
            };

            var result = await _searchService.GlobalSearchAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> GlobalSearch([FromBody] AdminGlobalSearchRequest request)
        {
            var result = await _searchService.GlobalSearchAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("users")]
        public async Task<IActionResult> SearchUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var request = new AdminUserSearchRequest
            {
                Page = page,
                PageSize = pageSize
            };

            var result = await _searchService.SearchUsersAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("topics")]
        public async Task<IActionResult> SearchTopics([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var request = new AdminTopicSearchRequest
            {
                Page = page,
                PageSize = pageSize
            };

            var result = await _searchService.SearchTopicsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("experts")]
        public async Task<IActionResult> SearchExperts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var request = new AdminExpertSearchRequest
            {
                Page = page,
                PageSize = pageSize
            };

            var result = await _searchService.SearchExpertsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> SearchCategories([FromQuery] string query)
        {
            var result = await _searchService.SearchCategoriesAsync(query);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSearchSuggestions([FromQuery] string query, [FromQuery] int limit = 10)
        {
            var result = await _searchService.GetSearchSuggestionsAsync(query, limit);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
