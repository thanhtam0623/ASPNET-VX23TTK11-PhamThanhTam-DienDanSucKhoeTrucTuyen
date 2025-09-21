using ApiApplication.Models.DTOs.User;
using ApiApplication.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.User
{
    [ApiController]
    [Route("api/user/experts")]
    public class UserExpertController : ControllerBase
    {
        private readonly IUserExpertService _expertService;

        public UserExpertController(IUserExpertService expertService)
        {
            _expertService = expertService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchExperts([FromBody] ExpertSearchRequest request)
        {
            var result = await _expertService.GetExpertsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetExperts([FromQuery] ExpertSearchRequest request)
        {
            var result = await _expertService.GetExpertsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetExpertByUsername(string username)
        {
            var result = await _expertService.GetExpertByUsernameAsync(username);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("specialties")]
        public async Task<IActionResult> GetSpecialties()
        {
            var result = await _expertService.GetExpertSpecialtiesAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{expertId}/topics")]
        public async Task<IActionResult> GetExpertTopics(int expertId, 
                                                        [FromQuery] int page = 1, 
                                                        [FromQuery] int pageSize = 20)
        {
            var result = await _expertService.GetExpertTopicsAsync(expertId, page, pageSize);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{expertId}/stats")]
        public async Task<IActionResult> GetExpertStats(int expertId)
        {
            var result = await _expertService.GetExpertStatsAsync(expertId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
