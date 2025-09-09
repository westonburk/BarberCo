using BarberCo.SharedLibrary.Dtos;

namespace BarberCo.Management.Auth
{
    public interface IAuthService
    {
        Task LoginAsync(string username, string password);
        Task LogoutAsync();
        Task<TokenDto?> GetCurrentTokenAsync();
        Task<bool> IsAuthenticatedAsync();
    }
}
