using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdeauMao.Application.Services;
using AdeauMao.Application.DTOs.Auth;

namespace AdeauMao.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(loginDto.UserName);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(loginDto.UserName);
                }

                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Nom d'utilisateur ou mot de passe incorrect"
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Nom d'utilisateur ou mot de passe incorrect"
                    };
                }

                var token = await GenerateJwtTokenAsync(user);
                var refreshToken = GenerateRefreshToken();
                var roles = await _userManager.GetRolesAsync(user);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Connexion réussie",
                    Token = token,
                    RefreshToken = refreshToken,
                    TokenExpiration = DateTime.UtcNow.AddHours(GetTokenExpiryHours()),
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        UserName = user.UserName ?? "",
                        Email = user.Email ?? "",
                        Roles = roles
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Erreur lors de la connexion",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(registerDto.UserName);
                if (existingUser != null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Ce nom d'utilisateur existe déjà"
                    };
                }

                existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Cette adresse email est déjà utilisée"
                    };
                }

                var user = new IdentityUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Erreur lors de la création du compte",
                        Errors = result.Errors.Select(e => e.Description)
                    };
                }

                // Assign role
                if (!string.IsNullOrEmpty(registerDto.Role))
                {
                    if (await _roleManager.RoleExistsAsync(registerDto.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerDto.Role);
                    }
                }

                var token = await GenerateJwtTokenAsync(user);
                var refreshToken = GenerateRefreshToken();
                var roles = await _userManager.GetRolesAsync(user);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Compte créé avec succès",
                    Token = token,
                    RefreshToken = refreshToken,
                    TokenExpiration = DateTime.UtcNow.AddHours(GetTokenExpiryHours()),
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = registerDto.FirstName,
                        LastName = registerDto.LastName,
                        Roles = roles
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Erreur lors de la création du compte",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                if (principal == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Token invalide"
                    };
                }

                var username = principal.Identity?.Name;
                var user = await _userManager.FindByNameAsync(username ?? "");
                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Utilisateur non trouvé"
                    };
                }

                var newToken = await GenerateJwtTokenAsync(user);
                var newRefreshToken = GenerateRefreshToken();
                var roles = await _userManager.GetRolesAsync(user);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Token rafraîchi avec succès",
                    Token = newToken,
                    RefreshToken = newRefreshToken,
                    TokenExpiration = DateTime.UtcNow.AddHours(GetTokenExpiryHours()),
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        UserName = user.UserName ?? "",
                        Email = user.Email ?? "",
                        Roles = roles
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Erreur lors du rafraîchissement du token",
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            try
            {
                // In a real implementation, you would store revoked tokens in a database
                // For now, we'll just return true
                await Task.CompletedTask;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(changePasswordDto.UserId);
                if (user == null)
                {
                    return false;
                }

                var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                if (user == null)
                {
                    return false;
                }

                var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new List<string>();
                }

                return await _userManager.GetRolesAsync(user);
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<bool> AssignRoleAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    return false;
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveRoleAsync(string userId, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }

                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> GenerateJwtTokenAsync(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(GetTokenExpiryHours()),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token invalide");
            }

            return principal;
        }

        private int GetTokenExpiryHours()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            return int.TryParse(jwtSettings["ExpiryInHours"], out var hours) ? hours : 24;
        }
    }
}

