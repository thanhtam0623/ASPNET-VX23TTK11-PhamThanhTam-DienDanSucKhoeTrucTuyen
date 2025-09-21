using ApiApplication.Data;
using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.User
{
    public class UserExpertService : IUserExpertService
    {
        private readonly AppDbContext _context;

        public UserExpertService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginationResponse<ExpertSummaryDTO>>> GetExpertsAsync(ExpertSearchRequest request)
        {
            try
            {
                var query = _context.UserAccounts
                    .Where(u => u.Role == "Bác sĩ" && u.IsActive)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(request.Query))
                {
                    query = query.Where(u => u.FullName.Contains(request.Query) ||
                                           u.Bio!.Contains(request.Query) ||
                                           u.Username.Contains(request.Query));
                }

                if (!string.IsNullOrEmpty(request.Specialty))
                {
                    query = query.Where(u => u.Bio!.Contains(request.Specialty));
                }

                if (request.IsOnline.HasValue && request.IsOnline.Value)
                {
                    // Consider users online if they were active in last 15 minutes
                    var onlineThreshold = DateTime.UtcNow.AddMinutes(-15);
                    query = query.Where(u => u.LastLoginAt >= onlineThreshold);
                }

                // Apply sorting
                query = request.SortBy?.ToLower() switch
                {
                    "experience" => request.SortOrder == "asc" 
                        ? query.OrderBy(u => u.CreatedAt)
                        : query.OrderByDescending(u => u.CreatedAt),
                    "answers" => request.SortOrder == "asc"
                        ? query.OrderBy(u => u.Posts.Count)
                        : query.OrderByDescending(u => u.Posts.Count),
                    "reviews" => request.SortOrder == "asc"
                        ? query.OrderBy(u => u.Topics.Count)
                        : query.OrderByDescending(u => u.Topics.Count),
                    _ => request.SortOrder == "asc"
                        ? query.OrderBy(u => u.CreatedAt)
                        : query.OrderByDescending(u => u.CreatedAt)
                };

                var totalCount = await query.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                var experts = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(u => u.Posts)
                    .Include(u => u.Topics)
                    .Select(u => new ExpertSummaryDTO
                    {
                        Id = u.Id,
                        Username = u.Username,
                        FullName = u.FullName,
                        Avatar = u.Avatar,
                        Bio = u.Bio,
                        Specialties = ExtractSpecialties(u.Bio),
                        Location = ExtractLocation(u.Bio),
                        Rating = CalculateRating(u.Posts.Count, u.Topics.Count),
                        ReviewCount = u.Topics.Count,
                        AnswerCount = u.Posts.Count,
                        Experience = CalculateExperience(u.CreatedAt),
                        IsVerified = true, // All doctors are verified
                        IsOnline = u.LastLoginAt >= DateTime.UtcNow.AddMinutes(-15),
                        LastSeen = u.LastLoginAt,
                        CreatedAt = u.CreatedAt
                    })
                    .ToListAsync();

                var result = new PaginationResponse<ExpertSummaryDTO>
                {
                    Items = experts,
                    TotalItems = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    HasNext = request.Page < totalPages,
                    HasPrevious = request.Page > 1
                };

                return ApiResponse<PaginationResponse<ExpertSummaryDTO>>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<ExpertSummaryDTO>>.ErrorResult(
                    "Failed to retrieve experts: " + ex.Message);
            }
        }

        public async Task<ApiResponse<ExpertDetailDTO>> GetExpertByUsernameAsync(string username)
        {
            try
            {
                var expert = await _context.UserAccounts
                    .Where(u => u.Username == username && u.Role == "Bác sĩ" && u.IsActive)
                    .Include(u => u.Posts)
                    .Include(u => u.Topics)
                        .ThenInclude(t => t.Category)
                    .Include(u => u.Topics)
                        .ThenInclude(t => t.TopicTags)
                            .ThenInclude(tt => tt.Tag)
                    .FirstOrDefaultAsync();

                if (expert == null)
                {
                    return ApiResponse<ExpertDetailDTO>.ErrorResult("Expert not found");
                }

                var recentTopics = expert.Topics
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(10)
                    .Select(t => new TopicSummaryDTO
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Slug = t.Slug,
                        CategoryName = t.Category.Name,
                        CategorySlug = t.Category.Slug,
                        AuthorName = expert.FullName,
                        AuthorAvatar = expert.Avatar,
                        AuthorRole = expert.Role,
                        IsPinned = t.IsPinned,
                        IsLocked = t.IsLocked,
                        HasAnswer = t.Posts.Any(p => p.IsAnswer),
                        ViewCount = t.Views.Count,
                        LikeCount = t.Likes.Count,
                        PostCount = t.Posts.Count,
                        CreatedAt = t.CreatedAt,
                        LastActivityAt = t.UpdatedAt,
                        Tags = t.TopicTags.Select(tt => tt.Tag.Name).ToList()
                    })
                    .ToList();

                var stats = new ExpertStatsDTO
                {
                    TotalTopics = expert.Topics.Count,
                    TotalAnswers = expert.Topics.Sum(t => t.PostCount),
                    TotalViews = expert.Topics.Sum(t => t.ViewCount),
                    AverageRating = 4.5, // Mock rating
                    JoinedDate = expert.CreatedAt.ToString("yyyy-MM-dd"),
                    LastSeenAt = expert.LastLoginAt?.ToString("yyyy-MM-dd HH:mm") ?? "Never",
                    VerifiedAt = expert.CreatedAt.ToString("yyyy-MM-dd"),
                    PopularTopics = expert.Topics
                        .OrderByDescending(t => t.ViewCount)
                        .Take(3)
                        .Select(t => t.Title)
                        .ToList()
                };

                var expertDetail = new ExpertDetailDTO
                {
                    Id = expert.Id,
                    Username = expert.Username,
                    FullName = expert.FullName,
                    Avatar = expert.Avatar,
                    Bio = expert.Bio,
                    Specialties = ExtractSpecialties(expert.Bio),
                    Location = ExtractLocation(expert.Bio),
                    Rating = CalculateRating(expert.Posts.Count, expert.Topics.Count),
                    ReviewCount = expert.Topics.Count,
                    AnswerCount = expert.Posts.Count,
                    Experience = CalculateExperience(expert.CreatedAt),
                    IsVerified = true,
                    IsOnline = expert.LastLoginAt >= DateTime.UtcNow.AddMinutes(-15),
                    LastSeen = expert.LastLoginAt,
                    CreatedAt = expert.CreatedAt,
                    Education = ExtractEducation(expert.Bio),
                    Certifications = ExtractCertifications(expert.Bio),
                    WorkHistory = ExtractWorkHistory(expert.Bio),
                    RecentTopics = recentTopics,
                    RecentReviews = new List<ReviewDTO>(), // Will implement reviews later if needed
                    Stats = stats
                };

                return ApiResponse<ExpertDetailDTO>.SuccessResponse(expertDetail);
            }
            catch (Exception ex)
            {
                return ApiResponse<ExpertDetailDTO>.ErrorResponse(
                    "Failed to retrieve expert details: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<SpecialtyDTO>>> GetExpertSpecialtiesAsync()
        {
            try
            {
                var specialties = await _context.Specialties
                    .Where(s => s.IsActive)
                    .Select(s => new SpecialtyDTO
                    {
                        Id = s.Id,
                        Name = s.Name
                    })
                    .OrderBy(s => s.Name)
                    .ToListAsync();

                return ApiResponse<List<SpecialtyDTO>>.SuccessResult(specialties);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<SpecialtyDTO>>.ErrorResult(
                    "Failed to retrieve specialties: " + ex.Message);
            }
        }

        public async Task<ApiResponse<List<TopicSummaryDTO>>> GetExpertTopicsAsync(int expertId, int page = 1, int pageSize = 20)
        {
            try
            {
                var topics = await _context.Topics
                    .Where(t => t.UserId == expertId && t.IsActive)
                    .Include(t => t.Category)
                    .Include(t => t.User)
                    .Include(t => t.Posts)
                    .Include(t => t.Likes)
                    .Include(t => t.Views)
                    .Include(t => t.TopicTags)
                        .ThenInclude(tt => tt.Tag)
                    .OrderByDescending(t => t.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
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
                        LastActivityAt = t.UpdatedAt,
                        Tags = t.TopicTags.Select(tt => tt.Tag.Name).ToList()
                    })
                    .ToListAsync();

                return ApiResponse<List<TopicSummaryDTO>>.SuccessResult(topics);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<TopicSummaryDTO>>.ErrorResult(
                    "Failed to retrieve expert topics: " + ex.Message);
            }
        }

        public async Task<ApiResponse<ExpertStatsDTO>> GetExpertStatsAsync(int expertId)
        {
            try
            {
                var expert = await _context.UserAccounts
                    .Where(u => u.Id == expertId)
                    .Include(u => u.Posts)
                    .Include(u => u.Topics)
                        .ThenInclude(t => t.Views)
                    .Include(u => u.Topics)
                        .ThenInclude(t => t.Likes)
                    .FirstOrDefaultAsync();

                if (expert == null)
                {
                    return ApiResponse<ExpertStatsDTO>.ErrorResult("Expert not found");
                }

                var stats = new ExpertStatsDTO
                {
                    TotalTopics = expert.Topics.Count,
                    TotalAnswers = expert.Topics.Sum(t => t.PostCount),
                    TotalViews = expert.Topics.Sum(t => t.ViewCount),
                    AverageRating = 4.5, // Mock rating
                    JoinedDate = expert.CreatedAt.ToString("yyyy-MM-dd"),
                    LastSeenAt = expert.LastLoginAt?.ToString("yyyy-MM-dd HH:mm") ?? "Never",
                    VerifiedAt = expert.CreatedAt.ToString("yyyy-MM-dd"),
                    PopularTopics = expert.Topics
                        .OrderByDescending(t => t.ViewCount)
                        .Take(3)
                        .Select(t => t.Title)
                        .ToList()
                };

                return ApiResponse<ExpertStatsDTO>.SuccessResult(stats);
            }
            catch (Exception ex)
            {
                return ApiResponse<ExpertStatsDTO>.ErrorResult(
                    "Failed to retrieve expert stats: " + ex.Message);
            }
        }

        // Helper methods
        private static List<string> ExtractSpecialties(string? bio)
        {
            if (string.IsNullOrEmpty(bio)) return new List<string>();
            
            var specialties = new List<string> { "General Medicine" };
            var commonSpecialties = new[]
            {
                "Cardiology", "Mental Health", "Diabetes", "Dermatology", 
                "Orthopedics", "Ophthalmology", "Nutrition", "Women's Health", 
                "Pediatrics", "Emergency Medicine", "Preventive Care"
            };

            foreach (var specialty in commonSpecialties)
            {
                if (bio.Contains(specialty, StringComparison.OrdinalIgnoreCase))
                {
                    specialties.Add(specialty);
                }
            }

            return specialties.Take(3).ToList();
        }

        private static string? ExtractLocation(string? bio)
        {
            // Simple location extraction - could be enhanced
            return "Healthcare Facility"; // Default location
        }

        private static double CalculateRating(int postsCount, int topicsCount)
        {
            // Simple rating calculation based on activity
            var totalActivity = postsCount + topicsCount;
            if (totalActivity == 0) return 4.0;
            if (totalActivity < 10) return 4.2;
            if (totalActivity < 50) return 4.5;
            if (totalActivity < 100) return 4.7;
            return 4.9;
        }

        private static string CalculateExperience(DateTime createdAt)
        {
            var years = (DateTime.UtcNow - createdAt).Days / 365;
            if (years == 0) return "Less than 1 year";
            return $"{years}+ year{(years > 1 ? "s" : "")}";
        }

        private static string? ExtractEducation(string? bio)
        {
            return !string.IsNullOrEmpty(bio) ? "Medical Doctor (MD)" : null;
        }

        private static string? ExtractCertifications(string? bio)
        {
            return !string.IsNullOrEmpty(bio) ? "Board Certified" : null;
        }

        private static string? ExtractWorkHistory(string? bio)
        {
            return !string.IsNullOrEmpty(bio) ? "Healthcare Professional" : null;
        }
    }
}
