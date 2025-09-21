using ApiApplication.Data;
using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;
using ApiApplication.Models.Entities;
using ApiApplication.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Services.User
{
    public class UserAuthService : IUserAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public UserAuthService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<UserLoginResponse>> RegisterAsync(UserRegisterRequest request)
        {
            try
            {
                // Check if username or email exists
                var existingUser = await _context.UserAccounts
                    .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

                if (existingUser != null)
                {
                    return ApiResponse<UserLoginResponse>.ErrorResult("Username or email already exists");
                }

                var user = new UserAccount
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    FullName = request.FullName,
                    Role = "Member",
                    IsActive = true,
                    IsEmailVerified = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.UserAccounts.Add(user);
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateUserToken(user.Id, user.Username, user.Role);

                var response = new UserLoginResponse
                {
                    Token = token,
                    User = new UserProfileDTO
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
                        TopicCount = 0,
                        PostCount = 0,
                        LikeCount = 0
                    }
                };

                return ApiResponse<UserLoginResponse>.SuccessResult(response, "Registration successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserLoginResponse>.ErrorResult($"Registration failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<UserLoginResponse>> LoginAsync(UserLoginRequest request)
        {
            try
            {
                var user = await _context.UserAccounts
                    .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return ApiResponse<UserLoginResponse>.ErrorResult("Invalid username or password");
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateUserToken(user.Id, user.Username, user.Role);

                var response = new UserLoginResponse
                {
                    Token = token,
                    User = new UserProfileDTO
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
                    }
                };

                return ApiResponse<UserLoginResponse>.SuccessResult(response, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserLoginResponse>.ErrorResult($"Login failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ForgotPasswordAsync(UserForgotPasswordRequest request)
        {
            try
            {
                var user = await _context.UserAccounts
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    // Don't reveal if email exists
                    return ApiResponse.SuccessResult("If the email exists, a reset token has been sent");
                }

                // Generate reset token
                var resetToken = Guid.NewGuid().ToString();
                user.ResetPasswordToken = resetToken;
                user.ResetPasswordExpires = DateTime.UtcNow.AddHours(24);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // TODO: Send email with reset token
                return ApiResponse.SuccessResult("If the email exists, a reset token has been sent");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to process request: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ResetPasswordAsync(UserResetPasswordRequest request)
        {
            try
            {
                var user = await _context.UserAccounts
                    .FirstOrDefaultAsync(u => u.ResetPasswordToken == request.Token && 
                                            u.ResetPasswordExpires > DateTime.UtcNow);

                if (user == null)
                {
                    return ApiResponse.ErrorResult("Invalid or expired reset token");
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.ResetPasswordToken = null;
                user.ResetPasswordExpires = null;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Password reset successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to reset password: {ex.Message}");
            }
        }

        public async Task<ApiResponse> ChangePasswordAsync(int userId, UserChangePasswordRequest request)
        {
            try
            {
                var user = await _context.UserAccounts.FindAsync(userId);
                if (user == null)
                {
                    return ApiResponse.ErrorResult("User not found");
                }

                if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
                {
                    return ApiResponse.ErrorResult("Current password is incorrect");
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return ApiResponse.SuccessResult("Password changed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.ErrorResult($"Failed to change password: {ex.Message}");
            }
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

        public Task<ApiResponse> VerifyEmailAsync(string token)
        {
            try
            {
                // TODO: Implement email verification logic
                return Task.FromResult(ApiResponse.SuccessResult("Email verified successfully"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ApiResponse.ErrorResult($"Failed to verify email: {ex.Message}"));
            }
        }
    }
}
