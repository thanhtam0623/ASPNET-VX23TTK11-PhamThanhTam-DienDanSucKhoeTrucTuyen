using ApiApplication.Data;
using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.User
{
    public class UserCategoryService : IUserCategoryService
    {
        private readonly AppDbContext _context;

        public UserCategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CategoryPageDTO>> GetCategoryPageAsync(string categorySlug, CategoryTopicsRequest request)
        {
            try
            {
                var category = await _context.Categories
                    .Where(c => c.Slug == categorySlug && c.IsActive)
                    .Select(c => new CategorySummaryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        Icon = c.Icon,
                        Color = c.Color,
                        TopicCount = c.TopicCount,
                        PostCount = c.PostCount
                    })
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return ApiResponse<CategoryPageDTO>.ErrorResult("Category not found");
                }

                Console.WriteLine($"Found category: ID={category.Id}, Slug={category.Slug}, Name={category.Name}");

                var query = _context.Topics
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.TopicTags)
                    .ThenInclude(tt => tt.Tag)
                    .Where(t => t.CategoryId == category.Id && t.IsActive)
                    .AsQueryable();

                var allTopics = await _context.Topics
                    .Where(t => t.CategoryId == category.Id)
                    .Select(t => new { t.Id, t.Title, t.IsActive, t.CategoryId })
                    .ToListAsync();
                Console.WriteLine($"All topics in category {category.Id}: {string.Join(", ", allTopics.Select(t => $"ID={t.Id}, Title={t.Title}, IsActive={t.IsActive}"))}");

                // Apply tag filters
                if (request.TagIds != null && request.TagIds.Any())
                {
                    query = query.Where(t => t.TopicTags.Any(tt => request.TagIds.Contains(tt.TagId)));
                }

                // Apply filters
                switch (request.Filter?.ToLower())
                {
                    case "answered":
                        query = query.Where(t => t.HasAnswer);
                        break;
                    case "unanswered":
                        query = query.Where(t => !t.HasAnswer);
                        break;
                    case "popular":
                        query = query.Where(t => t.LikeCount > 0 || t.ViewCount > 10);
                        break;
                    default:
                        // all - no additional filter
                        break;
                }

                // Count total items
                var totalItems = await query.CountAsync();

                // Apply sorting
                switch (request.SortBy?.ToLower())
                {
                    case "last_activity":
                        query = query.OrderByDescending(t => t.LastActivityAt ?? t.CreatedAt);
                        break;
                    case "view_count":
                        query = query.OrderByDescending(t => t.ViewCount);
                        break;
                    case "like_count":
                        query = query.OrderByDescending(t => t.LikeCount);
                        break;
                    default:
                        query = query.OrderByDescending(t => t.CreatedAt);
                        break;
                }

                // Apply pagination
                var topics = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(t => new TopicSummaryDTO
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Slug = t.Slug,
                        CategoryName = t.Category.Name,
                        CategorySlug = t.Category.Slug,
                        AuthorName = t.User.Username,
                        AuthorAvatar = t.User.Avatar,
                        AuthorRole = t.User.Role,
                        IsPinned = t.IsPinned,
                        IsLocked = t.IsLocked,
                        HasAnswer = t.HasAnswer,
                        ViewCount = t.ViewCount,
                        LikeCount = t.LikeCount,
                        PostCount = t.PostCount,
                        CreatedAt = t.CreatedAt,
                        LastActivityAt = t.LastActivityAt,
                        Tags = t.TopicTags.Select(tt => tt.Tag.Name).ToList()
                    })
                    .ToListAsync();

                Console.WriteLine($"Topics in category {category.Id}: {string.Join(", ", topics.Select(t => $"ID={t.Id}, Title={t.Title}"))}");

                // Get popular tags for this category
                var popularTags = await _context.Tags
                    .Where(tag => tag.TopicTags.Any(tt => tt.Topic.CategoryId == category.Id))
                    .OrderByDescending(tag => tag.TopicCount)
                    .Take(10)
                    .Select(tag => new TagSummaryDTO
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        Slug = tag.Slug,
                        Color = tag.Color,
                        TopicCount = tag.TopicCount
                    })
                    .ToListAsync();

                var categoryPage = new CategoryPageDTO
                {
                    Category = category,
                    Topics = topics,
                    TotalTopics = totalItems,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalPages = (int)Math.Ceiling((double)totalItems / request.PageSize),
                    PopularTags = popularTags
                };

                Console.WriteLine($"Returning CategoryPageDTO: Topics count={topics.Count}, TotalTopics={totalItems}");

                return ApiResponse<CategoryPageDTO>.SuccessResult(categoryPage);
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryPageDTO>.ErrorResult($"Failed to get category page: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategorySummaryDTO>> GetCategoryBySlugAsync(string slug)
        {
            try
            {
                var category = await _context.Categories
                    .Where(c => c.Slug == slug && c.IsActive)
                    .Select(c => new CategorySummaryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        Icon = c.Icon,
                        Color = c.Color,
                        TopicCount = c.TopicCount,
                        PostCount = c.PostCount,
                        LastActivityAt = c.Topics.OrderByDescending(t => t.LastActivityAt).Select(t => t.LastActivityAt).FirstOrDefault(),
                        LastTopicTitle = c.Topics.OrderByDescending(t => t.CreatedAt).Select(t => t.Title).FirstOrDefault(),
                        LastAuthorName = c.Topics.OrderByDescending(t => t.CreatedAt).Select(t => t.User.Username).FirstOrDefault()
                    })
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    return ApiResponse<CategorySummaryDTO>.ErrorResult("Category not found");
                }

                return ApiResponse<CategorySummaryDTO>.SuccessResult(category);
            }
            catch (Exception ex)
            {
                return ApiResponse<CategorySummaryDTO>.ErrorResult($"Failed to get category: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<CategorySummaryDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.DisplayOrder)
                    .ThenBy(c => c.Name)
                    .Select(c => new CategorySummaryDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        Icon = c.Icon,
                        Color = c.Color,
                        TopicCount = c.TopicCount,
                        PostCount = c.PostCount,
                        LastActivityAt = c.Topics.OrderByDescending(t => t.LastActivityAt).Select(t => t.LastActivityAt).FirstOrDefault(),
                        LastTopicTitle = c.Topics.OrderByDescending(t => t.CreatedAt).Select(t => t.Title).FirstOrDefault(),
                        LastAuthorName = c.Topics.OrderByDescending(t => t.CreatedAt).Select(t => t.User.Username).FirstOrDefault()
                    })
                    .ToListAsync();

                return ApiResponse<List<CategorySummaryDTO>>.SuccessResult(categories);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CategorySummaryDTO>>.ErrorResult($"Failed to get categories: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<TagSummaryDTO>>> GetCategoryTagsAsync(int categoryId)
        {
            try
            {
                var tags = await _context.Tags
                    .Where(tag => tag.TopicTags.Any(tt => tt.Topic.CategoryId == categoryId))
                    .OrderByDescending(tag => tag.TopicCount)
                    .Select(tag => new TagSummaryDTO
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        Slug = tag.Slug,
                        Color = tag.Color,
                        TopicCount = tag.TopicCount
                    })
                    .ToListAsync();

                return ApiResponse<List<TagSummaryDTO>>.SuccessResult(tags);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TagSummaryDTO>>.ErrorResult($"Failed to get category tags: {ex.Message}");
            }
        }
    }
}
