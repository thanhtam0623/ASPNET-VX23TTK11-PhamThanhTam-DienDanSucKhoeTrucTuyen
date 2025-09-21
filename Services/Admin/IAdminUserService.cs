using ApiApplication.Models.DTOs.Admin;
using ApiApplication.Models.DTOs.Common;

namespace ApiApplication.Services.Admin
{
    public interface IAdminUserService
    {
        Task<ApiResponse<PaginationResponse<AdminUserListDTO>>> GetUsersAsync(PaginationRequest pagination, AdminUserFilterRequest? filter = null);
        Task<ApiResponse<AdminUserListDTO>> GetUserByIdAsync(int userId);
        Task<ApiResponse<AdminUserStatsDTO>> GetUserStatsAsync();
        Task<ApiResponse<AdminUserListDTO>> CreateUserAsync(AdminCreateUserRequest request);
        Task<ApiResponse<AdminUserListDTO>> UpdateUserAsync(int userId, AdminUpdateUserRequest request);
        Task<ApiResponse> UpdateUserRoleAsync(int userId, AdminUpdateUserRoleRequest request);
        Task<ApiResponse> ToggleUserStatusAsync(int userId);
        Task<ApiResponse<object>> ResetUserPasswordAsync(AdminResetPasswordRequest request);
        Task<ApiResponse> DeleteUserAsync(int userId);
    }
}
