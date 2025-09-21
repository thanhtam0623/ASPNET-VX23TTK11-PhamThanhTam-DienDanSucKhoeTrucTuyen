using ApiApplication.Models.DTOs.User;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.User
{
    public interface IUserAuthService
    {
        Task<ApiResponse<UserLoginResponse>> RegisterAsync(UserRegisterRequest request);
        Task<ApiResponse<UserLoginResponse>> LoginAsync(UserLoginRequest request);
        Task<ApiResponse> ForgotPasswordAsync(UserForgotPasswordRequest request);
        Task<ApiResponse> ResetPasswordAsync(UserResetPasswordRequest request);
        Task<ApiResponse> ChangePasswordAsync(int userId, UserChangePasswordRequest request);
        Task<ApiResponse<UserProfileDTO>> GetProfileAsync(int userId);
        Task<ApiResponse> VerifyEmailAsync(string token);
    }
}
