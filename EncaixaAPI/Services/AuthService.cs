using BCrypt.Net;
using EncaixaAPI.Core.Contracts;
using EncaixaAPI.Core.Entities;
using EncaixaAPI.Database;
using EncaixaAPI.ViewModels.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EncaixaAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthService> _logger;
        private readonly Context _context;

        public AuthService(IOptions<JwtSettings> jwtSettings, Context context, ILogger<AuthService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _context = context;
            _logger = logger;
        }

        public async Task<AuthResponse> Authenticate(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning($"Tentativa de login falha para: {loginDto.Username}");
                return null;
            }

            var token = GenerateJwtToken(user);
            return new AuthResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                Username = user.Username,
                Role = user.Role
            };
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                // Verifica se a senha fornecida corresponde ao hash armazenado
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }
            catch (SaltParseException)
            {
                _logger.LogError("Hash de senha inválido no banco de dados");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar senha");
                return false;
            }
        }
    }
}
