using ApiApplication.Data;
using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Services.Admin;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.Admin
{
    public class AdminSearchService : IAdminSearchService
    {
        private readonly AppDbContext _context;
        private readonly IAdminUserService _userService;
        private readonly IAdminTopicService _topicService;
        private readonly IAdminExpertService _expertService;
        private readonly IAdminCategoryService _categoryService;

        public AdminSearchService(
            AppDbContext context, 
            IAdminUserService userService,
            IAdminTopicService topicService,
            IAdminExpertService expertService,
            IAdminCategoryService categoryService)
        {
            _context = context;
            _userService = userService;
            _topicService = topicService;
            _expertService = expertService;
            _categoryService = categoryService;
        }

        public async Task<ApiResponse<AdminGlobalSearchResponse>> GlobalSearchAsync(AdminGlobalSearchRequest request)
        {
            try
            {
                var response = new AdminGlobalSearchResponse
                {
                    Query = request.Query
                };

                // Search based on type filter or search all if no type specified
                if (string.IsNullOrEmpty(request.Type) || request.Type == "all" || request.Type == "users")
                {
                    var userResults = await SearchUsersInternalAsync(request.Query, 5);
                    response.Users = userResults;
                }

                if (string.IsNullOrEmpty(request.Type) || request.Type == "all" || request.Type == "topics")
                {
                    var topicResults = await SearchTopicsInternalAsync(request.Query, 5);
                    response.Topics = topicResults;
                }

                if (string.IsNullOrEmpty(request.Type) || request.Type == "all" || request.Type == "experts")
                {
                    var expertResults = await SearchExpertsInternalAsync(request.Query, 5);
                    response.Experts = expertResults;
                }

                if (string.IsNullOrEmpty(request.Type) || request.Type == "all" || request.Type == "categories")
                {
                    var categoryResults = await SearchCategoriesInternalAsync(request.Query, 5);
                    response.Categories = categoryResults;
                }

                response.TotalResults = response.Users.Count + response.Topics.Count + 
                                      response.Experts.Count + response.Categories.Count;

                return ApiResponse<AdminGlobalSearchResponse>.SuccessResult(response, "Global search completed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminGlobalSearchResponse>.ErrorResult($"Error performing global search: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PaginationResponse<AdminUserResponse>>> SearchUsersAsync(AdminUserSearchRequest request)
        {
            var pagination = new PaginationRequest { Page = request.Page, PageSize = request.PageSize };
            var filter = new AdminUserFilterRequest 
            { 
                Role = request.Role,
                IsActive = request.Status == "true" ? true : request.Status == "false" ? false : null
            };
            
            var result = await _userService.GetUsersAsync(pagination, filter);
            
            if (!result.Success || result.Data == null)
                return ApiResponse<PaginationResponse<AdminUserResponse>>.ErrorResult(result.Message);

            var convertedData = new PaginationResponse<AdminUserResponse>
            {
                Items = result.Data.Items.Select(u => new AdminUserResponse
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FullName = u.FullName,
                    Avatar = u.Avatar,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    IsEmailVerified = u.IsEmailVerified,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt,
                    TopicCount = u.TopicCount,
                    PostCount = u.PostCount
                }).ToList(),
                TotalItems = result.Data.TotalItems,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize,
                TotalPages = result.Data.TotalPages
            };

            return ApiResponse<PaginationResponse<AdminUserResponse>>.SuccessResult(convertedData, result.Message);
        }

        public async Task<ApiResponse<PaginationResponse<AdminTopicResponse>>> SearchTopicsAsync(AdminTopicSearchRequest request)
        {
            var pagination = new PaginationRequest { Page = request.Page, PageSize = request.PageSize };
            var filter = new AdminTopicFilterRequest();
            
            var result = await _topicService.GetTopicsAsync(pagination, filter);
            
            if (!result.Success || result.Data == null)
                return ApiResponse<PaginationResponse<AdminTopicResponse>>.ErrorResult(result.Message);

            var convertedData = new PaginationResponse<AdminTopicResponse>
            {
                Items = result.Data.Items.Select(t => new AdminTopicResponse
                {
                    Id = t.Id,
                    Title = t.Title,
                    Slug = t.Slug,
                    Content = t.Content,
                    CategoryName = t.CategoryName,
                    AuthorName = t.AuthorName,
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
                    Tags = t.Tags
                }).ToList(),
                TotalItems = result.Data.TotalItems,
                Page = result.Data.Page,
                PageSize = result.Data.PageSize,
                TotalPages = result.Data.TotalPages
            };

            return ApiResponse<PaginationResponse<AdminTopicResponse>>.SuccessResult(convertedData, result.Message);
        }

        public async Task<ApiResponse<PaginationResponse<AdminExpertResponse>>> SearchExpertsAsync(AdminExpertSearchRequest request)
        {
            var pagination = new PaginationRequest { Page = request.Page, PageSize = request.PageSize };
            var filter = new AdminExpertFilterRequest 
            { 
                VerificationStatus = request.VerificationStatus,
                Status = request.Status,
                Specialty = request.Specialty
            };
            
            return await _expertService.GetExpertsAsync(pagination, filter);
        }

        public async Task<ApiResponse<List<AdminCategoryResponse>>> SearchCategoriesAsync(string query)
        {
            try
            {
                var categories = await _context.Categories
                    .Where(c => c.Name.Contains(query) || (c.Description != null && c.Description.Contains(query)))
                    .Select(c => new AdminCategoryResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        Slug = c.Slug,
                        Color = c.Color,
                        Icon = c.Icon,
                        IsActive = c.IsActive,
                        DisplayOrder = c.DisplayOrder,
                        TopicCount = _context.Topics.Count(t => t.CategoryId == c.Id),
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt
                    })
                    .Take(10)
                    .ToListAsync();

                return ApiResponse<List<AdminCategoryResponse>>.SuccessResult(categories, "Categories searched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AdminCategoryResponse>>.ErrorResult($"Error searching categories: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminSearchSuggestionsResponse>> GetSearchSuggestionsAsync(string query, int limit)
        {
            try
            {
                var suggestions = new List<string>();

                // Get user suggestions
                var userSuggestions = await _context.UserAccounts
                    .Where(u => u.Username.Contains(query) || u.FullName.Contains(query))
                    .Select(u => u.Username)
                    .Take(3)
                    .ToListAsync();
                suggestions.AddRange(userSuggestions);

                // Get topic suggestions
                var topicSuggestions = await _context.Topics
                    .Where(t => t.Title.Contains(query))
                    .Select(t => t.Title)
                    .Take(3)
                    .ToListAsync();
                suggestions.AddRange(topicSuggestions);

                // Get category suggestions
                var categorySuggestions = await _context.Categories
                    .Where(c => c.Name.Contains(query))
                    .Select(c => c.Name)
                    .Take(2)
                    .ToListAsync();
                suggestions.AddRange(categorySuggestions);

                var response = new AdminSearchSuggestionsResponse
                {
                    Query = query,
                    Suggestions = suggestions.Take(limit).ToList()
                };

                return ApiResponse<AdminSearchSuggestionsResponse>.SuccessResult(response, "Search suggestions retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminSearchSuggestionsResponse>.ErrorResult($"Error getting search suggestions: {ex.Message}");
            }
        }

        private async Task<List<AdminSearchResultItem>> SearchUsersInternalAsync(string query, int limit)
        {
            var users = await _context.UserAccounts
                .Where(u => u.Username.Contains(query) || u.FullName.Contains(query) || u.Email.Contains(query))
                .Take(limit)
                .ToListAsync();

            return users.Select(u => new AdminSearchResultItem
            {
                Id = u.Id,
                Type = "user",
                Title = u.FullName,
                Subtitle = u.Username,
                Description = u.Bio ?? "",
                CreatedAt = u.CreatedAt,
                Metadata = new Dictionary<string, object>
                {
                    ["email"] = u.Email,
                    ["role"] = u.Role,
                    ["isActive"] = u.IsActive
                }
            }).ToList();
        }

        private async Task<List<AdminSearchResultItem>> SearchTopicsInternalAsync(string query, int limit)
        {
            var topics = await _context.Topics
                .Where(t => t.Title.Contains(query) || t.Content.Contains(query))
                .Take(limit)
                .ToListAsync();

            return topics.Select(t => new AdminSearchResultItem
            {
                Id = t.Id,
                Type = "topic",
                Title = t.Title,
                Subtitle = _context.UserAccounts.Where(u => u.Id == t.UserId).Select(u => u.FullName).FirstOrDefault(),
                Description = t.Content != null && t.Content.Length > 100 ? t.Content.Substring(0, 100) + "..." : t.Content,
                CreatedAt = t.CreatedAt,
                Metadata = new Dictionary<string, object>
                {
                    ["category"] = _context.Categories.Where(c => c.Id == t.CategoryId).Select(c => c.Name).FirstOrDefault() ?? "",
                    ["isLocked"] = t.IsLocked,
                    ["isPinned"] = t.IsPinned
                }
            }).ToList();
        }

        private async Task<List<AdminSearchResultItem>> SearchExpertsInternalAsync(string query, int limit)
        {
            var experts = await _context.UserAccounts
                .Where(u => u.Role == "Bác sĩ" && (u.Username.Contains(query) || u.FullName.Contains(query) || (u.Bio != null && u.Bio.Contains(query))))
                .Take(limit)
                .ToListAsync();

            return experts.Select(u => new AdminSearchResultItem
            {
                Id = u.Id,
                Type = "expert",
                Title = u.FullName,
                Subtitle = u.Username,
                Description = u.Bio ?? "",
                CreatedAt = u.CreatedAt,
                Metadata = new Dictionary<string, object>
                {
                    ["specialty"] = u.Bio ?? "",
                    ["isVerified"] = u.IsActive,
                    ["rating"] = 4.5
                }
            }).ToList();
        }

        private async Task<List<AdminSearchResultItem>> SearchCategoriesInternalAsync(string query, int limit)
        {
            var categories = await _context.Categories
                .Where(c => c.Name.Contains(query) || (c.Description != null && c.Description.Contains(query)))
                .Take(limit)
                .ToListAsync();

            return categories.Select(c => new AdminSearchResultItem
            {
                Id = c.Id,
                Type = "category",
                Title = c.Name,
                Subtitle = "",
                Description = c.Description ?? "",
                CreatedAt = c.CreatedAt,
                Metadata = new Dictionary<string, object>
                {
                    ["topicCount"] = _context.Topics.Count(t => t.CategoryId == c.Id),
                    ["isActive"] = c.IsActive,
                    ["displayOrder"] = c.DisplayOrder
                }
            }).ToList();
        }
    }
}
