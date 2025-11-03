# BarberCo

A full-stack proof of concept application demonstrating modern web development with .NET, Blazor, and Azure cloud services.

## Overview

BarberCo is a complete barber shop management system consisting of three main components:
- **BarberCo.Web** - Customer-facing website for the fictitious "Mike's Barber Shop"
- **BarberCo.Management** - Administrative portal for barbers to manage appointments
- **BarberCo.Api** - RESTful API providing data for both frontend applications

## 🛠️ Tech Stack

### Backend
- **Framework**: .NET 8 Web API
- **ORM**: Entity Framework Core with Code-First Migrations
- **Database**: PostgreSQL Database
- **Authentication**: Dual auth system - JWT Bearer tokens & API Key authentication
- **Identity**: ASP.NET Core Identity with role-based authorization

### Frontend
- **Customer Portal**: Blazor Server with interactive components
- **Admin Portal**: Blazor WebAssembly (WASM) with offline capabilities
- **UI Framework**: MudBlazor component library (Admin), Pure CSS (Customer)
- **State Management**: Local storage caching for offline support

### Cloud & DevOps
- **Hosting**: Linux Cloud Server (fully containerized with Docker)
- **Architecture**: Clean architecture with repository pattern
- **Cross-Platform**: Multi-project solution with shared libraries

### Development Practices
- **API Design**: RESTful API with proper HTTP status codes
- **Security**: CORS configuration, secure token handling
- **Code Organization**: Separation of concerns across projects
- **Dependency Injection**: Built-in .NET DI container

## Live Demo

- 🌐 **Customer Website**: [https://web.barberco.westonburkholder.com/](https://web.barberco.westonburkholder.com/)
- 👨‍💼 **Management Portal**: [https://management.barberco.westonburkholder.com/](https://management.barberco.westonburkholder.com/)

### Test Credentials for Management Portal

| Username | Password | Role |
|----------|----------|------|
| JoeLouden | oak-water-cougarD4 | barber |
| MikeBrenman | slowly-wood-chocolateW9 | admin |

## Project Components

### BarberCo.Web
Customer-facing website where customers can:
- Book appointments
- View business hours
- Access contact information

**Tech Stack:**
- Blazor Server
- Pure HTML/CSS (no UI libraries)
- ApiKey Authentication with BarberCo.Api

![BarberCo Web Screenshot](images/barberco-web.png)

### BarberCo.Management
Administrative portal for barbers featuring:
- Appointment management dashboard
- Offline access capabilities
- Role-based access (admin/barber)

**Tech Stack:**
- Blazor WebAssembly (WASM)
- MudBlazor UI component library
- JWT Authentication with BarberCo.Api

![BarberCo Management Screenshot](images/barberco-management.png)

### BarberCo.Api
RESTful API serving both frontend applications

**Tech Stack:**
- .NET Core Web API
- Entity Framework Core (ORM)
- Entity Framework Core Identity for Authentication
- PostgreSQL Database
- Dual authentication schemes (JWT & ApiKey)

![BarberCo API Screenshot](images/barberco-api.png)

## Author

**Weston Burkholder**
- GitHub: [@westonburk](https://github.com/westonburk)
- Website: [westonburkholder.com](https://westonburkholder.com)

## License

This project is available for viewing and educational purposes. Please contact me for any other use.