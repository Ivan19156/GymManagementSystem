# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build the solution
dotnet build

# Run the web application
dotnet run --project GymManagement.WebMVC

# Apply EF Core migrations to the database
dotnet ef database update --project GymManagement.Infrastructure --startup-project GymManagement.WebMVC

# Add a new EF Core migration
dotnet ef migrations add <MigrationName> --project GymManagement.Infrastructure --startup-project GymManagement.WebMVC
```

**Requirements**: .NET SDK 10.0.103 (see `global.json`), SQL Server Express at `.\SQLEXPRESS`.

## Architecture

Three-project layered architecture with dependencies flowing: WebMVC → Infrastructure → Domain.

- **GymManagement.Domain** — Entity models only. All entities inherit from `Entity` (int Id, auto-generated). Aggregate roots also implement `IAggregateRoot` (marker interface).
- **GymManagement.Infrastructure** — EF Core `GymContext`, Fluent API entity configurations (one class per entity), and migrations. Applies snake_case column naming convention globally in `OnModelCreating`.
- **GymManagement.WebMVC** — ASP.NET Core MVC. Controllers inject `GymContext` directly and use async EF Core methods. Views use Razor with Bootstrap 5.

## Domain Model

**User hierarchy** (all entities, not inheritance — composition via FK):
- `User` — base: Email, FirstName, LastName, Phone, IsActive
- `Client` (UserId FK) — MedicalNotes; has `UserMemberships` and `ScheduledSessions`
- `Trainer` (UserId FK) — SpecializationId, ExperienceYears, HourlyRate, IsAvailable
- `Admin` (UserId FK) — AccessLevel

**Membership**: `MembershipType` → `MembershipPlan` (DurationDays, Price) → `UserMembership` (ClientId, StartDate, EndDate, StatusId, SessionsUsed)

**Sessions**: `TrainingCategory` → `TrainingType` → `ScheduledSession` (TrainerId, ClientId, SessionDate, StartTime, EndTime, StatusId)

**Lookup tables**: `MembershipStatus`, `SessionStatus`, `Specialization`

## Key Conventions

- Entity configurations live in `GymManagement.Infrastructure/EntityConfigurations/` — one `IEntityTypeConfiguration<T>` class per entity, auto-discovered via `ApplyConfigurationsFromAssembly`.
- Database uses snake_case column names (converted from PascalCase in `GymContext.OnModelCreating`).
- Connection string key: `"DefaultConnection"` in `appsettings.json` targeting `GymManagementSystem` on `.\SQLEXPRESS`.
- No authentication/authorization is currently implemented.
- UI navigation labels are in Ukrainian.
