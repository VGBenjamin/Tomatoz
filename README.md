# Tomatoz 🍅

A comprehensive tomato varieties encyclopedia built with ASP.NET Core 9, Blazor, and Entity Framework Core.

## Features

- **User Authentication**: Local accounts with external provider support (Google, Microsoft, GitHub)
- **Tomato Variety Management**: Browse, search, and manage tomato varieties
- **Modern UI**: Built with Blazor Server and WebAssembly hybrid approach
- **API Support**: RESTful API with OpenAPI documentation
- **Clean Architecture**: Separation of concerns with Application, Infrastructure, and Shared layers

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server or LocalDB
- Visual Studio 2022 or VS Code

### Quick Start

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Tomatoz
   ```

2. **Set up the database**
   ```bash
   cd src/Tomatoz/Tomatoz
   dotnet ef database update
   ```

3. **Run the application**
   ```bash
   dotnet run --project src/Tomatoz.AppHost
   ```

4. **Access the application**
   - Web Application: `https://localhost:7154`
   - API Documentation: `https://localhost:7154/scalar/v1`

## External Authentication Setup

The application supports external authentication providers. To set them up:

### Quick Setup (Recommended)

Run the setup script:

**Windows (PowerShell):**
```powershell
.\setup-auth.ps1
```

**Linux/macOS (Bash):**
```bash
chmod +x setup-auth.sh
./setup-auth.sh
```

### Manual Setup

See the detailed guide: [Doc/external-authentication-setup.md](Doc/external-authentication-setup.md)

## Project Structure

```
├── src/
│   ├── Tomatoz/
│   │   ├── Tomatoz/           # Main web application (Blazor Server + WebAssembly)
│   │   └── Tomatoz.Client/    # Blazor WebAssembly client
│   ├── Tomatoz.AppHost/       # .NET Aspire app host
│   ├── Tomatoz.Application/   # Application layer (services, DTOs)
│   ├── Tomatoz.Infrastructure/ # Infrastructure layer (data access, external services)
│   ├── Tomatoz.ServiceDefaults/ # Shared service configurations
│   ├── Tomatoz.Shared/        # Shared models and contracts
│   └── Tomatoz.Tests.Unit/    # Unit tests
├── Doc/                       # Documentation
└── setup-auth.*              # Authentication setup scripts
```

## Technology Stack

- **Backend**: ASP.NET Core 9, Entity Framework Core
- **Frontend**: Blazor Server & WebAssembly
- **Database**: SQL Server / LocalDB
- **Authentication**: ASP.NET Core Identity with external providers
- **API Documentation**: Scalar (OpenAPI)
- **Architecture**: Clean Architecture with CQRS patterns
- **Hosting**: .NET Aspire for orchestration

## Development

### Database Migrations

Create a new migration:
```bash
cd src/Tomatoz/Tomatoz
dotnet ef migrations add <MigrationName>
```

Update the database:
```bash
dotnet ef database update
```

### Running Tests

```bash
dotnet test
```

### Building for Production

```bash
dotnet publish src/Tomatoz/Tomatoz -c Release
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Documentation

- [External Authentication Setup](Doc/external-authentication-setup.md)
- [Technical Analysis](Doc/analyse_technique_tomatoz.md)
- [Tomato Wiki Specifications](Doc/specifications_wiki_tomates.md)

## Support

For issues and questions, please use the GitHub Issues section of this repository.
