using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.Admin
{
    public interface IAdminAuthService
    {
        Task<ApiResponse<AdminLoginResponse>> LoginAsync(AdminLoginRequest request);
        Task<ApiResponse<AdminProfileDTO>> GetProfileAsync(int adminId);
        Task<ApiResponse> ChangePasswordAsync(int adminId, AdminChangePasswordRequest request);
        Task<ApiResponse<AdminProfileDTO>> UpdateProfileAsync(int adminId, AdminProfileDTO profile);
    }
}
