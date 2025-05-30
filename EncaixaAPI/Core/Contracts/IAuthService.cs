using EncaixaAPI.Core.Entities;
using EncaixaAPI.ViewModels.Auth;

namespace EncaixaAPI.Core.Contracts
{
    public interface IAuthService
    {
        public Task<AuthResponse> Authenticate(LoginDto loginDto);
    }
}
