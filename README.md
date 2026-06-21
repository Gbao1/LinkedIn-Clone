# LinkedIn Clone

LinkedIn Clone is a full-stack social web application built with ASP.NET Core MVC. It includes user authentication, profile management, a post feed, and like/unlike interactions, with Entity Framework Core handling data access.

## Tech Stack

- Backend: ASP.NET Core 8 (MVC + Razor Pages)
- Language: C# (.NET 8)
- ORM: Entity Framework Core
- Auth: ASP.NET Core Identity
- Databases: SQLite (default) and SQL Server (supported)
- Frontend: Razor Views, HTML, CSS, JavaScript

## Features

- User registration and login with Identity
- Profile create/edit/details flow
- Feed post creation for authenticated users
- Like/unlike on posts with dynamic response
- Seed data for demo users, posts, and likes
- Automatic migration and startup seeding

## Project Structure

```
LinkedIn-Clone/
	LinkedInClone.sln
	Web/
		Controllers/
		Data/
		Models/
		Views/
		wwwroot/
		Program.cs
		appsettings.json
```

## Prerequisites

- .NET SDK 8.0+
- SQL Server (optional, only if you want SQL Server instead of SQLite)

## Getting Started

1. Clone the repository.
2. Open a terminal in the repository root.
3. Restore dependencies and build:

```bash
dotnet restore
dotnet build LinkedInClone.sln
```

4. Run the web app:

```bash
dotnet run --project Web/Web.csproj
```

5. Open the URL shown in terminal (typically https://localhost:xxxx).

## Database Configuration

By default, the app uses SQLite via:

```json
"ConnectionStrings": {
	"DefaultConnection": "DataSource=app.db;Cache=Shared"
}
```

To use SQL Server, update `Web/appsettings.json` with a SQL Server connection string, for example:

```json
"ConnectionStrings": {
	"DefaultConnection": "Server=YOUR_SERVER;Database=LinkedInCloneDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

The app detects SQL Server connection strings and switches provider automatically.

## Build and Test

Build:

```bash
dotnet build LinkedInClone.sln
```

Run:

```bash
dotnet run --project Web/Web.csproj
```

If you add tests later:

```bash
dotnet test
```

## Branches

- `main`: primary branch
- `bao_posting`: posting-focused development branch

## Contribution

1. Create a new branch from `main`.
2. Commit focused, descriptive changes.
3. Open a pull request with a clear summary and screenshots for UI updates.

## License

This project is for educational and portfolio purposes.