using EncaixaAPI.Core.Entities;
using EncaixaAPI.ViewModels;

namespace EncaixaAPI.Core.Contracts
{
    public interface IAuthService
    {
        public Task<AuthResponse> Authenticate(LoginDto loginDto);
    }
}
