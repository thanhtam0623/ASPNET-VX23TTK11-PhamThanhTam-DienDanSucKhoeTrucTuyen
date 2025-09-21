using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/experts")]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminExpertController : ControllerBase
    {
        private readonly IAdminExpertService _expertService;

        public AdminExpertController(IAdminExpertService expertService)
        {
            _expertService = expertService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExperts([FromQuery] PaginationRequest pagination, [FromQuery] AdminExpertFilterRequest? filter = null)
        {
            var result = await _expertService.GetExpertsAsync(pagination, filter);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchExperts([FromBody] AdminExpertSearchRequest request)
        {
            var pagination = new PaginationRequest
            {
                Page = request.Page,
                PageSize = request.PageSize
            };
            
            var filter = new AdminExpertFilterRequest
            {
                Specialty = request.Specialty,
                MinRating = request.MinRating,
                IsOnline = request.IsOnline,
                IsVerified = request.IsVerified,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo
            };
            
            var result = await _expertService.GetExpertsAsync(pagination, filter);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{expertId}")]
        public async Task<IActionResult> GetExpert(int expertId)
        {
            var result = await _expertService.GetExpertByIdAsync(expertId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetExpertStats()
        {
            var result = await _expertService.GetExpertStatsAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("{expertId}/verify")]
        public async Task<IActionResult> VerifyExpert(int expertId, [FromBody] AdminVerifyExpertRequest request)
        {
            var result = await _expertService.VerifyExpertAsync(expertId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("{expertId}/toggle-status")]
        public async Task<IActionResult> ToggleExpertStatus(int expertId)
        {
            var result = await _expertService.ToggleExpertStatusAsync(expertId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("{expertId}/specialty")]
        public async Task<IActionResult> UpdateExpertSpecialty(int expertId, [FromBody] AdminUpdateExpertSpecialtyRequest request)
        {
            var result = await _expertService.UpdateExpertSpecialtyAsync(expertId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("specialties")]
        public async Task<IActionResult> GetSpecialties()
        {
            var result = await _expertService.GetSpecialtiesAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("specialties")]
        public async Task<IActionResult> CreateSpecialty([FromBody] AdminCreateSpecialtyRequest request)
        {
            var result = await _expertService.CreateSpecialtyAsync(request);
            
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetSpecialties), result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("specialties/{specialtyId}")]
        public async Task<IActionResult> UpdateSpecialty(int specialtyId, [FromBody] AdminUpdateSpecialtyRequest request)
        {
            var result = await _expertService.UpdateSpecialtyAsync(specialtyId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("specialties/{specialtyId}")]
        public async Task<IActionResult> DeleteSpecialty(int specialtyId)
        {
            var result = await _expertService.DeleteSpecialtyAsync(specialtyId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{expertId}/reviews")]
        public async Task<IActionResult> GetExpertReviews(int expertId, [FromQuery] PaginationRequest pagination)
        {
            var result = await _expertService.GetExpertReviewsAsync(expertId, pagination);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("reviews/{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var result = await _expertService.DeleteReviewAsync(reviewId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
