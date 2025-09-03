# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Only expose port 80 - Azure Container Apps handles HTTPS
EXPOSE 80

# Copy published output
COPY --from=publish /app/publish .

# Set the app to listen on port 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "BarberCo.Api.dll"]