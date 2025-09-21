using ApiApplication.Data;
using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ApiApplication.Services.Admin
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly AppDbContext _context;

        public AdminCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<AdminCategoryDTO>>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .OrderBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name)
                    .Select(c => new AdminCategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        Icon = c.Icon,
                        Color = c.Color,
                        DisplayOrder = c.DisplayOrder,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        TopicCount = c.TopicCount,
                        PostCount = c.PostCount
                    })
                    .ToListAsync();

                return ApiResponse<List<AdminCategoryDTO>>.SuccessResult(categories);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AdminCategoryDTO>>.ErrorResult($"Lấy danh sách danh mục thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminCategoryDTO>> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var category = await _context.Categories
                    .Where(c => c.Id == categoryId)
                    .Select(c => new AdminCategoryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        Icon = c.Icon,
                        Color = c.Color,
                        DisplayOrder = c.DisplayOrder,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        TopicCount = c.TopicCount,
                        PostCount = c.PostCount
                    })
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return ApiResponse<AdminCategoryDTO>.ErrorResult("Không tìm thấy danh mục");
                }

                return ApiResponse<AdminCategoryDTO>.SuccessResult(category);
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminCategoryDTO>.ErrorResult($"Lấy thông tin danh mục thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminCategoryDTO>> CreateCategoryAsync(AdminCreateCategoryRequest request)
        {
            try
            {
                var slug = GenerateSlug(request.Name);

                // Check if slug exists
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Slug == slug);

                if (existingCategory != null)
                {
                    return ApiResponse<AdminCategoryDTO>.ErrorResult("Danh mục với tên này đã tồn tại");
                }

                var category = new Category
                {
                    Name = request.Name,
                    Slug = slug,
                    Description = request.Description,
                    Icon = request.Icon,
                    Color = request.Color,
                    DisplayOrder = request.DisplayOrder,
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var result = new AdminCategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Slug = category.Slug,
                    Description = category.Description,
                    Icon = category.Icon,
                    Color = category.Color,
                    DisplayOrder = category.DisplayOrder,
                    IsActive = category.IsActive,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                    TopicCount = 0,
                    PostCount = 0
                };

                return ApiResponse<AdminCategoryDTO>.SuccessResult(result, "Tạo danh mục thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminCategoryDTO>.ErrorResult($"Tạo danh mục thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminCategoryDTO>> UpdateCategoryAsync(int categoryId, AdminUpdateCategoryRequest request)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    return ApiResponse<AdminCategoryDTO>.ErrorResult("Không tìm thấy danh mục");
                }

                var slug = GenerateSlug(request.Name);

                // Check if slug exists (excluding current category)
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Slug == slug && c.Id != categoryId);

                if (existingCategory != null)
                {
                    return ApiResponse<AdminCategoryDTO>.ErrorResult("Danh mục với tên này đã tồn tại");
                }

                category.Name = request.Name;
                category.Slug = slug;
                category.Description = request.Description;
                category.Icon = request.Icon;
                category.Color = request.Color;
                category.DisplayOrder = request.DisplayOrder;
                category.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var result = new AdminCategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Slug = category.Slug,
                    Description = category.Description,
                    Icon = category.Icon,
                    Color = category.Color,
                    DisplayOrder = category.DisplayOrder,
                    IsActive = category.IsActive,
                    CreatedAt = category.CreatedAt,
                    UpdatedAt = category.UpdatedAt,
                    TopicCount = category.TopicCount,
                    PostCount = category.PostCount
                };

                return ApiResponse<AdminCategoryDTO>.SuccessResult(result, "Cập nhật danh mục thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminCategoryDTO>.ErrorResult($"Cập nhật danh mục thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ToggleCategoryStatusAsync(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    return ApiResponse.ErrorResult("Không tìm thấy danh mục");
                }

                category.IsActive = !category.IsActive;
                category.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult($"Danh mục đã được {(category.IsActive ? "kích hoạt" : "vô hiệu hóa")} thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Thay đổi trạng thái danh mục thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    return ApiResponse.ErrorResult("Không tìm thấy danh mục");
                }

                // Check if category has topics
                var hasTopics = await _context.Topics.AnyAsync(t => t.CategoryId == categoryId);
                if (hasTopics)
                {
                    return ApiResponse.ErrorResult("Không thể xóa danh mục đang chứa chủ đề");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Xóa danh mục thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Xóa danh mục thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ReorderCategoriesAsync(List<int> categoryIds)
        {
            try
            {
                for (int i = 0; i < categoryIds.Count; i++)
                {
                    var category = await _context.Categories.FindAsync(categoryIds[i]);
                    if (category != null)
                    {
                        category.DisplayOrder = i + 1;
                        category.UpdatedAt = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Categories reordered successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to reorder categories: {ex.Message}");
            }
        }

        // Tag Management
        public async Task<ApiResponse<List<AdminTagDTO>>> GetTagsAsync()
        {
            try
            {
                var tags = await _context.Tags
                    .OrderByDescending(t => t.TopicCount)
                    .ThenBy(t => t.Name)
                    .Select(t => new AdminTagDTO
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Slug = t.Slug,
                        Color = t.Color,
                        CreatedAt = t.CreatedAt,
                        TopicCount = t.TopicCount
                    })
                    .ToListAsync();

                return ApiResponse<List<AdminTagDTO>>.SuccessResult(tags);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AdminTagDTO>>.ErrorResult($"Failed to get tags: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminTagDTO>> GetTagByIdAsync(int tagId)
        {
            try
            {
                var tag = await _context.Tags
                    .Where(t => t.Id == tagId)
                    .Select(t => new AdminTagDTO
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Slug = t.Slug,
                        Color = t.Color,
                        CreatedAt = t.CreatedAt,
                        TopicCount = t.TopicCount
                    })
                    .FirstOrDefaultAsync();

                if (tag == null)
                {
                    return ApiResponse<AdminTagDTO>.ErrorResult("Tag not found");
                }

                return ApiResponse<AdminTagDTO>.SuccessResult(tag);
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminTagDTO>.ErrorResult($"Failed to get tag: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminTagDTO>> CreateTagAsync(AdminCreateTagRequest request)
        {
            try
            {
                var slug = GenerateSlug(request.Name);

                // Check if slug exists
                var existingTag = await _context.Tags
                    .FirstOrDefaultAsync(t => t.Slug == slug);

                if (existingTag != null)
                {
                    return ApiResponse<AdminTagDTO>.ErrorResult("Tag with this name already exists");
                }

                var tag = new Tag
                {
                    Name = request.Name,
                    Slug = slug,
                    Color = request.Color,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();

                var result = new AdminTagDTO
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Slug = tag.Slug,
                    Color = tag.Color,
                    CreatedAt = tag.CreatedAt,
                    TopicCount = 0
                };

                return ApiResponse<AdminTagDTO>.SuccessResult(result, "Tag created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminTagDTO>.ErrorResult($"Failed to create tag: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminTagDTO>> UpdateTagAsync(int tagId, AdminUpdateTagRequest request)
        {
            try
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag == null)
                {
                    return ApiResponse<AdminTagDTO>.ErrorResult("Tag not found");
                }

                var slug = GenerateSlug(request.Name);

                // Check if slug exists (excluding current tag)
                var existingTag = await _context.Tags
                    .FirstOrDefaultAsync(t => t.Slug == slug && t.Id != tagId);

                if (existingTag != null)
                {
                    return ApiResponse<AdminTagDTO>.ErrorResult("Tag with this name already exists");
                }

                tag.Name = request.Name;
                tag.Slug = slug;
                tag.Color = request.Color;

                await _context.SaveChangesAsync();

                var result = new AdminTagDTO
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Slug = tag.Slug,
                    Color = tag.Color,
                    CreatedAt = tag.CreatedAt,
                    TopicCount = tag.TopicCount
                };

                return ApiResponse<AdminTagDTO>.SuccessResult(result, "Tag updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminTagDTO>.ErrorResult($"Failed to update tag: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteTagAsync(int tagId)
        {
            try
            {
                var tag = await _context.Tags.FindAsync(tagId);
                if (tag == null)
                {
                    return ApiResponse.ErrorResult("Tag not found");
                }

                // Remove all topic-tag associations
                var topicTags = await _context.TopicTags
                    .Where(tt => tt.TagId == tagId)
                    .ToListAsync();

                _context.TopicTags.RemoveRange(topicTags);
                _context.Tags.Remove(tag);

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Tag deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to delete tag: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminTagDTO>> MergeTagsAsync(AdminMergeTagsRequest request)
        {
            try
            {
                var tagsToMerge = await _context.Tags
                    .Where(t => request.TagIds.Contains(t.Id))
                    .ToListAsync();

                if (tagsToMerge.Count < 2)
                {
                    return ApiResponse<AdminTagDTO>.ErrorResult("At least 2 tags are required for merging");
                }

                var slug = GenerateSlug(request.NewTagName);

                // Check if new tag name conflicts
                var existingTag = await _context.Tags
                    .FirstOrDefaultAsync(t => t.Slug == slug && !request.TagIds.Contains(t.Id));

                if (existingTag != null)
                {
                    return ApiResponse<AdminTagDTO>.ErrorResult("Tag with this name already exists");
                }

                // Create new merged tag
                var newTag = new Tag
                {
                    Name = request.NewTagName,
                    Slug = slug,
                    Color = request.Color,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Tags.Add(newTag);
                await _context.SaveChangesAsync();

                // Move all topic associations to new tag
                var topicTags = await _context.TopicTags
                    .Where(tt => request.TagIds.Contains(tt.TagId))
                    .GroupBy(tt => tt.TopicId)
                    .Select(g => g.First())
                    .ToListAsync();

                foreach (var topicTag in topicTags)
                {
                    topicTag.TagId = newTag.Id;
                }

                // Remove duplicate associations
                var duplicates = await _context.TopicTags
                    .Where(tt => request.TagIds.Contains(tt.TagId))
                    .Where(tt => !topicTags.Contains(tt))
                    .ToListAsync();

                _context.TopicTags.RemoveRange(duplicates);

                // Remove old tags
                _context.Tags.RemoveRange(tagsToMerge);

                await _context.SaveChangesAsync();

                // Recount
                await RecountSingleTagAsync(newTag.Id);

                var result = new AdminTagDTO
                {
                    Id = newTag.Id,
                    Name = newTag.Name,
                    Slug = newTag.Slug,
                    Color = newTag.Color,
                    CreatedAt = newTag.CreatedAt,
                    TopicCount = newTag.TopicCount
                };

                return ApiResponse<AdminTagDTO>.SuccessResult(result, "Tags merged successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminTagDTO>.ErrorResult($"Failed to merge tags: {ex.Message}");
            }
        }

        public async Task<ApiResponse> RecountTagsAsync()
        {
            try
            {
                var tags = await _context.Tags.ToListAsync();

                foreach (var tag in tags)
                {
                    tag.TopicCount = await _context.TopicTags
                        .CountAsync(tt => tt.TagId == tag.Id);
                }

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Tag counts updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to recount tags: {ex.Message}");
            }
        }

        public async Task RecountSingleTagAsync(int tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag != null)
            {
                tag.TopicCount = await _context.TopicTags.CountAsync(tt => tt.TagId == tagId);
                await _context.SaveChangesAsync();
            }
        }

        private string GenerateSlug(string text)
        {
            // Convert to lowercase and replace spaces with hyphens
            var slug = text.ToLowerInvariant().Replace(" ", "-");

            // Remove special characters
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

            // Remove multiple consecutive hyphens
            slug = Regex.Replace(slug, @"-+", "-");

            // Remove leading and trailing hyphens
            slug = slug.Trim('-');

            return slug;
        }
    }
}
