using ApiApplication.Data;
using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly AppDbContext _context;

        public AdminUserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PaginationResponse<AdminUserListDTO>>> GetUsersAsync(PaginationRequest pagination, AdminUserFilterRequest? filter = null)
        {
            try
            {
                var query = _context.UserAccounts.AsQueryable();

                // Apply filters
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Role))
                        query = query.Where(u => u.Role == filter.Role);

                    if (filter.IsActive.HasValue)
                        query = query.Where(u => u.IsActive == filter.IsActive.Value);

                    if (filter.IsEmailVerified.HasValue)
                        query = query.Where(u => u.IsEmailVerified == filter.IsEmailVerified.Value);

                    if (filter.CreatedFrom.HasValue)
                        query = query.Where(u => u.CreatedAt >= filter.CreatedFrom.Value);

                    if (filter.CreatedTo.HasValue)
                        query = query.Where(u => u.CreatedAt <= filter.CreatedTo.Value);

                }

                // Count total items
                var totalItems = await query.CountAsync();

                // Apply sorting
                switch (pagination.SortBy?.ToLower())
                {
                    case "username":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(u => u.Username) : query.OrderByDescending(u => u.Username);
                        break;
                    case "email":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(u => u.Email) : query.OrderByDescending(u => u.Email);
                        break;
                    case "role":
                        query = pagination.SortOrder == "asc" ? query.OrderBy(u => u.Role) : query.OrderByDescending(u => u.Role);
                        break;
                    default:
                        query = pagination.SortOrder == "asc" ? query.OrderBy(u => u.CreatedAt) : query.OrderByDescending(u => u.CreatedAt);
                        break;
                }

                // Apply pagination
                var users = await query
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .Select(u => new AdminUserListDTO
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
                        TopicCount = u.Topics.Count,
                        PostCount = u.Posts.Count
                    })
                    .ToListAsync();

                var response = new PaginationResponse<AdminUserListDTO>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize),
                    HasNext = pagination.Page * pagination.PageSize < totalItems,
                    HasPrevious = pagination.Page > 1,
                    Items = users
                };

                return ApiResponse<PaginationResponse<AdminUserListDTO>>.SuccessResult(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginationResponse<AdminUserListDTO>>.ErrorResult($"Failed to get users: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminUserListDTO>> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _context.UserAccounts
                    .Where(u => u.Id == userId)
                    .Select(u => new AdminUserListDTO
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
                        TopicCount = u.Topics.Count,
                        PostCount = u.Posts.Count
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return ApiResponse<AdminUserListDTO>.ErrorResult("User not found");
                }

                return ApiResponse<AdminUserListDTO>.SuccessResult(user);
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminUserListDTO>.ErrorResult($"Failed to get user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminUserStatsDTO>> GetUserStatsAsync()
        {
            try
            {
                var totalUsers = await _context.UserAccounts.CountAsync();
                var activeUsers = await _context.UserAccounts.CountAsync(u => u.IsActive);
                var memberCount = await _context.UserAccounts.CountAsync(u => u.Role == "Member");
                var doctorCount = await _context.UserAccounts.CountAsync(u => u.Role == "Bác sĩ");
                var moderatorCount = await _context.UserAccounts.CountAsync(u => u.Role == "Moderator");

                var thisMonth = DateTime.UtcNow.AddDays(-30);
                var newUsersThisMonth = await _context.UserAccounts.CountAsync(u => u.CreatedAt >= thisMonth);

                var stats = new AdminUserStatsDTO
                {
                    TotalUsers = totalUsers,
                    ActiveUsers = activeUsers,
                    MemberCount = memberCount,
                    DoctorCount = doctorCount,
                    ModeratorCount = moderatorCount,
                    NewUsersThisMonth = newUsersThisMonth
                };

                return ApiResponse<AdminUserStatsDTO>.SuccessResult(stats);
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminUserStatsDTO>.ErrorResult($"Failed to get user stats: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminUserListDTO>> CreateUserAsync(AdminCreateUserRequest request)
        {
            try
            {
                // Check if username already exists
                var existingUser = await _context.UserAccounts
                    .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);
                
                if (existingUser != null)
                {
                    return ApiResponse<AdminUserListDTO>.ErrorResult("Username or email already exists");
                }

                // Create new user
                var newUser = new Models.Entities.UserAccount
                {
                    Username = request.Username,
                    Email = request.Email,
                    FullName = request.FullName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = request.Role,
                    IsActive = true,
                    IsEmailVerified = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.UserAccounts.Add(newUser);
                await _context.SaveChangesAsync();

                // Return created user data
                var createdUser = new AdminUserListDTO
                {
                    Id = newUser.Id,
                    Username = newUser.Username,
                    Email = newUser.Email,
                    FullName = newUser.FullName,
                    Avatar = newUser.Avatar,
                    Role = newUser.Role,
                    IsActive = newUser.IsActive,
                    IsEmailVerified = newUser.IsEmailVerified,
                    CreatedAt = newUser.CreatedAt,
                    LastLoginAt = newUser.LastLoginAt,
                    TopicCount = 0,
                    PostCount = 0
                };

                return ApiResponse<AdminUserListDTO>.SuccessResult(createdUser, "User created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminUserListDTO>.ErrorResult($"Failed to create user: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminUserListDTO>> UpdateUserAsync(int userId, AdminUpdateUserRequest request)
        {
            try
            {
                var user = await _context.UserAccounts.FindAsync(userId);
                if (user == null)
                {
                    return ApiResponse<AdminUserListDTO>.ErrorResult("User not found");
                }

                // Update fields if provided
                if (!string.IsNullOrEmpty(request.FullName))
                    user.FullName = request.FullName;

                if (!string.IsNullOrEmpty(request.Email))
                    user.Email = request.Email;

                if (!string.IsNullOrEmpty(request.Role))
                    user.Role = request.Role;

                if (request.IsActive.HasValue)
                    user.IsActive = request.IsActive.Value;

                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Return updated user data
                var updatedUser = new AdminUserListDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Avatar = user.Avatar,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    IsEmailVerified = user.IsEmailVerified,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    TopicCount = user.Topics.Count,
                    PostCount = user.Posts.Count
                };

                return ApiResponse<AdminUserListDTO>.SuccessResult(updatedUser, "User updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminUserListDTO>.ErrorResult($"Failed to update user: {ex.Message}");
            }
        }

        public async Task<ApiResponse> UpdateUserRoleAsync(int userId, AdminUpdateUserRoleRequest request)
        {
            try
            {
                var user = await _context.UserAccounts.FindAsync(userId);
                if (user == null)
                {
                    return ApiResponse.ErrorResult("User not found");
                }

                user.Role = request.Role;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("User role updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to update user role: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ToggleUserStatusAsync(int userId)
        {
            try
            {
                var user = await _context.UserAccounts.FindAsync(userId);
                if (user == null)
                {
                    return ApiResponse.ErrorResult("User not found");
                }

                user.IsActive = !user.IsActive;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var status = user.IsActive ? "activated" : "deactivated";
                return ApiResponse.SuccessResult($"User {status} successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to toggle user status: {ex.Message}");
            }
        }

        public async Task<ApiResponse<object>> ResetUserPasswordAsync(AdminResetPasswordRequest request)
        {
            try
            {
                var user = await _context.UserAccounts.FindAsync(request.UserId);

                if (user == null)
                {
                    return ApiResponse<object>.ErrorResult("User not found");
                }

                // Use the provided new password
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse<object>.SuccessResult(new { }, "Password reset successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<object>.ErrorResult($"Failed to reset password: {ex.Message}");
            }
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<ApiResponse> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _context.UserAccounts.FindAsync(userId);
                if (user == null)
                {
                    return ApiResponse.ErrorResult("Không tìm thấy người dùng");
                }

                // Check for related data that would prevent deletion
                var relatedDataErrors = new List<string>();

                // Check topics
                var topicCount = await _context.Topics.CountAsync(t => t.UserId == userId);
                if (topicCount > 0)
                {
                    relatedDataErrors.Add($"{topicCount} chủ đề");
                }

                // Check posts
                var postCount = await _context.Posts.CountAsync(p => p.UserId == userId);
                if (postCount > 0)
                {
                    relatedDataErrors.Add($"{postCount} bài viết");
                }

                // Check likes
                var likeCount = await _context.Likes.CountAsync(l => l.UserId == userId);
                if (likeCount > 0)
                {
                    relatedDataErrors.Add($"{likeCount} lượt thích");
                }

                // Check reports
                var reportCount = await _context.Reports.CountAsync(r => r.UserId == userId);
                if (reportCount > 0)
                {
                    relatedDataErrors.Add($"{reportCount} báo cáo");
                }

                // Check topic views
                var viewCount = await _context.TopicViews.CountAsync(tv => tv.UserId == userId);
                if (viewCount > 0)
                {
                    relatedDataErrors.Add($"{viewCount} lượt xem");
                }

                // If there are related data, return detailed error
                if (relatedDataErrors.Any())
                {
                    var errorMessage = $"Không thể xóa người dùng '{user.FullName}' vì vẫn còn dữ liệu liên quan:\n" +
                                     $"- {string.Join("\n- ", relatedDataErrors)}\n\n" +
                                     "Vui lòng xóa hoặc chuyển quyền sở hữu các dữ liệu này trước khi xóa người dùng.";
                    
                    return ApiResponse.ErrorResult(errorMessage);
                }

                // If no related data, proceed with deletion
                _context.UserAccounts.Remove(user);
                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult($"Đã xóa người dùng '{user.FullName}' thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Lỗi khi xóa người dùng: {ex.Message}");
            }
        }
    }
}
