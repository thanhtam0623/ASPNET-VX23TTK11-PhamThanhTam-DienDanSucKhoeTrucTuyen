using ApiApplication.Models.DTOs.User;
using ApiApplication.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.
    User
{
    [ApiController]
    [Route("api/user/home")]
    public class UserHomeController : ControllerBase
    {
        private readonly IUserHomeService _homeService;

        public UserHomeController(IUserHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHomePage()
        {
            var result = await _homeService.GetHomePageDataAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchTopics([FromBody] SearchRequest request)
        {
            var result = await _homeService.SearchTopicsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _homeService.GetCategoriesAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("pinned-topics")]
        public async Task<IActionResult> GetPinnedTopics()
        {
            var result = await _homeService.GetPinnedTopicsAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("latest-topics")]
        public async Task<IActionResult> GetLatestTopics([FromQuery] int count = 10)
        {
            var result = await _homeService.GetLatestTopicsAsync(count);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("popular-tags")]
        public async Task<IActionResult> GetPopularTags([FromQuery] int count = 20)
        {
            var result = await _homeService.GetPopularTagsAsync(count);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("site-stats")]
        public async Task<IActionResult> GetSiteStats()
        {
            var result = await _homeService.GetSiteStatsAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
