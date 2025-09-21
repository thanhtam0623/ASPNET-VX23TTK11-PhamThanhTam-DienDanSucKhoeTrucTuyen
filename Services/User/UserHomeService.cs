using ApiApplication.Data;
using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.User
{
    public class UserHomeService : IUserHomeService
    {
        private readonly AppDbContext _context;

        public UserHomeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<HomePageDTO>> GetHomePageDataAsync()
        {
            try
            {
                var categories = await GetCategoriesAsync();
                var pinnedTopics = await GetPinnedTopicsAsync();
                var latestTopics = await GetLatestTopicsAsync();
                var popularTags = await GetPopularTagsAsync();
                var siteStats = await GetSiteStatsAsync();

                var homePage = new HomePageDTO
                {
                    Categories = categories.Data ?? new List<CategorySummaryDTO>(),
                    PinnedTopics = pinnedTopics.Data ?? new List<TopicSummaryDTO>(),
                    LatestTopics = latestTopics.Data ?? new List<TopicSummaryDTO>(),
                    PopularTags = popularTags.Data ?? new List<TagSummaryDTO>(),
                    SiteStats = siteStats.Data ?? new SiteStatsDTO()
                };

                return ApiResponse<HomePageDTO>.SuccessResult(homePage);
            }
            catch (Exception ex)
            {
                return ApiResponse<HomePageDTO>.ErrorResult($"Failed to get homepage data: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PaginationResponse<TopicSummaryDTO>>> SearchTopicsAsync(SearchRequest request)
        {
            try
            {
                var query = _context.Topics
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.TopicTags)
                    .ThenInclude(tt => tt.Tag)
                    .Where(t => t.IsActive)
                    .AsQueryable();

                // Apply search filters
                if (!string.IsNullOrEmpty(request.Query))
                {
                    query = query.Where(t => t.Title.Contains(request.Query) || t.Content.Contains(request.Query));
                }

                if (request.CategoryId.HasValue)
                {
                    query = query.Where(t => t.CategoryId == request.CategoryId.Value);
                }

                if (request.TagIds != null && request.TagIds.Any())
                {
                    query = query.Where(t => t.TopicTags.Any(tt => request.TagIds.Contains(tt.TagId)));
                }

                if (request.HasAnswer.HasValue)
                {
                    query = query.Where(t => t.HasAnswer == request.HasAnswer.Value);
                }

                // Count total items
                var totalItems = await query.CountAsync();

                // Apply sorting
                switch (request.SortBy?.ToLower())
                {
                    case "updated_at":
                        query = request.SortOrder == "asc" ? query.OrderBy(t => t.UpdatedAt) : query.OrderByDescending(t => t.UpdatedAt);
                        break;
                    case "view_count":
                        query = request.SortOrder == "asc" ? query.OrderBy(t => t.ViewCount) : query.OrderByDescending(t => t.ViewCount);
                        break;
                    case "like_count":
                        query = request.SortOrder == "asc" ? query.OrderBy(t => t.LikeCount) : query.OrderByDescending(t => t.LikeCount);
                        break;
                    default:
                        query = request.SortOrder == "asc" ? query.OrderBy(t => t.CreatedAt) : query.OrderByDescending(t => t.CreatedAt);
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
                        UpdatedAt = t.UpdatedAt,
                        LastActivityAt = t.LastActivityAt,
                        Tags = t.TopicTags.Select(tt => tt.Tag.Name).ToList()
                    })
                    .ToListAsync();

                var response = new PaginationResponse<TopicSummaryDTO>
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / request.PageSize),
                    HasNext = request.Page * request.PageSize < totalItems,
                    HasPrevious = request.Page > 1,
                    Items = topics
                };

                return ApiResponse<PaginationResponse<TopicSummaryDTO>>.SuccessResult(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<TopicSummaryDTO>>.ErrorResult($"Failed to search topics: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<CategorySummaryDTO>>> GetCategoriesAsync()
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

        public async Task<ApiResponse<List<TopicSummaryDTO>>> GetPinnedTopicsAsync()
        {
            try
            {
                var topics = await _context.Topics
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.TopicTags)
                    .ThenInclude(tt => tt.Tag)
                    .Where(t => t.IsActive && t.IsPinned)
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(5)
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
                        UpdatedAt = t.UpdatedAt,
                        LastActivityAt = t.LastActivityAt,
                        Tags = t.TopicTags.Select(tt => tt.Tag.Name).ToList()
                    })
                    .ToListAsync();

                return ApiResponse<List<TopicSummaryDTO>>.SuccessResult(topics);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TopicSummaryDTO>>.ErrorResult($"Failed to get pinned topics: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<TopicSummaryDTO>>> GetLatestTopicsAsync(int count = 10)
        {
            try
            {
                var topics = await _context.Topics
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.TopicTags)
                    .ThenInclude(tt => tt.Tag)
                    .Where(t => t.IsActive)
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(count)
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
                        UpdatedAt = t.UpdatedAt,
                        LastActivityAt = t.LastActivityAt,
                        Tags = t.TopicTags.Select(tt => tt.Tag.Name).ToList()
                    })
                    .ToListAsync();

                return ApiResponse<List<TopicSummaryDTO>>.SuccessResult(topics);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TopicSummaryDTO>>.ErrorResult($"Failed to get latest topics: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<TagSummaryDTO>>> GetPopularTagsAsync(int count = 20)
        {
            try
            {
                var tags = await _context.Tags
                    .OrderByDescending(t => t.TopicCount)
                    .Take(count)
                    .Select(t => new TagSummaryDTO
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Slug = t.Slug,
                        Color = t.Color,
                        TopicCount = t.TopicCount
                    })
                    .ToListAsync();

                return ApiResponse<List<TagSummaryDTO>>.SuccessResult(tags);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TagSummaryDTO>>.ErrorResult($"Failed to get popular tags: {ex.Message}");
            }
        }

        public async Task<ApiResponse<SiteStatsDTO>> GetSiteStatsAsync()
        {
            try
            {
                var stats = new SiteStatsDTO
                {
                    TotalTopics = await _context.Topics.CountAsync(t => t.IsActive),
                    TotalPosts = await _context.Posts.CountAsync(p => p.IsActive),
                    TotalUsers = await _context.UserAccounts.CountAsync(u => u.IsActive),
                    OnlineUsers = 0 // TODO: Implement online user tracking
                };

                return ApiResponse<SiteStatsDTO>.SuccessResult(stats);
            }
            catch (Exception ex)
            {
                return ApiResponse<SiteStatsDTO>.ErrorResult($"Failed to get site stats: {ex.Message}");
            }
        }
    }
}
