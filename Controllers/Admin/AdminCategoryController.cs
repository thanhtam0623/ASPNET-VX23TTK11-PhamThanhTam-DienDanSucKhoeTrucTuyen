using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/categories")]
    [Authorize(Policy = "AdminPolicy")]
    public class AdminCategoryController : ControllerBase
    {
        private readonly IAdminCategoryService _categoryService;

        public AdminCategoryController(IAdminCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetCategoriesAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] AdminCreateCategoryRequest request)
        {
            var result = await _categoryService.CreateCategoryAsync(request);
            
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetCategory), new { categoryId = result.Data?.Id }, result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] AdminUpdateCategoryRequest request)
        {
            var result = await _categoryService.UpdateCategoryAsync(categoryId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("{categoryId}/toggle-status")]
        public async Task<IActionResult> ToggleCategoryStatus(int categoryId)
        {
            var result = await _categoryService.ToggleCategoryStatusAsync(categoryId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var result = await _categoryService.DeleteCategoryAsync(categoryId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> ReorderCategories([FromBody] List<int> categoryIds)
        {
            var result = await _categoryService.ReorderCategoriesAsync(categoryIds);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        // Tags
        [HttpGet("tags")]
        public async Task<IActionResult> GetTags()
        {
            var result = await _categoryService.GetTagsAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("tags/{tagId}")]
        public async Task<IActionResult> GetTag(int tagId)
        {
            var result = await _categoryService.GetTagByIdAsync(tagId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("tags")]
        public async Task<IActionResult> CreateTag([FromBody] AdminCreateTagRequest request)
        {
            var result = await _categoryService.CreateTagAsync(request);
            
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetTag), new { tagId = result.Data?.Id }, result);
            }
            
            return BadRequest(result);
        }

        [HttpPut("tags/{tagId}")]
        public async Task<IActionResult> UpdateTag(int tagId, [FromBody] AdminUpdateTagRequest request)
        {
            var result = await _categoryService.UpdateTagAsync(tagId, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpDelete("tags/{tagId}")]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            var result = await _categoryService.DeleteTagAsync(tagId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("tags/merge")]
        public async Task<IActionResult> MergeTags([FromBody] AdminMergeTagsRequest request)
        {
            var result = await _categoryService.MergeTagsAsync(request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("tags/recount")]
        public async Task<IActionResult> RecountTags()
        {
            var result = await _categoryService.RecountTagsAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
