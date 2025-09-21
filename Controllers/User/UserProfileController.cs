using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiApplication.Controllers.User
{
    [ApiController]
    [Route("api/user/profile")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;

        public UserProfileController(IUserProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _profileService.GetProfileAsync(userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetPublicProfile(string username)
        {
            var result = await _profileService.GetPublicProfileAsync(username);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateProfileRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _profileService.UpdateProfileAsync(userId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("topics")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetUserTopics([FromQuery] PaginationRequest pagination)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _profileService.GetUserTopicsAsync(userId, pagination);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{username}/topics")]
        public async Task<IActionResult> GetPublicUserTopics(string username, [FromQuery] PaginationRequest pagination)
        {
            var userResult = await _profileService.GetPublicProfileAsync(username);
            if (!userResult.Success || userResult.Data == null)
            {
                return BadRequest(userResult);
            }

            var result = await _profileService.GetUserTopicsAsync(userResult.Data.Id, pagination);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("posts")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetUserPosts([FromQuery] PaginationRequest pagination)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _profileService.GetUserPostsAsync(userId, pagination);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{username}/posts")]
        public async Task<IActionResult> GetPublicUserPosts(string username, [FromQuery] PaginationRequest pagination)
        {
            // First get the user by username to get their ID
            var userResult = await _profileService.GetPublicProfileAsync(username);
            if (!userResult.Success || userResult.Data == null)
            {
                return BadRequest(userResult);
            }

            var result = await _profileService.GetUserPostsAsync(userResult.Data.Id, pagination);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
