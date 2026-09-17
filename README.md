# IdeaTracker

## Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB, Express, or full instance)
- Node.js 18+ (for the client)

## Backend Configuration (IdeaTracker.Api)

The backend reads configuration from `appsettings.json`, `appsettings.{Environment}.json`, and environment variables (environment variables take precedence).

### Required Environment Variables

| Variable | Description | Example |
|---|---|---|
| `ConnectionStrings__DefaultConnection` | SQL Server connection string | `Server=(localdb)\mssqllocaldb;Database=IdeaTrackerDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True` |
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | `Development` |
| `SeedOnStartup` | Seeds sample data on startup if the database is empty | `true` / `false` |
| `AllowedOrigins` | Comma-separated list of allowed CORS origins for the client | `http://localhost:5173` |

> Note: `SQLCONNSTR_DefaultConnection` and `CUSTOMCONNSTR_DefaultConnection` are also supported as fallbacks (useful for Azure App Service deployments).

### Setting Environment Variables Locally
{ "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=IdeaTrackerDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True" }, "AllowedOrigins": "http://localhost:5173" }


## Client Configuration

The client needs to know the backend's API base URL.

### Required Environment Variable

| Variable | Description | Example |
|---|---|---|
| `VITE_API_BASE_URL` (or equivalent per framework) | Base URL of the IdeaTracker API | `http://localhost:5290/api` |

### Setting Up the Client Environment File

Create a `.env` file at the root of the client project (do not commit this file — add it to `.gitignore`):
.env VITE_API_BASE_URL=http://localhost:5290/api


## Running Locally

### Backend

cd IdeaTracker.Api dotnet restore dotnet ef database update   
# applies migrations (if using EF Core migrations) dotnet run

The API will start at:
- HTTP: `http://localhost:5290`
- HTTPS: `https://localhost:7002`

Swagger UI is available at `/swagger` in the `Development` environment.

### Client
cd ..\IdeaTracker.Ui npm install npm run dev

The client will start at `http://localhost:5173` by default (Vite) and communicate with the backend using the configured `VITE_API_BASE_URL`.

## Notes
- Ensure `AllowedOrigins` on the backend matches the client's running URL, or CORS requests will be blocked.
- Sample data is seeded automatically on first run when `SeedOnStartup` is `true` and the database is empty.