using ApiApplication.Data;
using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Models.Entities;
using ApiApplication.Services.Common;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ApiApplication.Services.User
{
    public class UserTopicService : IUserTopicService
    {
        private readonly AppDbContext _context;
        private readonly IHtmlSanitizerService _htmlSanitizer;

        public UserTopicService(AppDbContext context, IHtmlSanitizerService htmlSanitizer)
        {
            _context = context;
            _htmlSanitizer = htmlSanitizer;
        }

        public async Task<ApiResponse<TopicDetailDTO>> GetTopicByIdAsync(int topicId, int? currentUserId = null)
        {
            return await GetTopicByIdInternalAsync(topicId, currentUserId);
        }

        public async Task<ApiResponse<TopicDetailDTO>> GetTopicBySlugAsync(string slug, int? currentUserId = null)
        {
            try
            {
                var topic = await _context.Topics
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.TopicTags)
                    .ThenInclude(tt => tt.Tag)
                    .Where(t => t.Slug == slug && t.IsActive)
                    .FirstOrDefaultAsync();

                if (topic == null)
                {
                    return ApiResponse<TopicDetailDTO>.ErrorResult("Topic not found");
                }

                // Check if current user has liked this topic (separate query for accuracy)
                var isLikedByCurrentUser = currentUserId.HasValue && 
                    await _context.Likes.AnyAsync(l => l.TopicId == topic.Id && l.UserId == currentUserId.Value);

                var canEdit = currentUserId.HasValue && 
                    (topic.UserId == currentUserId.Value || await IsModeratorOrAdminAsync(currentUserId.Value));

                var canDelete = currentUserId.HasValue && 
                    (topic.UserId == currentUserId.Value || await IsModeratorOrAdminAsync(currentUserId.Value));

                var topicDetail = new TopicDetailDTO
                {
                    Id = topic.Id,
                    Title = topic.Title,
                    Slug = topic.Slug,
                    Content = topic.Content,
                    Category = new CategorySummaryDTO
                    {
                        Id = topic.Category.Id,
                        Name = topic.Category.Name,
                        Slug = topic.Category.Slug,
                        Description = topic.Category.Description,
                        Icon = topic.Category.Icon,
                        Color = topic.Category.Color
                    },
                    Author = new UserSummaryDTO
                    {
                        Id = topic.User.Id,
                        Username = topic.User.Username,
                        FullName = topic.User.FullName,
                        Avatar = topic.User.Avatar,
                        Role = topic.User.Role,
                        CreatedAt = topic.User.CreatedAt,
                        TopicCount = topic.User.Topics.Count,
                        PostCount = topic.User.Posts.Count
                    },
                    IsLocked = topic.IsLocked,
                    HasAnswer = topic.HasAnswer,
                    ViewCount = topic.ViewCount,
                    LikeCount = topic.LikeCount,
                    PostCount = topic.PostCount,
                    CreatedAt = topic.CreatedAt,
                    UpdatedAt = topic.UpdatedAt,
                    Tags = topic.TopicTags.Select(tt => new TagSummaryDTO
                    {
                        Id = tt.Tag.Id,
                        Name = tt.Tag.Name,
                        Slug = tt.Tag.Slug,
                        Color = tt.Tag.Color
                    }).ToList(),
                    IsLikedByCurrentUser = isLikedByCurrentUser,
                    CanEdit = canEdit,
                    CanDelete = canDelete
                };

                return ApiResponse<TopicDetailDTO>.SuccessResult(topicDetail);
            }
            catch (Exception ex)
            {
                return ApiResponse<TopicDetailDTO>.ErrorResult($"Failed to get topic: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PaginationResponse<PostDetailDTO>>> GetTopicPostsAsync(int topicId, int? currentUserId = null, int page = 1, int pageSize = 20)
        {
            try
            {
                var query = _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Likes)
                    .Include(p => p.Replies)
                    .ThenInclude(r => r.User)
                    .Where(p => p.TopicId == topicId && p.IsActive && p.ParentPostId == null)
                    .OrderBy(p => p.CreatedAt);

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var posts = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var postDetails = posts.Select(p => MapToPostDetailDTO(p, currentUserId)).ToList();

                var result = new PaginationResponse<PostDetailDTO>
                {
                    Items = postDetails,
                    TotalItems = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    HasNext = page < totalPages,
                    HasPrevious = page > 1
                };

                return ApiResponse<PaginationResponse<PostDetailDTO>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<PostDetailDTO>>.ErrorResult($"Failed to get topic posts: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TopicDetailDTO>> CreateTopicAsync(int userId, CreateTopicRequest request)
        {
            try
            {
                // Check if category exists
                var category = await _context.Categories.FindAsync(request.CategoryId);
                if (category == null || !category.IsActive)
                {
                    return ApiResponse<TopicDetailDTO>.ErrorResult("Category not found");
                }

                var slug = await GenerateUniqueTopicSlugAsync(request.Title);

                // Sanitize user input to prevent XSS
                var sanitizedTitle = _htmlSanitizer.SanitizeTitle(request.Title);
                var sanitizedContent = _htmlSanitizer.Sanitize(request.Content);

                var topic = new Topic
                {
                    Title = sanitizedTitle,
                    Slug = slug,
                    Content = sanitizedContent,
                    CategoryId = request.CategoryId,
                    UserId = userId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Topics.Add(topic);
                await _context.SaveChangesAsync();

                // Add tags if provided
                if (request.Tags != null && request.Tags.Any())
                {
                    await AddTagsToTopicAsync(topic.Id, request.Tags);
                }

                // Update category counts
                await UpdateCategoryCountsAsync(request.CategoryId);

                return await GetTopicByIdInternalAsync(topic.Id, userId);
            }
            catch (Exception ex)
            {
                return ApiResponse<TopicDetailDTO>.ErrorResult($"Failed to create topic: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TopicDetailDTO>> UpdateTopicAsync(int topicId, int userId, UpdateTopicRequest request)
        {
            try
            {
                var topic = await _context.Topics
                    .Include(t => t.TopicTags)
                    .FirstOrDefaultAsync(t => t.Id == topicId);

                if (topic == null)
                {
                    return ApiResponse<TopicDetailDTO>.ErrorResult("Topic not found");
                }

                // Check permissions
                if (topic.UserId != userId && !await IsModeratorOrAdminAsync(userId))
                {
                    return ApiResponse<TopicDetailDTO>.ErrorResult("You don't have permission to edit this topic");
                }

                // Check if category exists
                var category = await _context.Categories.FindAsync(request.CategoryId);
                if (category == null || !category.IsActive)
                {
                    return ApiResponse<TopicDetailDTO>.ErrorResult("Category not found");
                }

                var oldCategoryId = topic.CategoryId;
                var slug = topic.Title != request.Title ? await GenerateUniqueTopicSlugAsync(request.Title) : topic.Slug;

                topic.Title = request.Title;
                topic.Slug = slug;
                topic.Content = request.Content;
                topic.CategoryId = request.CategoryId;
                topic.UpdatedAt = DateTime.UtcNow;

                // Update tags
                if (request.Tags != null)
                {
                    // Remove existing tags
                    _context.TopicTags.RemoveRange(topic.TopicTags);
                    await _context.SaveChangesAsync();

                    // Add new tags
                    await AddTagsToTopicAsync(topic.Id, request.Tags);
                }

                await _context.SaveChangesAsync();

                // Update category counts if category changed
                if (oldCategoryId != request.CategoryId)
                {
                    await UpdateCategoryCountsAsync(oldCategoryId);
                    await UpdateCategoryCountsAsync(request.CategoryId);
                }

                return await GetTopicByIdInternalAsync(topic.Id, userId);
            }
            catch (Exception ex)
            {
                return ApiResponse<TopicDetailDTO>.ErrorResult($"Failed to update topic: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeleteTopicAsync(int topicId, int userId)
        {
            try
            {
                var topic = await _context.Topics.FindAsync(topicId);
                if (topic == null)
                {
                    return ApiResponse.ErrorResult("Topic not found");
                }

                // Check permissions
                if (topic.UserId != userId && !await IsModeratorOrAdminAsync(userId))
                {
                    return ApiResponse.ErrorResult("You don't have permission to delete this topic");
                }

                var categoryId = topic.CategoryId;
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();

                // Update category counts
                await UpdateCategoryCountsAsync(categoryId);

                return ApiResponse.SuccessResult("Topic deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to delete topic: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PostDetailDTO>> CreatePostAsync(int topicId, int userId, CreatePostRequest request)
        {
            try
            {
                var topic = await _context.Topics.FindAsync(topicId);
                if (topic == null || !topic.IsActive)
                {
                    return ApiResponse<PostDetailDTO>.ErrorResult("Topic not found");
                }

                if (topic.IsLocked)
                {
                    return ApiResponse<PostDetailDTO>.ErrorResult("Topic is locked");
                }

                // Sanitize post content to prevent XSS
                var sanitizedContent = _htmlSanitizer.Sanitize(request.Content);

                var post = new Post
                {
                    Content = sanitizedContent,
                    TopicId = topicId,
                    UserId = userId,
                    ParentPostId = request.ParentPostId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Posts.Add(post);

                // Update topic activity
                topic.LastActivityAt = DateTime.UtcNow;
                topic.PostCount++;

                await _context.SaveChangesAsync();

                return await GetPostByIdAsync(post.Id, userId);
            }
            catch (Exception ex)
            {
                return ApiResponse<PostDetailDTO>.ErrorResult($"Failed to create post: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PostDetailDTO>> UpdatePostAsync(int postId, int userId, UpdatePostRequest request)
        {
            try
            {
                var post = await _context.Posts.FindAsync(postId);
                if (post == null)
                {
                    return ApiResponse<PostDetailDTO>.ErrorResult("Post not found");
                }

                // Check permissions
                if (post.UserId != userId && !await IsModeratorOrAdminAsync(userId))
                {
                    return ApiResponse<PostDetailDTO>.ErrorResult("You don't have permission to edit this post");
                }

                post.Content = request.Content;
                post.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return await GetPostByIdAsync(post.Id, userId);
            }
            catch (Exception ex)
            {
                return ApiResponse<PostDetailDTO>.ErrorResult($"Failed to update post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> DeletePostAsync(int postId, int userId)
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

                // Check permissions
                if (post.UserId != userId && !await IsModeratorOrAdminAsync(userId))
                {
                    return ApiResponse.ErrorResult("You don't have permission to delete this post");
                }

                _context.Posts.Remove(post);

                // Update topic post count
                post.Topic.PostCount--;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Post deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to delete post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> MarkPostAsAnswerAsync(int postId, int userId)
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

                // Check permissions (topic author or moderator/admin)
                if (post.Topic.UserId != userId && !await IsModeratorOrAdminAsync(userId))
                {
                    return ApiResponse.ErrorResult("Only topic author or moderators can mark answers");
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

        public async Task<ApiResponse> LikeTopicAsync(int topicId, int userId)
        {
            try
            {
                var existingLike = await _context.Likes
                    .FirstOrDefaultAsync(l => l.TopicId == topicId && l.UserId == userId);

                if (existingLike != null)
                {
                    return ApiResponse.ErrorResult("Topic already liked");
                }

                var like = new Like
                {
                    TopicId = topicId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Likes.Add(like);

                // Update topic like count
                var topic = await _context.Topics.FindAsync(topicId);
                if (topic != null)
                {
                    topic.LikeCount++;
                }

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Topic liked successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to like topic: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UnlikeTopicAsync(int topicId, int userId)
        {
            try
            {
                var like = await _context.Likes
                    .FirstOrDefaultAsync(l => l.TopicId == topicId && l.UserId == userId);

                if (like == null)
                {
                    return ApiResponse.ErrorResult("Topic not liked");
                }

                _context.Likes.Remove(like);

                // Update topic like count
                var topic = await _context.Topics.FindAsync(topicId);
                if (topic != null)
                {
                    topic.LikeCount--;
                }

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Topic unliked successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to unlike topic: {ex.Message}");
            }
        }

        public async Task<ApiResponse> LikePostAsync(int postId, int userId)
        {
            try
            {
                var existingLike = await _context.Likes
                    .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

                if (existingLike != null)
                {
                    return ApiResponse.ErrorResult("Post already liked");
                }

                var like = new Like
                {
                    PostId = postId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Likes.Add(like);

                // Update post like count
                var post = await _context.Posts.FindAsync(postId);
                if (post != null)
                {
                    post.LikeCount++;
                }

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Post liked successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to like post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UnlikePostAsync(int postId, int userId)
        {
            try
            {
                var like = await _context.Likes
                    .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

                if (like == null)
                {
                    return ApiResponse.ErrorResult("Post not liked");
                }

                _context.Likes.Remove(like);

                // Update post like count
                var post = await _context.Posts.FindAsync(postId);
                if (post != null)
                {
                    post.LikeCount--;
                }

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Post unliked successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to unlike post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ReportTopicAsync(int topicId, int userId, ReportRequest request)
        {
            try
            {
                var report = new Report
                {
                    TopicId = topicId,
                    UserId = userId,
                    Category = request.Category,
                    Reason = request.Reason,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Reports.Add(report);
                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Topic reported successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to report topic: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ReportPostAsync(int postId, int userId, ReportRequest request)
        {
            try
            {
                var report = new Report
                {
                    PostId = postId,
                    UserId = userId,
                    Category = request.Category,
                    Reason = request.Reason,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Reports.Add(report);
                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Post reported successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to report post: {ex.Message}");
            }
        }

        public async Task<ApiResponse> RecordTopicViewAsync(int topicId, int? userId, string? ipAddress)
        {
            try
            {
                // Check if user has already viewed this topic recently (within 1 hour)
                var recentView = await _context.TopicViews
                    .Where(v => v.TopicId == topicId && 
                               (userId.HasValue ? v.UserId == userId.Value : v.IpAddress == ipAddress) &&
                               v.ViewedAt > DateTime.UtcNow.AddHours(-1))
                    .FirstOrDefaultAsync();

                if (recentView == null)
                {
                    var view = new TopicView
                    {
                        TopicId = topicId,
                        UserId = userId,
                        IpAddress = ipAddress,
                        ViewedAt = DateTime.UtcNow
                    };

                    _context.TopicViews.Add(view);

                    // Update topic view count
                    var topic = await _context.Topics.FindAsync(topicId);
                    if (topic != null)
                    {
                        topic.ViewCount++;
                    }

                    await _context.SaveChangesAsync();
                }

                return ApiResponse.SuccessResult("Topic view recorded");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to record topic view: {ex.Message}");
            }
        }

        private async Task<ApiResponse<TopicDetailDTO>> GetTopicByIdInternalAsync(int topicId, int? currentUserId)
        {
            var topic = await _context.Topics
                .Include(t => t.Category)
                .Include(t => t.User)
                .Include(t => t.TopicTags)
                .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(t => t.Id == topicId);

            if (topic == null)
            {
                return ApiResponse<TopicDetailDTO>.ErrorResult("Topic not found");
            }

            // Check if current user has liked this topic (separate query for accuracy)
            var isLikedByCurrentUser = currentUserId.HasValue && 
                await _context.Likes.AnyAsync(l => l.TopicId == topicId && l.UserId == currentUserId.Value);

            var canEdit = currentUserId.HasValue && 
                (topic.UserId == currentUserId.Value || await IsModeratorOrAdminAsync(currentUserId.Value));

            var canDelete = currentUserId.HasValue && 
                (topic.UserId == currentUserId.Value || await IsModeratorOrAdminAsync(currentUserId.Value));

            var topicDetail = new TopicDetailDTO
            {
                Id = topic.Id,
                Title = topic.Title,
                Slug = topic.Slug,
                Content = topic.Content,
                Category = new CategorySummaryDTO
                {
                    Id = topic.Category.Id,
                    Name = topic.Category.Name,
                    Slug = topic.Category.Slug,
                    Description = topic.Category.Description,
                    Icon = topic.Category.Icon,
                    Color = topic.Category.Color
                },
                Author = new UserSummaryDTO
                {
                    Id = topic.User.Id,
                    Username = topic.User.Username,
                    FullName = topic.User.FullName,
                    Avatar = topic.User.Avatar,
                    Role = topic.User.Role,
                    CreatedAt = topic.User.CreatedAt,
                    TopicCount = topic.User.Topics.Count,
                    PostCount = topic.User.Posts.Count
                },
                IsLocked = topic.IsLocked,
                HasAnswer = topic.HasAnswer,
                ViewCount = topic.ViewCount,
                LikeCount = topic.LikeCount,
                PostCount = topic.PostCount,
                CreatedAt = topic.CreatedAt,
                UpdatedAt = topic.UpdatedAt,
                Tags = topic.TopicTags.Select(tt => new TagSummaryDTO
                {
                    Id = tt.Tag.Id,
                    Name = tt.Tag.Name,
                    Slug = tt.Tag.Slug,
                    Color = tt.Tag.Color
                }).ToList(),
                IsLikedByCurrentUser = isLikedByCurrentUser,
                CanEdit = canEdit,
                CanDelete = canDelete
            };

            return ApiResponse<TopicDetailDTO>.SuccessResult(topicDetail);
        }

        private async Task<ApiResponse<PostDetailDTO>> GetPostByIdAsync(int postId, int? currentUserId)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Topic)
                .Include(p => p.Likes)
                .Include(p => p.Replies)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return ApiResponse<PostDetailDTO>.ErrorResult("Post not found");
            }

            var postDetail = MapToPostDetailDTO(post, currentUserId);
            return ApiResponse<PostDetailDTO>.SuccessResult(postDetail);
        }

        private PostDetailDTO MapToPostDetailDTO(Post post, int? currentUserId)
        {
            // Check if current user has liked this post
            var isLikedByCurrentUser = currentUserId.HasValue && 
                post.Likes.Any(l => l.UserId == currentUserId.Value);

            var canEdit = currentUserId.HasValue && 
                (post.UserId == currentUserId.Value || IsModeratorOrAdminAsync(currentUserId.Value).Result);

            var canDelete = currentUserId.HasValue && 
                (post.UserId == currentUserId.Value || IsModeratorOrAdminAsync(currentUserId.Value).Result);

            var canMarkAsAnswer = currentUserId.HasValue && 
                (post.Topic?.UserId == currentUserId.Value || IsModeratorOrAdminAsync(currentUserId.Value).Result);

            return new PostDetailDTO
            {
                Id = post.Id,
                Content = post.Content,
                Author = new UserSummaryDTO
                {
                    Id = post.User.Id,
                    Username = post.User.Username,
                    FullName = post.User.FullName,
                    Avatar = post.User.Avatar,
                    Role = post.User.Role,
                    CreatedAt = post.User.CreatedAt,
                    TopicCount = 0, // Will be loaded separately if needed
                    PostCount = 0   // Will be loaded separately if needed
                },
                IsAnswer = post.IsAnswer,
                LikeCount = post.LikeCount,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                Replies = post.Replies?.Select(r => MapToPostDetailDTO(r, currentUserId)).ToList() ?? new List<PostDetailDTO>(),
                IsLikedByCurrentUser = isLikedByCurrentUser,
                CanEdit = canEdit,
                CanDelete = canDelete,
                CanMarkAsAnswer = canMarkAsAnswer
            };
        }

        private async Task<bool> IsModeratorOrAdminAsync(int userId)
        {
            var user = await _context.UserAccounts.FindAsync(userId);
            return user != null && (user.Role == "Moderator" || user.Role == "Admin");
        }

        private async Task<string> GenerateUniqueTopicSlugAsync(string title)
        {
            var baseSlug = GenerateSlug(title);
            var slug = baseSlug;
            var counter = 1;

            while (await _context.Topics.AnyAsync(t => t.Slug == slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        private string GenerateSlug(string text)
        {
            var slug = text.ToLowerInvariant().Replace(" ", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = Regex.Replace(slug, @"-+", "-");
            return slug.Trim('-');
        }

        private async Task AddTagsToTopicAsync(int topicId, List<string> tagNames)
        {
            // Sanitize tags to prevent XSS
            var sanitizedTags = _htmlSanitizer.SanitizeTags(tagNames);
            
            foreach (var tagName in sanitizedTags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag
                    {
                        Name = tagName,
                        Slug = GenerateSlug(tagName),
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Tags.Add(tag);
                    await _context.SaveChangesAsync();
                }

                var topicTag = new TopicTag
                {
                    TopicId = topicId,
                    TagId = tag.Id,
                    CreatedAt = DateTime.UtcNow
                };

                _context.TopicTags.Add(topicTag);
            }

            await _context.SaveChangesAsync();

            // Update tag counts
            foreach (var tagName in tagNames)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                if (tag != null)
                {
                    tag.TopicCount = await _context.TopicTags.CountAsync(tt => tt.TagId == tag.Id);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ApiResponse<PaginationResponse<TopicSummaryDTO>>> GetTopicsAsync(TopicsListRequest request)
        {
            try
            {
                var query = _context.Topics
                    .Where(t => t.IsActive)
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.Posts)
                    .Include(t => t.Likes)
                    .Include(t => t.Views)
                    .Include(t => t.TopicTags)
                        .ThenInclude(tt => tt.Tag)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(request.Query))
                {
                    query = query.Where(t => t.Title.Contains(request.Query) ||
                                           t.Content.Contains(request.Query));
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
                    if (request.HasAnswer.Value)
                    {
                        query = query.Where(t => t.Posts.Any(p => p.IsAnswer));
                    }
                    else
                    {
                        query = query.Where(t => !t.Posts.Any(p => p.IsAnswer));
                    }
                }

                if (request.IsAnswered.HasValue)
                {
                    if (request.IsAnswered.Value)
                    {
                        query = query.Where(t => t.Posts.Any());
                    }
                    else
                    {
                        query = query.Where(t => !t.Posts.Any());
                    }
                }

                if (!string.IsNullOrEmpty(request.AuthorRole) && request.AuthorRole != "all")
                {
                    if (request.AuthorRole == "expert")
                    {
                        query = query.Where(t => t.User.Role == "Bác sĩ");
                    }
                    else if (request.AuthorRole == "member")
                    {
                        query = query.Where(t => t.User.Role == "Member");
                    }
                }

                // Apply sorting
                query = request.SortBy?.ToLower() switch
                {
                    "popular" => request.SortOrder == "asc"
                        ? query.OrderBy(t => t.Views.Count)
                        : query.OrderByDescending(t => t.Views.Count),
                    "discussed" => request.SortOrder == "asc"
                        ? query.OrderBy(t => t.Posts.Count)
                        : query.OrderByDescending(t => t.Posts.Count),
                    "answered" => request.SortOrder == "asc"
                        ? query.OrderBy(t => t.Posts.Any(p => p.IsAnswer) ? 0 : 1)
                        : query.OrderByDescending(t => t.Posts.Any(p => p.IsAnswer) ? 0 : 1),
                    _ => request.SortOrder == "asc"
                        ? query.OrderBy(t => t.CreatedAt)
                        : query.OrderByDescending(t => t.CreatedAt)
                };

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

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
                        AuthorName = t.User.FullName,
                        AuthorAvatar = t.User.Avatar,
                        AuthorRole = t.User.Role,
                        IsPinned = t.IsPinned,
                        IsLocked = t.IsLocked,
                        HasAnswer = t.Posts.Any(p => p.IsAnswer),
                        ViewCount = t.Views.Count,
                        LikeCount = t.Likes.Count,
                        PostCount = t.Posts.Count,
                        CreatedAt = t.CreatedAt,
                        UpdatedAt = t.UpdatedAt,
                        LastActivityAt = t.UpdatedAt,
                        Tags = t.TopicTags.Select(tt => tt.Tag.Name).ToList()
                    })
                    .ToListAsync();

                var result = new PaginationResponse<TopicSummaryDTO>
                {
                    Items = topics,
                    TotalItems = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    HasNext = request.Page < totalPages,
                    HasPrevious = request.Page > 1
                };

                return ApiResponse<PaginationResponse<TopicSummaryDTO>>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<TopicSummaryDTO>>.ErrorResponse(
                    "Failed to retrieve topics: " + ex.Message);
            }
        }

        private async Task UpdateCategoryCountsAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                category.TopicCount = await _context.Topics.CountAsync(t => t.CategoryId == categoryId && t.IsActive);
                category.PostCount = await _context.Posts
                    .Where(p => p.Topic.CategoryId == categoryId && p.IsActive)
                    .CountAsync();
                
                await _context.SaveChangesAsync();
            }
        }
    }
}
