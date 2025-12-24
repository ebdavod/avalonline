using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.User;
using AvvalOnline.Shop.Api.Model.Entites;

namespace AvvalOnline.Shop.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserDTO> AuthenticateAsync(string username, string password);
        Task<string> GetUserTokenAsync(string username, string password);
        Task<bool> HasPermissionAsync(int userId, string permissionName);
        Task<bool> HasRoleAsync(int userId, string roleName);
        Task<List<string>> GetUserPermissionsAsync(int userId);
        Task<List<string>> GetUserRolesAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
    }
}
