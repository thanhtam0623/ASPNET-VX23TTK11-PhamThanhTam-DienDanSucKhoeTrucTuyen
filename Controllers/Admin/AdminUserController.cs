using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _userService;

        public AdminUserController(IAdminUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationRequest pagination, [FromQuery] AdminUserFilterRequest? filter = null)
        {
            var result = await _userService.GetUsersAsync(pagination, filter);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchUsers([FromBody] AdminUserSearchRequest request)
        {
            var pagination = new PaginationRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            };
            
            var filter = new AdminUserFilterRequest
            {
                Role = request.Role,
                Status = request.Status,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo
            };
            
            var result = await _userService.GetUsersAsync(pagination, filter);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetUserStats()
        {
            var result = await _userService.GetUserStatsAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] AdminCreateUserRequest request)
        {
            var result = await _userService.CreateUserAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] AdminUpdateUserRequest request)
        {
            var result = await _userService.UpdateUserAsync(userId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("{userId}/role")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] AdminUpdateUserRoleRequest request)
        {
            var result = await _userService.UpdateUserRoleAsync(userId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("{userId}/toggle-status")]
        [HttpPut("{userId}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(int userId)
        {
            var result = await _userService.ToggleUserStatusAsync(userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("{userId}/reset-password")]
        public async Task<IActionResult> ResetUserPassword(int userId, [FromBody] AdminResetPasswordRequest request)
        {
            request.UserId = userId;
            var result = await _userService.ResetUserPasswordAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
