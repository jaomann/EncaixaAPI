using EncaixaAPI.Core.Contracts;
using EncaixaAPI.Core.Entities;
using EncaixaAPI.Database;
using EncaixaAPI.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EncaixaAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IWebHostEnvironment _env;  
        private readonly Context _context;

        public AuthController(IAuthService authService, Context context, ILogger<AuthController> logger, IWebHostEnvironment env)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
            _env = env;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var authResponse = await _authService.Authenticate(loginDto);

            if (authResponse == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            return Ok(authResponse);
        }

        [HttpPost("setup-demo")]
        [AllowAnonymous]
        public async Task<IActionResult> SetupDemoUser()
        {
            try
            {
                if (!_env.IsDevelopment())
                {
                    return BadRequest("Endpoint disponível apenas em desenvolvimento");
                }
                // Verifica se já existe usuário demo
                if (await _context.Users.AnyAsync(u => u.Username == "demo"))
                {
                    return BadRequest(new
                    {
                        Message = "Usuário demo já existe",
                        Suggestion = "Use username: 'demo', password: 'Demo@123'"
                    });
                }

                // Cria usuário demo
                var demoUser = new User
                {
                    Username = "demo",
                    Email = "demo@encaixa.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Demo@123"),
                    Role = "Admin"
                };

                _context.Users.Add(demoUser);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuário demo criado com sucesso");

                return Ok(new
                {
                    Message = "Usuário demo criado",
                    Credentials = new
                    {
                        Username = "demo",
                        Password = "Demo@123",
                        Role = "Admin"
                    },
                    Greetings = "Boas vindas!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar usuário demo");
                return StatusCode(500, new
                {
                    Error = "Falha ao configurar demo",
                    Details = ex.Message
                });
            }
        }
    }
}
