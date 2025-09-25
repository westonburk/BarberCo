namespace BarberCo.Management.Services
{
    public interface IConnectivityService
    {
        bool IsOnline { get; }
        event Action<bool> ConnectivityChanged;
        Task<bool> CheckConnectivityAsync();
        Task InitializeAsync();
    }
}
