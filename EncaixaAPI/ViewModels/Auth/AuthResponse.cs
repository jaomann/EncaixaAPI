namespace EncaixaAPI.ViewModels.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
