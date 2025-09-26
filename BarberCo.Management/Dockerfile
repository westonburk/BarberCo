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
RUN dotnet build "BarberCo.Management.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BarberCo.Management.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage - Use nginx to serve the Blazor WebAssembly app
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Remove default nginx static assets
RUN rm -rf ./*

# Copy the published Blazor WebAssembly files
COPY --from=publish /app/publish/wwwroot .

# Copy nginx configuration
COPY BarberCo.Management/nginx.conf /etc/nginx/nginx.conf

# Expose port 80
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]