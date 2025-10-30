# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["BarberCo.Api/BarberCo.Api.csproj", "BarberCo.Api/"]
COPY ["BarberCo.DataAccess/BarberCo.DataAccess.csproj", "BarberCo.DataAccess/"]
COPY ["BarberCo.SharedLibrary/BarberCo.SharedLibrary.csproj", "BarberCo.SharedLibrary/"]
RUN dotnet restore "BarberCo.Api/BarberCo.Api.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/BarberCo.Api"
RUN dotnet build "BarberCo.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BarberCo.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create non-root user
RUN useradd -m -s /usr/sbin/nologin appuser
# Copy published output and fix ownership
COPY --from=publish /app/publish .
RUN chown -R appuser:appuser /app

# App listens on 80; Nginx will reverse-proxy to the host's 8080->container:80
EXPOSE 80

# Enable forwarded headers so app knows it's behind a proxy (scheme/host)
ENV ASPNETCORE_URLS=http://+:80 \
    ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

USER appuser

ENTRYPOINT ["dotnet", "BarberCo.Api.dll"]