using ApiApplication.Models.DTOs.User;
using ApiApplication.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers.User
{
    [ApiController]
    [Route("api/user/categories")]
    public class UserCategoryController : ControllerBase
    {
        private readonly IUserCategoryService _categoryService;

        public UserCategoryController(IUserCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetCategoryBySlug(string slug)
        {
            var result = await _categoryService.GetCategoryBySlugAsync(slug);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpPost("{slug}/topics")]
        public async Task<IActionResult> GetCategoryPage(string slug, [FromBody] CategoryTopicsRequest request)
        {
            var result = await _categoryService.GetCategoryPageAsync(slug, request);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }

        [HttpGet("{categoryId}/tags")]
        public async Task<IActionResult> GetCategoryTags(int categoryId)
        {
            var result = await _categoryService.GetCategoryTagsAsync(categoryId);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return BadRequest(result);
        }
    }
}
