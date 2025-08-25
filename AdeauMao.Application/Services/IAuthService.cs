using AdeauMao.Application.DTOs.Auth;

namespace AdeauMao.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        Task<bool> AssignRoleAsync(string userId, string roleName);
        Task<bool> RemoveRoleAsync(string userId, string roleName);
    }
}

