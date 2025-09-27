using BarberCo.Management;
using BarberCo.Management.Auth;
using BarberCo.SharedLibrary.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add MudBlazor
builder.Services.AddMudServices();

// Add Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// Add Authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// Configure HttpClient with the handler
builder.Services.AddHttpClient("BarberCoAPI", client =>
{
#if DEBUG == false
    var baseUrl = builder.Configuration["ApiBaseUrl"];
#else
    var baseUrl = "https://localhost:7200/api/";
#endif

    client.BaseAddress = new Uri(baseUrl);
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddScoped<IBarberCoApiService, BarberCoApiService>();
builder.Services.AddScoped<IHourService, HourService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


await builder.Build().RunAsync();
