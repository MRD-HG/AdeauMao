using System.ComponentModel.DataAnnotations;

namespace AdeauMao.Application.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }

    public class RegisterDto
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom d'utilisateur doit contenir entre 3 et 50 caractères")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmation du mot de passe est requise")]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Role { get; set; } = "Operator";
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public UserInfoDto? User { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }

    public class UserInfoDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }

    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "L'ID utilisateur est requis")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe actuel est requis")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nouveau mot de passe est requis")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmation du nouveau mot de passe est requise")]
        [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le token est requis")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nouveau mot de passe est requis")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmation du nouveau mot de passe est requise")]
        [Compare("NewPassword", ErrorMessage = "Les mots de passe ne correspondent pas")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }

    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "Le token est requis")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le refresh token est requis")]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RoleAssignmentDto
    {
        [Required(ErrorMessage = "L'ID utilisateur est requis")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom du rôle est requis")]
        public string RoleName { get; set; } = string.Empty;
    }
}

