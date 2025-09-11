# Timo's Trattoria - Restaurant Management MVC

A modern ASP.NET Core MVC application for managing restaurant operations including bookings, tables, and menu items.

## Features

- 🍝 **Menu Management**: Display and manage restaurant menu items with categories
- 📅 **Booking System**: Create, update, and manage customer reservations
- 🪑 **Table Management**: Configure restaurant tables and their capacities
- 👨‍🍳 **Admin Authentication**: Secure admin panel with JWT authentication
- 📱 **Responsive Design**: Bootstrap-based UI that works on all devices

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Backend API running (see [BookingSystem API](https://github.com/timsuv/BookingSystem))

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <your-repo-url>
cd RestautantMvc
```

### 2. Configure App Settings

Create an `appsettings.json` file in the root directory with the following configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AdminRegistration": {
    "Code": "AdminSecret123!"
  }
}
```

**Important**: The `AdminRegistration.Code` is required for admin registration. Change this to a secure secret code for production use.

### 3. Backend API Setup

This MVC application requires the [BookingSystem API](https://github.com/timsuv/BookingSystem) to be running.

1. Clone and setup the backend API from: https://github.com/timsuv/BookingSystem
2. Ensure the API is running on `https://localhost:7189`
3. If your API runs on a different port, update the `HttpClient` configuration in `Program.cs`:

```csharp
builder.Services.AddHttpClient("RestoApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:YOUR_PORT/api/");
});
```

### 4. Run the Application

```bash
dotnet restore
dotnet run
```

The application will be available at:
- HTTP: `http://localhost:5224`
- HTTPS: `https://localhost:7129`

## Admin Access

### First Time Setup

1. Navigate to `/Auth/Register`
2. Use the secret code configured in `appsettings.json` (`AdminSecret123!` by default)
3. Create your admin account
4. Login with your credentials

### Admin Features

Once logged in as admin, you can:
- Manage restaurant bookings
- Add/edit/delete tables
- Manage menu items and categories
- View booking statistics

## Project Structure

```
├── Controllers/          # MVC Controllers
├── DTOs/                # Data Transfer Objects
├── Models/              # Domain Models
├── Services/            # API Service Layer
├── ViewComponents/      # Reusable View Components
├── ViewModels/          # View Models
├── Views/               # Razor Views
└── wwwroot/            # Static files (CSS, JS, images)
```

## API Integration

The application integrates with the backend API through service classes:
- `AuthService`: Handles admin authentication
- `BookingApiService`: Manages booking operations
- `MenuApiService`: Handles menu item CRUD operations
- `TableApiService`: Manages restaurant tables

## Environment Configuration

- **Development**: Uses `appsettings.Development.json`
- **Production**: Ensure to set secure values for production deployment

## Security Notes

- Change the `AdminRegistration.Code` for production
- The application uses session-based authentication
- JWT tokens are stored in session storage
- All admin operations require authentication

## Troubleshooting

**API Connection Issues:**
- Ensure the backend API is running
- Check the API base URL in `Program.cs`
- Verify CORS settings on the API

**Admin Registration Issues:**
- Verify the secret code in `appsettings.json`
- Check that the API `/Auth/register` endpoint is accessible

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is for educational/demonstration purposes.
