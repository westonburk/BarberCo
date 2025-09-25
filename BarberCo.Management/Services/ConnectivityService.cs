using Microsoft.JSInterop;

namespace BarberCo.Management.Services
{
    public class ConnectivityService : IConnectivityService, IAsyncDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<ConnectivityService>? _dotNetReference;
        private bool _isOnline = true;

        public bool IsOnline => _isOnline;
        public event Action<bool>? ConnectivityChanged;

        public ConnectivityService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            // Create a reference to this instance for JavaScript callbacks
            _dotNetReference = DotNetObjectReference.Create(this);

            // Initialize JavaScript connectivity monitoring
            await _jsRuntime.InvokeVoidAsync("connectivity.initialize", _dotNetReference);

            // Get initial connectivity status
            _isOnline = await _jsRuntime.InvokeAsync<bool>("connectivity.isOnline");
        }

        public async Task<bool> CheckConnectivityAsync()
        {
            try
            {
                _isOnline = await _jsRuntime.InvokeAsync<bool>("connectivity.checkConnectivity");
                return _isOnline;
            }
            catch
            {
                _isOnline = false;
                return false;
            }
        }

        [JSInvokable]
        public void UpdateConnectivityStatus(bool isOnline)
        {
            if (_isOnline != isOnline)
            {
                _isOnline = isOnline;
                ConnectivityChanged?.Invoke(isOnline);
            }
        }

        public async ValueTask DisposeAsync()
        {
            _dotNetReference?.Dispose();
            await Task.CompletedTask;
        }
    }
}
