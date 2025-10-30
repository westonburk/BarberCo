# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["BarberCo.Web/BarberCo.Web.csproj", "BarberCo.Web/"]
COPY ["BarberCo.SharedLibrary/BarberCo.SharedLibrary.csproj", "BarberCo.SharedLibrary/"]
RUN dotnet restore "BarberCo.Web/BarberCo.Web.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/BarberCo.Web"
RUN dotnet build "BarberCo.Web.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BarberCo.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create non-root user
RUN useradd -m -s /usr/sbin/nologin appuser

# Copy published output and fix ownership
COPY --from=publish /app/publish .
RUN chown -R appuser:appuser /app

# Listen on 80; Nginx will proxy 8081->80 on host
EXPOSE 80

# Respect X-Forwarded-* from Nginx (scheme/host), important for redirects and links
ENV ASPNETCORE_URLS=http://+:80 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

USER appuser

ENTRYPOINT ["dotnet", "BarberCo.Web.dll"]