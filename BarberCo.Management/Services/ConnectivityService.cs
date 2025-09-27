using Microsoft.JSInterop;

namespace BarberCo.Management.Services
{
    public class ConnectivityService : IConnectivityService, IAsyncDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<ConnectivityService>? _dotNetReference;

        public bool IsOnline { get; private set; } = true;
        public event Action<bool>? ConnectivityChanged;

        public ConnectivityService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeAsync()
        {
            _dotNetReference = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync("network.initialize", _dotNetReference);
        }

        [JSInvokable("Network.StatusChanged")]
        public void OnStatusChanged(bool isOnline)
        {
            if (IsOnline != isOnline)
            {
                IsOnline = isOnline;
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
