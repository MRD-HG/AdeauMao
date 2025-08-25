using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AdeauMao.Application.Services;
using AdeauMao.Application.DTOs.Auth;

namespace AdeauMao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticate user and return JWT token
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>Authentication response with token</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.LoginAsync(loginDto);
                
                if (result.Success)
                {
                    _logger.LogInformation("User {UserName} logged in successfully", loginDto.UserName);
                    return Ok(result);
                }

                _logger.LogWarning("Failed login attempt for user {UserName}", loginDto.UserName);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user {UserName}", loginDto.UserName);
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Erreur interne du serveur"
                });
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerDto">Registration information</param>
        /// <returns>Authentication response with token</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RegisterAsync(registerDto);
                
                if (result.Success)
                {
                    _logger.LogInformation("User {UserName} registered successfully", registerDto.UserName);
                    return Ok(result);
                }

                _logger.LogWarning("Failed registration attempt for user {UserName}", registerDto.UserName);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {UserName}", registerDto.UserName);
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Erreur interne du serveur"
                });
            }
        }

        /// <summary>
        /// Refresh JWT token
        /// </summary>
        /// <param name="refreshTokenDto">Refresh token information</param>
        /// <returns>New authentication response with refreshed token</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(AuthResponseDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RefreshTokenAsync(refreshTokenDto.Token);
                
                if (result.Success)
                {
                    _logger.LogInformation("Token refreshed successfully");
                    return Ok(result);
                }

                _logger.LogWarning("Failed token refresh attempt");
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Erreur interne du serveur"
                });
            }
        }

        /// <summary>
        /// Revoke JWT token (logout)
        /// </summary>
        /// <param name="token">Token to revoke</param>
        /// <returns>Success status</returns>
        [HttpPost("revoke-token")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RevokeToken([FromBody] string token)
        {
            try
            {
                var result = await _authService.RevokeTokenAsync(token);
                
                if (result)
                {
                    _logger.LogInformation("Token revoked successfully");
                    return Ok(new { message = "Token révoqué avec succès" });
                }

                return BadRequest(new { message = "Erreur lors de la révocation du token" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token revocation");
                return StatusCode(500, new { message = "Erreur interne du serveur" });
            }
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="changePasswordDto">Password change information</param>
        /// <returns>Success status</returns>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.ChangePasswordAsync(changePasswordDto);
                
                if (result)
                {
                    _logger.LogInformation("Password changed successfully for user {UserId}", changePasswordDto.UserId);
                    return Ok(new { message = "Mot de passe modifié avec succès" });
                }

                return BadRequest(new { message = "Erreur lors de la modification du mot de passe" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password change for user {UserId}", changePasswordDto.UserId);
                return StatusCode(500, new { message = "Erreur interne du serveur" });
            }
        }

        /// <summary>
        /// Reset user password
        /// </summary>
        /// <param name="resetPasswordDto">Password reset information</param>
        /// <returns>Success status</returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.ResetPasswordAsync(resetPasswordDto);
                
                if (result)
                {
                    _logger.LogInformation("Password reset successfully for email {Email}", resetPasswordDto.Email);
                    return Ok(new { message = "Mot de passe réinitialisé avec succès" });
                }

                return BadRequest(new { message = "Erreur lors de la réinitialisation du mot de passe" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during password reset for email {Email}", resetPasswordDto.Email);
                return StatusCode(500, new { message = "Erreur interne du serveur" });
            }
        }

        /// <summary>
        /// Get current user information
        /// </summary>
        /// <returns>Current user information</returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserInfoDto), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var roles = await _authService.GetUserRolesAsync(userId);
                var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

                var userInfo = new UserInfoDto
                {
                    Id = userId,
                    UserName = userName ?? "",
                    Email = email ?? "",
                    Roles = roles
                };

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user information");
                return StatusCode(500, new { message = "Erreur interne du serveur" });
            }
        }

        /// <summary>
        /// Assign role to user (Admin only)
        /// </summary>
        /// <param name="roleAssignmentDto">Role assignment information</param>
        /// <returns>Success status</returns>
        [HttpPost("assign-role")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentDto roleAssignmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.AssignRoleAsync(roleAssignmentDto.UserId, roleAssignmentDto.RoleName);
                
                if (result)
                {
                    _logger.LogInformation("Role {RoleName} assigned to user {UserId}", roleAssignmentDto.RoleName, roleAssignmentDto.UserId);
                    return Ok(new { message = "Rôle assigné avec succès" });
                }

                return BadRequest(new { message = "Erreur lors de l'assignation du rôle" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role {RoleName} to user {UserId}", roleAssignmentDto.RoleName, roleAssignmentDto.UserId);
                return StatusCode(500, new { message = "Erreur interne du serveur" });
            }
        }

        /// <summary>
        /// Remove role from user (Admin only)
        /// </summary>
        /// <param name="roleAssignmentDto">Role removal information</param>
        /// <returns>Success status</returns>
        [HttpPost("remove-role")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> RemoveRole([FromBody] RoleAssignmentDto roleAssignmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authService.RemoveRoleAsync(roleAssignmentDto.UserId, roleAssignmentDto.RoleName);
                
                if (result)
                {
                    _logger.LogInformation("Role {RoleName} removed from user {UserId}", roleAssignmentDto.RoleName, roleAssignmentDto.UserId);
                    return Ok(new { message = "Rôle retiré avec succès" });
                }

                return BadRequest(new { message = "Erreur lors du retrait du rôle" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing role {RoleName} from user {UserId}", roleAssignmentDto.RoleName, roleAssignmentDto.UserId);
                return StatusCode(500, new { message = "Erreur interne du serveur" });
            }
        }
    }
}

