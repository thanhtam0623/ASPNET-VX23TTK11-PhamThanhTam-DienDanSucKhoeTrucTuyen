using ApiApplication.Data;
using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.User
{
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext _context;

        public UserProfileService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<UserProfileDTO>> GetProfileAsync(int userId)
        {
            try
            {
                var user = await _context.UserAccounts
                    .Include(u => u.Topics)
                    .Include(u => u.Posts)
                    .Include(u => u.Likes)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return ApiResponse<UserProfileDTO>.ErrorResult("User not found");
                }

                var profile = new UserProfileDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Avatar = user.Avatar,
                    Bio = user.Bio,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    IsEmailVerified = user.IsEmailVerified,
                    ShowEmail = user.ShowEmail,
                    ShowBio = user.ShowBio,
                    CreatedAt = user.CreatedAt,
                    TopicCount = user.Topics.Count,
                    PostCount = user.Posts.Count,
                    LikeCount = user.Likes.Count
                };

                return ApiResponse<UserProfileDTO>.SuccessResult(profile);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserProfileDTO>.ErrorResult($"Failed to get profile: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserProfileDTO>> GetPublicProfileAsync(string username)
        {
            try
            {
                var user = await _context.UserAccounts
                    .Include(u => u.Topics)
                    .Include(u => u.Posts)
                    .Include(u => u.Likes)
                    .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

                if (user == null)
                {
                    return ApiResponse<UserProfileDTO>.ErrorResult("User not found");
                }

                var profile = new UserProfileDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.ShowEmail ? user.Email : string.Empty,
                    FullName = user.FullName,
                    Avatar = user.Avatar,
                    Bio = user.ShowBio ? user.Bio : null,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    IsEmailVerified = user.IsEmailVerified,
                    ShowEmail = user.ShowEmail,
                    ShowBio = user.ShowBio,
                    CreatedAt = user.CreatedAt,
                    TopicCount = user.Topics.Count,
                    PostCount = user.Posts.Count,
                    LikeCount = user.Likes.Count
                };

                return ApiResponse<UserProfileDTO>.SuccessResult(profile);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserProfileDTO>.ErrorResult($"Failed to get public profile: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserProfileDTO>> UpdateProfileAsync(int userId, UserUpdateProfileRequest request)
        {
            try
            {
                var user = await _context.UserAccounts.FindAsync(userId);
                if (user == null)
                {
                    return ApiResponse<UserProfileDTO>.ErrorResult("User not found");
                }

                user.FullName = request.FullName;
                user.Bio = request.Bio;
                user.Avatar = request.Avatar;
                user.ShowEmail = request.ShowEmail;
                user.ShowBio = request.ShowBio;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return await GetProfileAsync(userId);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserProfileDTO>.ErrorResult($"Failed to update profile: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PaginationResponse<TopicSummaryDTO>>> GetUserTopicsAsync(int userId, PaginationRequest pagination)
        {
            try
            {
                var query = _context.Topics
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.TopicTags)
                    .ThenInclude(tt => tt.Tag)
                    .Where(t => t.UserId == userId && t.IsActive)
                    .AsQueryable();

                // Apply search
                if (!string.IsNullOrEmpty(pagination.Search))
                {
                    query = query.Where(t => t.Title.Contains(pagination.Search) || t.Content.Contains(pagination.Search));
                }

                // Apply sorting
                query = pagination.SortBy?.ToLower() switch
                {
                    "title" => pagination.SortOrder == "asc" ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title),
                    "category" => pagination.SortOrder == "asc" ? query.OrderBy(t => t.Category.Name) : query.OrderByDescending(t => t.Category.Name),
                    "views" => pagination.SortOrder == "asc" ? query.OrderBy(t => t.ViewCount) : query.OrderByDescending(t => t.ViewCount),
                    "likes" => pagination.SortOrder == "asc" ? query.OrderBy(t => t.LikeCount) : query.OrderByDescending(t => t.LikeCount),
                    _ => pagination.SortOrder == "asc" ? query.OrderBy(t => t.CreatedAt) : query.OrderByDescending(t => t.CreatedAt)
                };

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / pagination.PageSize);

                var topics = await query
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .Select(t => new TopicSummaryDTO
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Slug = t.Slug,
                        CategoryName = t.Category.Name,
                        CategorySlug = t.Category.Slug,
                        AuthorName = t.User.FullName,
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

                var result = new PaginationResponse<TopicSummaryDTO>
                {
                    Items = topics,
                    TotalItems = totalCount,
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalPages = totalPages,
                    HasNext = pagination.Page < totalPages,
                    HasPrevious = pagination.Page > 1
                };

                return ApiResponse<PaginationResponse<TopicSummaryDTO>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<TopicSummaryDTO>>.ErrorResult($"Failed to get user topics: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<PostDetailDTO>>> GetUserPostsAsync(int userId, PaginationRequest pagination)
        {
            try
            {
                var query = _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Topic)
                    .Include(p => p.Likes)
                    .Where(p => p.UserId == userId && p.IsActive)
                    .AsQueryable();

                // Apply search
                if (!string.IsNullOrEmpty(pagination.Search))
                {
                    query = query.Where(p => p.Content.Contains(pagination.Search) || p.Topic.Title.Contains(pagination.Search));
                }

                // Apply sorting
                switch (pagination.SortBy?.ToLower())
                {
                    case "topic":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(p => p.Topic.Title) : query.OrderByDescending(p => p.Topic.Title);
                        break;
                    case "like_count":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(p => p.LikeCount) : query.OrderByDescending(p => p.LikeCount);
                        break;
                    default:
                        query = pagination.SortOrder == "asc" ? query.OrderBy(p => p.CreatedAt) : query.OrderByDescending(p => p.CreatedAt);
                        break;
                }

                // Apply pagination
                var posts = await query
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .Select(p => new PostDetailDTO
                    {
                        Id = p.Id,
                        Content = p.Content,
                        Author = new UserSummaryDTO
                        {
                            Id = p.User.Id,
                            Username = p.User.Username,
                            FullName = p.User.FullName,
                            Avatar = p.User.Avatar,
                            Role = p.User.Role,
                            CreatedAt = p.User.CreatedAt,
                            TopicCount = p.User.Topics.Count,
                            PostCount = p.User.Posts.Count
                        },
                        IsAnswer = p.IsAnswer,
                        LikeCount = p.LikeCount,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        Replies = new List<PostDetailDTO>(), // Don't load replies for user posts list
                        IsLikedByCurrentUser = p.Likes.Any(l => l.UserId == userId),
                        CanEdit = true, // User can edit their own posts
                        CanDelete = true, // User can delete their own posts
                        CanMarkAsAnswer = false // Determined in topic context
                    })
                    .ToListAsync();

                return ApiResponse<List<PostDetailDTO>>.SuccessResult(posts);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<PostDetailDTO>>.ErrorResult($"Failed to get user posts: {ex.Message}");
            }
        }
    }
}
