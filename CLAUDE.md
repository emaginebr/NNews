# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

NNews is a full-stack CMS (Content Management System) for news and blogs with AI-powered content generation (ChatGPT and DALL-E 3). It consists of a .NET 8 backend API, PostgreSQL database, and React frontend, all orchestrated via Docker Compose.

## Build & Run Commands

### Backend (.NET 8)
```bash
# Restore and build the entire solution
dotnet restore NNews.sln
dotnet build NNews.sln

# Run the API (HTTP: 5007, HTTPS: 5008)
dotnet run --project NNews.API
```

### Frontend
```bash
# nnews-react (component library)
cd nnews-react && npm install && npm run build

# nnews-app (main application)
cd nnews-app && npm install && npm run dev
```

### Docker (full stack)
```bash
docker-compose up --build          # Start all services (postgres, api, app)
docker-compose up --build nnews-api  # Rebuild and start only the API
```

### Testing
```bash
# Frontend tests (nnews-react)
cd nnews-react && npm test
cd nnews-react && npx vitest run --coverage

# Backend has no unit test project; use NNews.API.http for manual endpoint testing
```

### NuGet Package Publishing
```bash
# Pack DTO and ACL libraries
dotnet pack NNews.DTO/NNews.DTO.csproj -c Release
dotnet pack NNews.ACL/NNews.ACL.csproj -c Release
```

## Architecture

The solution follows **Clean Architecture** with these layers:

```
NNews.API              → ASP.NET Core Web API (Controllers, Auth, Swagger)
NNews.Application      → DI registration & initialization (Initializer.cs)
NNews.Domain           → Business logic, entities, service interfaces & implementations
NNews.Infra            → EF Core DbContext, repositories, AutoMapper profiles
NNews.Infra.Interfaces → Repository interface contracts
NNews.DTO              → Data transfer objects, settings, AI request/response models (published as NuGet)
NNews.ACL              → Anti-Corruption Layer HTTP clients for consuming the API (published as NuGet)
```

**Frontend:**
- `nnews-react/` — Reusable React component library (published to NPM as `nnews-react`)
- `nnews-app/` — Main SPA consuming `nnews-react` and `nauth-react`

### Key Patterns
- **Repository Pattern**: Generic interfaces in `NNews.Infra.Interfaces`, implementations in `NNews.Infra`
- **AutoMapper Profiles**: Entity-to-DTO mapping in `NNews.Infra` (ArticleProfile, TagProfile, CategoryProfile)
- **DI Setup**: All service/repository registration happens in `NNews.Application/Initializer.cs`
- **Authentication**: NAuth.ACL with BasicAuthentication scheme and JWT; configured in `Program.cs`
- **CORS**: "AllowFrontend" policy (permissive — allows any origin)

### Domain Entities
- **Article** (with status: Draft, Published, Archived, Scheduled)
- **Category** (hierarchical via parent_id)
- **Tag** (many-to-many with articles via article_tags junction)
- **ArticleRole** (role-based access control per article)

### External Dependencies
- **NAuth.ACL** (v0.2.3) — Authentication & user management
- **NTools.ACL** (v0.2.2) — ChatGPT integration, file management, string utilities
- **PostgreSQL 16** — Primary database via Npgsql + EF Core

## Configuration

Environment-specific settings are in `appsettings.{Environment}.json`. Key sections:
- `ConnectionStrings:DefaultConnection` — PostgreSQL connection
- `NAuth` — Auth service URL and JWT secret
- `NTools` — Tools service URL (ChatGPT, file ops)
- `Serilog` — Structured logging config

Docker environment variables are defined in `.env` at the repo root.

## Documentation

All generated documentation must:
- Be in **Markdown** format (`.md`)
- Be saved in the `docs/` folder
- Use **UPPER_SNAKE_CASE** for file names (e.g., `docs/API_ENDPOINTS.md`, `docs/DEPLOYMENT_GUIDE.md`)

## Versioning

Uses GitVersion (ContinuousDelivery mode). Commit message prefixes control version bumps:
- `major:` or `breaking:` → Major
- `feature:` or `minor:` → Minor
- `fix:` or `patch:` → Patch

GitHub workflows handle NuGet publishing (`publish-nuget.yml`) and version tagging (`version-tag.yml`).
