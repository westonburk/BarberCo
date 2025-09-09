using BarberCo.Management.LocalStorage;
using BarberCo.SharedLibrary.Dtos;
using Blazored.LocalStorage;

namespace BarberCo.Management.Auth
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthorizationMessageHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Get token from local storage
            var tokenDto = await _localStorage.GetItemAsync<TokenDto>(StorageKeys.AuthToken);

            if (tokenDto != null && tokenDto.Expires > DateTime.UtcNow)
            {
                // Add JWT to Authorization header
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenDto.Token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
