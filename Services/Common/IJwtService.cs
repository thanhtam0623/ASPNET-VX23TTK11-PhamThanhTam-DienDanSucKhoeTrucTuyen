namespace ApiApplication.Services.Common
{
    public interface IJwtService
    {
        string GenerateAdminToken(int adminId, string username, string role);
        string GenerateUserToken(int userId, string username, string role);
        bool ValidateToken(string token, bool isAdmin = false);
        int? GetUserIdFromToken(string token);
    }
}
