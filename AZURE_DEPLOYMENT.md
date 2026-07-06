# LockSight Backend Deployment

This backend follows the practical deployment flow from the learning-center-platform guide: build the ASP.NET API, configure the database connection through environment variables, run EF Core migrations, and deploy a production artifact.

## Local Docker

1. Copy `.env.example` to `.env`.
2. Set `DATABASE_PASSWORD` and `TOKEN_SECRET` in `.env`.
3. Run:

```powershell
docker compose up --build
```

The API is exposed on `http://localhost:8080` and Swagger is available at `/swagger`.

## Required Environment Variables

- `ASPNETCORE_ENVIRONMENT`: use `Production` in Azure.
- `DATABASE_URL`: MySQL host name.
- `DATABASE_SCHEMA`: MySQL database name.
- `DATABASE_USER`: MySQL user.
- `DATABASE_PASSWORD`: MySQL password.
- `TOKEN_SECRET`: long secret used for JWT signing.

## Azure Option: App Service for Containers or Azure Container Apps

Use this option because the project targets `.NET 10` and has a Dockerfile.

1. Create an Azure Database for MySQL instance.
2. Create the database schema named in `DATABASE_SCHEMA`.
3. Build and push the Docker image to Azure Container Registry.
4. Create an Azure App Service for Containers or Azure Container App using the pushed image.
5. Configure the environment variables listed above in Azure.
6. Make sure the app can connect to MySQL through firewall/VNet settings.

## Database Migrations

Before first production use, apply migrations with the production connection string:

```powershell
dotnet ef database update --project .\Watchgate.Locksight.Platform\Watchgate.Locksight.Platform.csproj --startup-project .\Watchgate.Locksight.Platform\Watchgate.Locksight.Platform.csproj
```

The app also calls `context.Database.Migrate()` at startup, so in container deployments it can apply pending migrations automatically when the configured MySQL user has permissions.

## GitHub Notes

Do not commit `.env`. Commit `.env.example`, `Dockerfile`, `docker-compose.yml`, migrations, and source code.
