using ApiApplication.Data;
using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.Admin
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AdminAuthService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<AdminLoginResponse>> LoginAsync(AdminLoginRequest request)
        {
            try
            {
                var admin = await _context.AdminAccounts
                    .FirstOrDefaultAsync(a => a.Username == request.Username && a.IsActive);

                if (admin == null || !BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
                {
                    return ApiResponse<AdminLoginResponse>.ErrorResult("Invalid username or password");
                }

                // Update last login
                admin.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateAdminToken(admin.Id, admin.Username, admin.Role);
                
                var response = new AdminLoginResponse
                {
                    Token = token,
                    Admin = new AdminProfileDTO
                    {
                        Id = admin.Id,
                        Username = admin.Username,
                        Email = admin.Email,
                        FullName = admin.FullName,
                        Avatar = admin.Avatar,
                        Role = admin.Role,
                        IsActive = admin.IsActive,
                        CreatedAt = admin.CreatedAt,
                        LastLoginAt = admin.LastLoginAt
                    }
                };

                return ApiResponse<AdminLoginResponse>.SuccessResult(response, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminLoginResponse>.ErrorResult($"Login failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminProfileDTO>> GetProfileAsync(int adminId)
        {
            try
            {
                var admin = await _context.AdminAccounts
                    .FirstOrDefaultAsync(a => a.Id == adminId);

                if (admin == null)
                {
                    return ApiResponse<AdminProfileDTO>.ErrorResult("Admin not found");
                }

                var profile = new AdminProfileDTO
                {
                    Id = admin.Id,
                    Username = admin.Username,
                    Email = admin.Email,
                    FullName = admin.FullName,
                    Avatar = admin.Avatar,
                    Role = admin.Role,
                    IsActive = admin.IsActive,
                    CreatedAt = admin.CreatedAt,
                    LastLoginAt = admin.LastLoginAt
                };

                return ApiResponse<AdminProfileDTO>.SuccessResult(profile);
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminProfileDTO>.ErrorResult($"Failed to get profile: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ChangePasswordAsync(int adminId, AdminChangePasswordRequest request)
        {
            try
            {
                var admin = await _context.AdminAccounts
                    .FirstOrDefaultAsync(a => a.Id == adminId);

                if (admin == null)
                {
                    return ApiResponse.ErrorResult("Admin not found");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, admin.PasswordHash))
                {
                    return ApiResponse.ErrorResult("Current password is incorrect");
                }

                admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                admin.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Password changed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to change password: {ex.Message}");
            }
        }

        public async Task<ApiResponse<AdminProfileDTO>> UpdateProfileAsync(int adminId, AdminProfileDTO profile)
        {
            try
            {
                var admin = await _context.AdminAccounts
                    .FirstOrDefaultAsync(a => a.Id == adminId);

                if (admin == null)
                {
                    return ApiResponse<AdminProfileDTO>.ErrorResult("Admin not found");
                }

                admin.FullName = profile.FullName;
                admin.Email = profile.Email;
                admin.Avatar = profile.Avatar;
                admin.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var updatedProfile = new AdminProfileDTO
                {
                    Id = admin.Id,
                    Username = admin.Username,
                    Email = admin.Email,
                    FullName = admin.FullName,
                    Avatar = admin.Avatar,
                    Role = admin.Role,
                    IsActive = admin.IsActive,
                    CreatedAt = admin.CreatedAt,
                    LastLoginAt = admin.LastLoginAt
                };

                return ApiResponse<AdminProfileDTO>.SuccessResult(updatedProfile, "Profile updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AdminProfileDTO>.ErrorResult($"Failed to update profile: {ex.Message}");
            }
        }
    }
}
