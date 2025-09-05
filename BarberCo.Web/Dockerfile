# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Only expose port 80 - Azure Container Apps handles HTTPS
EXPOSE 80

# Copy published output
COPY --from=publish /app/publish .

# Set the app to listen on port 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "BarberCo.Web.dll"]