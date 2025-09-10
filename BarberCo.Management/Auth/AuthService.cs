using BarberCo.Management.LocalStorage;
using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BarberCo.Management.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IBarberCoApiService _apiService;
        private readonly ILocalStorageService _localStorage;
        private readonly CustomAuthenticationStateProvider _authStateProvider;

        public AuthService(IBarberCoApiService apiService, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _apiService = apiService;
            _localStorage = localStorage;
            _authStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
        }

        public async Task LoginAsync(string username, string password)
        {
            var tokenDto = await _apiService.PostAsync<LoginDto, TokenDto>("auth/login",
                    new LoginDto { Username = username, Password = password });

            await _localStorage.SetItemAsync(StorageKeys.AuthToken, tokenDto);
            _authStateProvider.NotifyUserAuthentication();
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync(StorageKeys.AuthToken);
            _authStateProvider.NotifyUserLogout();
        }

        public async Task<TokenDto?> GetCurrentTokenAsync()
        {
            return await _localStorage.GetItemAsync<TokenDto>(StorageKeys.AuthToken);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetCurrentTokenAsync();
            return token != null && token.Expires > DateTime.UtcNow;
        }
    }
}
