using BarberCo.Api.Auth;
using BarberCo.DataAccess;
using BarberCo.DataAccess.Repositories;
using BarberCo.SharedLibrary.Models;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.AspNetCore.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IBarberRepo, BarberRepo>();
builder.Services.AddTransient<IHourRepo, HourRepo>();
builder.Services.AddTransient<IServiceRepo, ServiceRepo>();
builder.Services.AddTransient<IAppointmentRepo, AppointmentRepo>();
builder.Services.AddScoped<JwtHelper>();

#if DEBUG == false
builder.Logging
    .AddApplicationInsights(
    x => x.ConnectionString= builder.Configuration.GetSection("ApplicationInsights:ConnectionString").Get<string>(),
    y => y.IncludeScopes = true);
#endif

var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
var issuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var audience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();

builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
            ValidIssuer = issuer,
            ValidAudience = audience,
            ValidateIssuer = true,
            ValidateAudience = true,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
        };
    }).AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", options => { });

builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
    options.AddPolicy("ManagementApp_WASM", policy =>
    {
        policy.WithOrigins(allowedOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddIdentityCore<Barber>()
           .AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<DataContext>()
           .AddSignInManager();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("ManagementApp_WASM");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
