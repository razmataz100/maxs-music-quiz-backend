# Max's Music Quiz - Backend

Music quiz API built with ASP.NET Core.

> This is the backend API. You'll also need the [frontend](https://github.com/razmataz100/maxs-music-quiz-frontend) running to use the complete application.

## Setup

1. **Install prerequisites**
   - .NET 8 SDK
   - SQL Server
   - Spotify Developer Account

2. **Clone and configure**
   ```bash
   git clone https://github.com/razmataz100/maxs-music-quiz-backend
   cd MaxsMusicQuiz.Backend
   ```

3. **Create settings file**
   - Copy `appsettings.Example.json` to `appsettings.json`
   - Add your Spotify credentials and database connection

4. **Create database structure**
   ```bash
   dotnet ef database update
   ```
   This creates all the required tables in your database

5. **Run**
   ```bash
   dotnet run
   ```

## API Documentation

Visit `/swagger` when the app is running to see all endpoints and test them.

## Spotify Setup

1. Go to https://developer.spotify.com/dashboard
2. Create app
3. Copy Client ID and Secret to `appsettings.json`

## Common Issues

- **Database errors**: Run `dotnet ef database update`
- **Connection failed**: Check SQL Server is running
- **Spotify errors**: Check your credentials