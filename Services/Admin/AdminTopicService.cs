using ApiApplication.Data;
using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.Admin
{
    public class AdminTopicService : IAdminTopicService
    {
        private readonly AppDbContext _context;

        public AdminTopicService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginationResponse<AdminTopicDTO>>> GetTopicsAsync(PaginationRequest pagination, AdminTopicFilterRequest? filter = null)
        {
            try
            {
                var query = _context.Topics
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.TopicTags)
                    .ThenInclude(tt => tt.Tag)
                    .AsQueryable();

                // Apply filters
                if (filter != null)
                {
                    if (filter.CategoryId.HasValue)
                        query = query.Where(t => t.CategoryId == filter.CategoryId.Value);

                    if (filter.UserId.HasValue)
                        query = query.Where(t => t.UserId == filter.UserId.Value);

                    if (filter.IsActive.HasValue)
                        query = query.Where(t => t.IsActive == filter.IsActive.Value);

                    if (filter.IsPinned.HasValue)
                        query = query.Where(t => t.IsPinned == filter.IsPinned.Value);

                    if (filter.IsLocked.HasValue)
                        query = query.Where(t => t.IsLocked == filter.IsLocked.Value);

                    if (filter.HasAnswer.HasValue)
                        query = query.Where(t => t.HasAnswer == filter.HasAnswer.Value);

                    if (filter.CreatedFrom.HasValue)
                        query = query.Where(t => t.CreatedAt >= filter.CreatedFrom.Value);

                    if (filter.CreatedTo.HasValue)
                        query = query.Where(t => t.CreatedAt <= filter.CreatedTo.Value);

                    if (!string.IsNullOrEmpty(filter.Author))
                        query = query.Where(t => t.User.Username.Contains(filter.Author) || t.User.FullName.Contains(filter.Author));

                }

                // Count total items
                var totalItems = await query.CountAsync();

                // Apply sorting
                switch (pagination.SortBy?.ToLower())
                {
                    case "title":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title);
                        break;
                    case "category":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(t => t.Category.Name) : query.OrderByDescending(t => t.Category.Name);
                        break;
                    case "author":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(t => t.User.Username) : query.OrderByDescending(t => t.User.Username);
                        break;
                    case "view_count":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(t => t.ViewCount) : query.OrderByDescending(t => t.ViewCount);
                        break;
                    case "like_count":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(t => t.LikeCount) : query.OrderByDescending(t => t.LikeCount);
                        break;
                    case "post_count":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(t => t.PostCount) : query.OrderByDescending(t => t.PostCount);
                        break;
                    default:
                        query = pagination.SortOrder == "asc" ? query.OrderBy(t => t.CreatedAt) : query.OrderByDescending(t => t.CreatedAt);
                        break;
                }

                // Apply pagination
                var topics = await query
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .Select(t => new AdminTopicDTO
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Slug = t.Slug,
                        Content = t.Content,
                        CategoryName = t.Category.Name,
                        AuthorName = t.User.Username,
                        IsActive = t.IsActive,
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

                var response = new PaginationResponse<AdminTopicDTO>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize),
                    HasNext = pagination.Page * pagination.PageSize < totalItems,
                    HasPrevious = pagination.Page > 1,
                    Items = topics
                };

                return ApiResponse<PaginationResponse<AdminTopicDTO>>.SuccessResult(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<AdminTopicDTO>>.ErrorResult($"Failed to get topics: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminTopicDTO>> GetTopicByIdAsync(int topicId)
        {
            try
            {
                var topic = await _context.Topics
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.TopicTags)
                    .ThenInclude(tt => tt.Tag)
                    .Where(t => t.Id == topicId)
                    .Select(t => new AdminTopicDTO
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Slug = t.Slug,
                        Content = t.Content,
                        CategoryName = t.Category.Name,
                        AuthorName = t.User.Username,
                        IsActive = t.IsActive,
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
                    .FirstOrDefaultAsync();

                if (topic == null)
                {
                    return ApiResponse<AdminTopicDTO>.ErrorResult("Không tìm thấy chủ đề");
                }

                return ApiResponse<AdminTopicDTO>.SuccessResult(topic);
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminTopicDTO>.ErrorResult($"Lấy thông tin chủ đề thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminTopicStatsDTO>> GetTopicStatsAsync()
        {
            try
            {
                var stats = new AdminTopicStatsDTO
                {
                    TotalTopics = await _context.Topics.CountAsync(),
                    ActiveTopics = await _context.Topics.CountAsync(t => t.IsActive),
                    PinnedTopics = await _context.Topics.CountAsync(t => t.IsPinned),
                    LockedTopics = await _context.Topics.CountAsync(t => t.IsLocked),
                    TopicsWithAnswers = await _context.Topics.CountAsync(t => t.HasAnswer),
                    NewTopicsToday = await _context.Topics.CountAsync(t => t.CreatedAt.Date == DateTime.UtcNow.Date),
                    NewTopicsThisWeek = await _context.Topics.CountAsync(t => t.CreatedAt >= DateTime.UtcNow.AddDays(-7)),
                    NewTopicsThisMonth = await _context.Topics.CountAsync(t => t.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                };

                return ApiResponse<AdminTopicStatsDTO>.SuccessResult(stats);
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminTopicStatsDTO>.ErrorResult($"Failed to get topic stats: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminTopicDTO>> UpdateTopicAsync(int topicId, AdminUpdateTopicRequest request)
        {
            try
            {
                var topic = await _context.Topics
                    .Include(t => t.TopicTags)
                    .ThenInclude(tt => tt.Tag)
                    .FirstOrDefaultAsync(t => t.Id == topicId);

                if (topic == null)
                {
                    return ApiResponse<AdminTopicDTO>.ErrorResult("Không tìm thấy chủ đề");
                }

                topic.Title = request.Title;
                topic.Content = request.Content;
                topic.CategoryId = request.CategoryId;
                topic.UpdatedAt = DateTime.UtcNow;

                // Update tags
                _context.TopicTags.RemoveRange(topic.TopicTags);
                foreach (var tagName in request.Tags)
                {
                    var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                    if (tag == null)
                    {
                        tag = new Models.Entities.Tag
                        {
                            Name = tagName,
                            Slug = tagName.ToLower().Replace(" ", "-"),
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.Tags.Add(tag);
                    }

                    topic.TopicTags.Add(new Models.Entities.TopicTag
                    {
                        TopicId = topic.Id,
                        Tag = tag
                    });
                }

                await _context.SaveChangesAsync();

                var result = new AdminTopicDTO
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    Content = topic.Content,
                    CategoryName = topic.Category?.Name ?? "",
                    AuthorName = topic.User?.Username ?? "",
                    IsActive = topic.IsActive,
                    IsPinned = topic.IsPinned,
                    IsLocked = topic.IsLocked,
                    HasAnswer = topic.HasAnswer,
                    ViewCount = topic.ViewCount,
                    LikeCount = topic.LikeCount,
                    PostCount = topic.PostCount,
                    CreatedAt = topic.CreatedAt,
                    UpdatedAt = topic.UpdatedAt,
                    LastActivityAt = topic.LastActivityAt,
                    Tags = topic.TopicTags.Select(tt => tt.Tag.Name).ToList()
                };

                return ApiResponse<AdminTopicDTO>.SuccessResult(result, "Cập nhật chủ đề thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminTopicDTO>.ErrorResult($"Cập nhật chủ đề thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ToggleTopicPinAsync(int topicId)
        {
            try
            {
                var topic = await _context.Topics.FindAsync(topicId);
                if (topic == null)
                {
                    return ApiResponse.ErrorResult("Không tìm thấy chủ đề");
                }

                topic.IsPinned = !topic.IsPinned;
                topic.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult($"Chủ đề đã được {(topic.IsPinned ? "ghim" : "bỏ ghim")} thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Thay đổi trạng thái ghim thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateTopicStatusAsync(int topicId, AdminUpdateTopicStatusRequest request)
        {
            try
            {
                var topic = await _context.Topics.FindAsync(topicId);
                if (topic == null)
                {
                    return ApiResponse.ErrorResult("Không tìm thấy chủ đề");
                }

                if (request.IsActive.HasValue)
                    topic.IsActive = request.IsActive.Value;

                if (request.IsPinned.HasValue)
                    topic.IsPinned = request.IsPinned.Value;

                if (request.IsLocked.HasValue)
                    topic.IsLocked = request.IsLocked.Value;

                topic.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Cập nhật trạng thái chủ đề thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Cập nhật trạng thái chủ đề thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteTopicAsync(int topicId)
        {
            try
            {
                var topic = await _context.Topics.FindAsync(topicId);
                if (topic == null)
                {
                    return ApiResponse.ErrorResult("Không tìm thấy chủ đề");
                }

                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Xóa chủ đề thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Xóa chủ đề thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PaginationResponse<AdminPostDTO>>> GetPostsAsync(PaginationRequest pagination, int? topicId = null)
        {
            try
            {
                var query = _context.Posts
                    .Include(p => p.Topic)
                    .Include(p => p.User)
                    .AsQueryable();

                if (topicId.HasValue)
                {
                    query = query.Where(p => p.TopicId == topicId.Value);
                }

                // Apply search
                if (!string.IsNullOrEmpty(pagination.Search))
                {
                    query = query.Where(p => p.Content.Contains(pagination.Search) ||
                                           p.User.Username.Contains(pagination.Search) ||
                                           p.Topic.Title.Contains(pagination.Search));
                }

                // Count total items
                var totalItems = await query.CountAsync();

                // Apply sorting
                switch (pagination.SortBy?.ToLower())
                {
                    case "author":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(p => p.User.Username) : query.OrderByDescending(p => p.User.Username);
                        break;
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
                    .Select(p => new AdminPostDTO
                    {
                        Id = p.Id,
                        Content = p.Content,
                        TopicId = p.TopicId,
                        TopicTitle = p.Topic.Title,
                        AuthorName = p.User.Username,
                        IsActive = p.IsActive,
                        IsAnswer = p.IsAnswer,
                        LikeCount = p.LikeCount,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    })
                    .ToListAsync();

                var response = new PaginationResponse<AdminPostDTO>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize),
                    HasNext = pagination.Page * pagination.PageSize < totalItems,
                    HasPrevious = pagination.Page > 1,
                    Items = posts
                };

                return ApiResponse<PaginationResponse<AdminPostDTO>>.SuccessResult(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<AdminPostDTO>>.ErrorResult($"Failed to get posts: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminPostDTO>> GetPostByIdAsync(int postId)
        {
            try
            {
                var post = await _context.Posts
                    .Include(p => p.Topic)
                    .Include(p => p.User)
                    .Where(p => p.Id == postId)
                    .Select(p => new AdminPostDTO
                    {
                        Id = p.Id,
                        Content = p.Content,
                        TopicId = p.TopicId,
                        TopicTitle = p.Topic.Title,
                        AuthorName = p.User.Username,
                        IsActive = p.IsActive,
                        IsAnswer = p.IsAnswer,
                        LikeCount = p.LikeCount,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                if (post == null)
                {
                    return ApiResponse<AdminPostDTO>.ErrorResult("Không tìm thấy bài viết");
                }

                return ApiResponse<AdminPostDTO>.SuccessResult(post);
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminPostDTO>.ErrorResult($"Lấy thông tin bài viết thất bại: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeletePostAsync(int postId)
        {
            try
            {
                var post = await _context.Posts.FindAsync(postId);
                if (post == null)
                {
                    return ApiResponse.ErrorResult("Post not found");
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Post deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to delete post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> MarkPostAsAnswerAsync(int postId)
        {
            try
            {
                var post = await _context.Posts
                    .Include(p => p.Topic)
                    .FirstOrDefaultAsync(p => p.Id == postId);

                if (post == null)
                {
                    return ApiResponse.ErrorResult("Post not found");
                }

                // Unmark other answers in the same topic
                var otherAnswers = await _context.Posts
                    .Where(p => p.TopicId == post.TopicId && p.Id != postId && p.IsAnswer)
                    .ToListAsync();

                foreach (var answer in otherAnswers)
                {
                    answer.IsAnswer = false;
                }

                // Mark this post as answer
                post.IsAnswer = true;
                post.UpdatedAt = DateTime.UtcNow;

                // Update topic has answer status
                post.Topic.HasAnswer = true;
                post.Topic.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Post marked as answer successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to mark post as answer: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PaginationResponse<AdminReportDTO>>> GetReportsAsync(AdminReportsRequest request)
        {
            try
            {
                var query = _context.Reports
                    .Include(r => r.User)
                    .Include(r => r.Topic)
                    .Include(r => r.Post)
                    .AsQueryable();

                // Apply status filter
                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(r => r.Status == request.Status);
                }

                // Apply category filter
                if (!string.IsNullOrEmpty(request.Category))
                {
                    query = query.Where(r => r.Category == request.Category);
                }

                // Apply search
                if (!string.IsNullOrEmpty(request.Search))
                {
                    query = query.Where(r => r.Category.Contains(request.Search) ||
                                           r.Reason.Contains(request.Search) ||
                                           r.User.Username.Contains(request.Search));
                }

                // Count total items
                var totalItems = await query.CountAsync();

                // Apply sorting
                switch (request.SortBy?.ToLower())
                {
                    case "category":
                        query = request.SortOrder == "asc" ? query.OrderBy(r => r.Category) : query.OrderByDescending(r => r.Category);
                        break;
                    case "status":
                        query = request.SortOrder == "asc" ? query.OrderBy(r => r.Status) : query.OrderByDescending(r => r.Status);
                        break;
                    case "user":
                        query = request.SortOrder == "asc" ? query.OrderBy(r => r.User.Username) : query.OrderByDescending(r => r.User.Username);
                        break;
                    default:
                        query = request.SortOrder == "asc" ? query.OrderBy(r => r.CreatedAt) : query.OrderByDescending(r => r.CreatedAt);
                        break;
                }

                // Apply pagination
                var reports = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(r => new AdminReportDTO
                    {
                        Id = r.Id,
                        UserName = r.User.Username,
                        TopicTitle = r.Topic != null ? r.Topic.Title : null,
                        PostContent = r.Post != null ? r.Post.Content.Substring(0, Math.Min(100, r.Post.Content.Length)) : null,
                        Category = r.Category,
                        Reason = r.Reason,
                        Status = r.Status,
                        CreatedAt = r.CreatedAt,
                        ReviewedAt = r.ReviewedAt
                    })
                    .ToListAsync();

                var response = new PaginationResponse<AdminReportDTO>
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / request.PageSize),
                    HasNext = request.Page * request.PageSize < totalItems,
                    HasPrevious = request.Page > 1,
                    Items = reports
                };

                return ApiResponse<PaginationResponse<AdminReportDTO>>.SuccessResult(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<AdminReportDTO>>.ErrorResult($"Failed to get reports: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateReportStatusAsync(int reportId, AdminUpdateReportStatusRequest request)
        {
            try
            {
                var report = await _context.Reports.FindAsync(reportId);
                if (report == null)
                {
                    return ApiResponse.ErrorResult("Report not found");
                }

                report.Status = request.Status;
                report.ReviewedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Report status updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to update report status: {ex.Message}");
            }
        }
    }
}
