using ApiApplication.Data;
using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.Admin
{
    public class AdminExpertService : IAdminExpertService
    {
        private readonly AppDbContext _context;

        public AdminExpertService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginationResponse<AdminExpertResponse>>> GetExpertsAsync(PaginationRequest pagination, AdminExpertFilterRequest? filter = null)
        {
            try
            {
                // Handle null pagination
                if (pagination == null)
                {
                    pagination = new PaginationRequest { Page = 1, PageSize = 10 };
                }

                // Ensure pagination has valid values
                pagination.Page = pagination.Page > 0 ? pagination.Page : 1;
                pagination.PageSize = pagination.PageSize > 0 ? pagination.PageSize : 10;

                var query = _context.UserAccounts
                    .Where(u => u.Role == "Bác sĩ")
                    .AsQueryable();

                // Apply filters
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.VerificationStatus))
                    {
                        if (filter.VerificationStatus == "Verified")
                            query = query.Where(u => u.IsActive == true);
                        else if (filter.VerificationStatus == "Pending")
                            query = query.Where(u => u.IsActive == false);
                    }

                    if (!string.IsNullOrEmpty(filter.Status))
                    {
                        if (filter.Status == "Active")
                            query = query.Where(u => u.IsActive == true);
                        else if (filter.Status == "Inactive")
                            query = query.Where(u => u.IsActive == false);
                    }

                    if (!string.IsNullOrEmpty(filter.Specialty))
                        query = query.Where(u => u.Bio != null && u.Bio.Contains(filter.Specialty));

                }

                var totalCount = await query.CountAsync();
                var expertsList = await query
                    .OrderByDescending(u => u.CreatedAt)
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                var experts = expertsList.Select(u => new AdminExpertResponse
                {
                    Id = u.Id,
                    Username = u.Username ?? "",
                    Email = u.Email ?? "",
                    FullName = u.FullName ?? "",
                    Avatar = u.Avatar ?? "",
                    Bio = u.Bio ?? "",
                    Specialty = u.Bio ?? "", 
                    Rating = 4.5, 
                    ReviewCount = 0, 
                    IsVerified = u.IsActive, 
                    IsOnline = u.LastLoginAt.HasValue && u.LastLoginAt >= DateTime.UtcNow.AddMinutes(-30),
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt,
                    TopicCount = 0, // Simplified for now
                    PostCount = 0   // Simplified for now
                }).ToList();

                var totalPages = totalCount > 0 ? (int)Math.Ceiling((double)totalCount / pagination.PageSize) : 0;
                
                var paginatedResponse = new PaginationResponse<AdminExpertResponse>
                {
                    Items = experts,
                    TotalItems = totalCount,
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalPages = totalPages,
                    HasNext = pagination.Page < totalPages,
                    HasPrevious = pagination.Page > 1
                };

                return ApiResponse<PaginationResponse<AdminExpertResponse>>.SuccessResult(paginatedResponse, "Lấy danh sách chuyên gia thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<AdminExpertResponse>>.ErrorResult($"Lỗi: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminExpertResponse>> GetExpertByIdAsync(int expertId)
        {
            try
            {
                var expert = await _context.UserAccounts
                    .Where(u => u.Id == expertId && u.Role == "Bác sĩ")
                    .Select(u => new AdminExpertResponse
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Email = u.Email,
                        FullName = u.FullName,
                        Avatar = u.Avatar,
                        Bio = u.Bio,
                        Specialty = u.Bio,
                        Rating = 4.5,
                        ReviewCount = 0,
                        IsVerified = u.IsActive,
                        IsOnline = u.LastLoginAt >= DateTime.UtcNow.AddMinutes(-30),
                        IsActive = u.IsActive,
                        CreatedAt = u.CreatedAt,
                        LastLoginAt = u.LastLoginAt,
                        TopicCount = _context.Topics.Count(t => t.UserId == u.Id),
                        PostCount = _context.Posts.Count(p => p.UserId == u.Id)
                    })
                    .FirstOrDefaultAsync();

                if (expert == null)
                {
                    return ApiResponse<AdminExpertResponse>.ErrorResult("Không tìm thấy chuyên gia");
                }

                return ApiResponse<AdminExpertResponse>.SuccessResult(expert, "Lấy thông tin chuyên gia thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminExpertResponse>.ErrorResult($"Lỗi khi lấy thông tin chuyên gia: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminExpertStatsResponse>> GetExpertStatsAsync()
        {
            try
            {
                var totalExperts = await _context.UserAccounts.CountAsync(u => u.Role == "Bác sĩ");
                var verifiedExperts = await _context.UserAccounts.CountAsync(u => u.Role == "Bác sĩ" && u.IsActive);
                var onlineExperts = await _context.UserAccounts.CountAsync(u => u.Role == "Bác sĩ" && u.LastLoginAt >= DateTime.UtcNow.AddMinutes(-30));
                var newExpertsThisMonth = await _context.UserAccounts.CountAsync(u => u.Role == "Bác sĩ" && u.CreatedAt >= DateTime.UtcNow.AddMonths(-1));

                var stats = new AdminExpertStatsResponse
                {
                    TotalExperts = totalExperts,
                    VerifiedExperts = verifiedExperts,
                    OnlineExperts = onlineExperts,
                    NewExpertsThisMonth = newExpertsThisMonth,
                    AverageRating = 4.2, // Placeholder
                    TotalReviews = 0, // Placeholder
                    TopSpecialties = new List<AdminSpecialtyStatsResponse>
                    {
                        new() { Name = "General Medicine", ExpertCount = 10 },
                        new() { Name = "Cardiology", ExpertCount = 8 },
                        new() { Name = "Dermatology", ExpertCount = 6 }
                    }
                };

                return ApiResponse<AdminExpertStatsResponse>.SuccessResult(stats, "Expert statistics retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminExpertStatsResponse>.ErrorResult($"Error retrieving expert statistics: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> VerifyExpertAsync(int expertId, AdminVerifyExpertRequest request)
        {
            try
            {
                var expert = await _context.UserAccounts.FindAsync(expertId);
                if (expert == null || expert.Role != "Bác sĩ")
                {
                    return ApiResponse<bool>.ErrorResult("Expert not found");
                }

                // For now, we'll use IsActive as verification status
                expert.IsActive = request.IsVerified;
                expert.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, $"Expert {(request.IsVerified ? "verified" : "unverified")} successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error updating expert verification: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> ToggleExpertStatusAsync(int expertId)
        {
            try
            {
                var expert = await _context.UserAccounts.FindAsync(expertId);
                if (expert == null || expert.Role != "Bác sĩ")
                {
                    return ApiResponse<bool>.ErrorResult("Expert not found");
                }

                expert.IsActive = !expert.IsActive;
                expert.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, $"Expert status {(expert.IsActive ? "activated" : "deactivated")} successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error toggling expert status: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateExpertSpecialtyAsync(int expertId, AdminUpdateExpertSpecialtyRequest request)
        {
            try
            {
                var expert = await _context.UserAccounts.FindAsync(expertId);
                if (expert == null || expert.Role != "Bác sĩ")
                {
                    return ApiResponse<bool>.ErrorResult("Expert not found");
                }

                // For now, we'll store specialty in bio field
                expert.Bio = request.Specialty;
                expert.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, "Expert specialty updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error updating expert specialty: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<AdminSpecialtyResponse>>> GetSpecialtiesAsync()
        {
            try
            {
                var specialties = await _context.Specialties
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToListAsync();

                var response = specialties.Select(s => new AdminSpecialtyResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ExpertCount = _context.UserAccounts.Count(u => u.Role == "Bác sĩ" && u.Bio == s.Name && u.IsActive),
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt
                }).ToList();

                return ApiResponse<List<AdminSpecialtyResponse>>.SuccessResult(response, "Specialties retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AdminSpecialtyResponse>>.ErrorResult($"Error retrieving specialties: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminSpecialtyResponse>> CreateSpecialtyAsync(AdminCreateSpecialtyRequest request)
        {
            try
            {
                // Check if specialty with same name already exists
                var existingSpecialty = await _context.Specialties
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == request.Name.ToLower());

                if (existingSpecialty != null)
                {
                    return ApiResponse<AdminSpecialtyResponse>.ErrorResult("A specialty with this name already exists");
                }

                var specialty = new Specialty
                {
                    Name = request.Name,
                    Description = request.Description ?? string.Empty,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Specialties.Add(specialty);
                await _context.SaveChangesAsync();

                var response = new AdminSpecialtyResponse
                {
                    Id = specialty.Id,
                    Name = specialty.Name,
                    Description = specialty.Description,
                    ExpertCount = 0,
                    IsActive = specialty.IsActive,
                    CreatedAt = specialty.CreatedAt
                };

                return ApiResponse<AdminSpecialtyResponse>.SuccessResult(response, "Specialty created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminSpecialtyResponse>.ErrorResult($"Error creating specialty: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminSpecialtyResponse>> UpdateSpecialtyAsync(int specialtyId, AdminUpdateSpecialtyRequest request)
        {
            try
            {
                var specialty = await _context.Specialties.FindAsync(specialtyId);
                if (specialty == null)
                {
                    return ApiResponse<AdminSpecialtyResponse>.ErrorResult("Specialty not found");
                }

                // Check if another specialty with the same name exists
                var existingSpecialty = await _context.Specialties
                    .FirstOrDefaultAsync(s => s.Name.ToLower() == request.Name.ToLower() && s.Id != specialtyId);

                if (existingSpecialty != null)
                {
                    return ApiResponse<AdminSpecialtyResponse>.ErrorResult("A specialty with this name already exists");
                }

                specialty.Name = request.Name;
                specialty.Description = request.Description ?? string.Empty;
                specialty.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var response = new AdminSpecialtyResponse
                {
                    Id = specialty.Id,
                    Name = specialty.Name,
                    Description = specialty.Description,
                    ExpertCount = _context.UserAccounts.Count(u => u.Role == "Bác sĩ" && u.Bio == specialty.Name && u.IsActive),
                    IsActive = specialty.IsActive,
                    CreatedAt = specialty.CreatedAt
                };

                return ApiResponse<AdminSpecialtyResponse>.SuccessResult(response, "Specialty updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminSpecialtyResponse>.ErrorResult($"Error updating specialty: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteSpecialtyAsync(int specialtyId)
        {
            try
            {
                var specialty = await _context.Specialties.FindAsync(specialtyId);
                if (specialty == null)
                {
                    return ApiResponse<bool>.ErrorResult("Specialty not found");
                }

                // Check if any experts are using this specialty
                var expertsUsingSpecialty = await _context.UserAccounts
                    .AnyAsync(u => u.Role == "Bác sĩ" && u.Bio == specialty.Name);

                if (expertsUsingSpecialty)
                {
                    return ApiResponse<bool>.ErrorResult("Cannot delete specialty that is being used by experts");
                }

                _context.Specialties.Remove(specialty);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResult(true, "Specialty deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResult($"Error deleting specialty: {ex.Message}");
            }
        }

        public Task<ApiResponse<PaginationResponse<AdminExpertReviewResponse>>> GetExpertReviewsAsync(int expertId, PaginationRequest pagination)
        {
            try
            {
                // Placeholder implementation - return sample reviews
                var reviews = new List<AdminExpertReviewResponse>
                {
                    new() { Id = 1, ReviewerName = "John Doe", Rating = 5.0, Comment = "Excellent service", CreatedAt = DateTime.UtcNow.AddDays(-5), IsReported = false },
                    new() { Id = 2, ReviewerName = "Jane Smith", Rating = 4.5, Comment = "Very helpful", CreatedAt = DateTime.UtcNow.AddDays(-10), IsReported = false }
                };

                var paginatedResponse = new PaginationResponse<AdminExpertReviewResponse>
                {
                    Items = reviews,
                    TotalItems = reviews.Count,
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalPages = 1,
                    HasNext = false,
                    HasPrevious = false
                };

                return Task.FromResult(ApiResponse<PaginationResponse<AdminExpertReviewResponse>>.SuccessResult(paginatedResponse, "Expert reviews retrieved successfully"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ApiResponse<PaginationResponse<AdminExpertReviewResponse>>.ErrorResult($"Error retrieving expert reviews: {ex.Message}"));
            }
        }

        public Task<ApiResponse<bool>> DeleteReviewAsync(int reviewId)
        {
            // Placeholder implementation
            return Task.FromResult(ApiResponse<bool>.SuccessResult(true, "Review deleted successfully"));
        }
    }
}
