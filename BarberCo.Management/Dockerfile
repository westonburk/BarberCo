# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["BarberCo.Management/BarberCo.Management.csproj", "BarberCo.Management/"]
COPY ["BarberCo.SharedLibrary/BarberCo.SharedLibrary.csproj", "BarberCo.SharedLibrary/"]
RUN dotnet restore "BarberCo.Management/BarberCo.Management.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/BarberCo.Management"
RUN dotnet publish "BarberCo.Management.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage - Nginx serves the static WASM
FROM nginx:1.25-alpine AS final
WORKDIR /usr/share/nginx/html

# Clean default site
RUN rm -rf ./*

# Copy published static files
COPY --from=build /app/publish/wwwroot .

# Nginx config (should handle caching, compression, SPA fallback)
COPY BarberCo.Management/nginx.conf /etc/nginx/nginx.conf

# Permissions (nginx user exists in this image)
RUN chown -R nginx:nginx /usr/share/nginx/html

EXPOSE 80

# Optional healthcheck
HEALTHCHECK --interval=30s --timeout=3s CMD wget -qO- http://127.0.0.1/ || exit 1

CMD ["nginx", "-g", "daemon off;"]