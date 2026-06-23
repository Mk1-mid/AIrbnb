# RentalPlatform

A unified short-term rental platform built with ASP.NET Core (.NET 9), Razor Pages, PostgreSQL, and Docker. Designed to eliminate booking friction, automate identity validation via AI (KYC), and empower property owners with real-time financial analytics.

## Prerequisites

- [Docker](https://www.docker.com/) and Docker Compose installed
- No additional tools required — everything runs inside containers

## Getting Started

Clone the repository and run:

```bash
git clone https://github.com/Mk1-mid/AIrbnb.git
cd rental-platform
docker compose up --build
```

The application will be available at:

| Service        | URL                        |
|----------------|----------------------------|
| Web App        | http://localhost:8080       |
| PostgreSQL     | localhost:5432              |
| Mailer Service | http://localhost:8081       |

> On first boot, the database is automatically migrated and seeded with sample data (3 owners, 5 guests, 10 properties).

## Project Structure

```
├── docker-compose.yml
├── README.md
│
└── src/
    ├── RentalPlatform.Domain/          # Entities, Value Objects, Enums, Domain Services
    │   ├── Entities/
    │   ├── Enums/
    │   ├── Services/
    │   └── ValueObjects/
    │
    ├── RentalPlatform.Application/     # Use Cases, DTOs, Interfaces, Validators
    │
    ├── RentalPlatform.Infrastructure/  # EF Core, Repositories, KYC, Reports, Identity
    │
    ├── RentalPlatform.Web/             # Razor Pages, Controllers, Program.cs
    │   ├── Pages/                      # Razor Pages UI
    │   │   └── Front/                  # Converted Stitch front pages
    │   │       └── Shared/             # Shared front layout + navigation
    │   ├── wwwroot/                    # Static assets (shared CSS, JS, images)
    │   └── Program.cs
    │
    └── RentalPlatform.Tests/           # Unit and integration tests
└── services/
    └── mailer/                         # Laravel microservice — email dispatch only
```

## Frontend Organization

The Stitch export lives in `front/` as the original design source. The runnable UI is now under `src/RentalPlatform.Web/Pages/Front/`, with the main landing page mapped to `src/RentalPlatform.Web/Pages/Index.cshtml`.

`/Index` now redirects to `/Front/Index` so the stitched home remains the canonical landing screen while the root route stays valid.

If you later extract shared styles or scripts from the Stitch pages, place them in `src/RentalPlatform.Web/wwwroot/` and reference them from the Razor pages.

## Architecture

### Clean Architecture

The system is built around Clean Architecture principles. The domain layer has zero external dependencies — no EF Core, no HTTP, no framework. Business rules are enforced at the object level, not at the controller or database level.

```
Domain → Application → Infrastructure
        ↑
        Web
```

This means the core business logic can be unit tested without spinning up a database or a web server.

### Monorepo

A multi-repo approach was considered and discarded. The Laravel mailer is a lightweight complement, not an independent system. A single `docker-compose.yml` orchestrates the entire environment, keeping setup to one command.

## Key Technical Decisions

### Value Objects

Three Value Objects encode critical business rules directly into the domain:

- **`DateRange`** — automatically enforces check-in at 2:00 PM and check-out at 12:00 PM. It is structurally impossible to create a reservation with incorrect times because the domain rejects it at construction.
- **`Money`** — guards against negative values and prepares the system for multi-currency support without entity changes.
- **`Email`** — validates format and normalizes to lowercase at construction time.

### Double Booking Prevention

Conflict detection lives in `ReservationDomainService`, not in a controller or repository. The overlap check uses standard interval logic:

```
CheckIn < other.CheckOut && CheckOut > other.CheckIn
```

The entire reservation flow executes inside a serializable transaction to eliminate race conditions on concurrent booking attempts.

### KYC — Identity Validation Without Data Retention

Identity documents are processed using **Tesseract OCR** (fully dockerized, no external API keys required). The image is deleted immediately after extraction. Only structured data is persisted: first name, last name, document number, and date of birth. This follows the data minimization principle and protects user privacy by design.

### Omnichannel Notifications

In-app notifications are managed by the `Notification` entity inside the .NET domain and served through Razor Pages. Email dispatch is delegated to a minimal Laravel microservice — a single HTTP endpoint that receives a payload and sends the email. This keeps the .NET core focused on business logic while honoring the enunciado's suggestion for Laravel as a lightweight complement.

### PostgreSQL — Fully Local

Supabase was considered and rejected to keep the environment self-contained. No external credentials are required. EF Core Code First manages the schema through versioned migrations.

### Excel Reports with ClosedXML

ClosedXML was chosen for its MIT license and clean fluent API. Reports are filterable by property and date range, and include: rental dates, amount paid, guest information, and associated property.

## Roles

| Role   | Capabilities                                                                 |
|--------|------------------------------------------------------------------------------|
| Guest  | Browse properties, manage wishlist, make reservations, complete KYC           |
| Owner  | Publish and manage properties, view performance dashboard, export Excel reports |

> Anonymous users can browse the catalog and filter by city and dates. Authentication is only required at the moment of reserving or saving a favorite.

## Environment Variables

Configured via `docker-compose.yml`. No `.env` file required for local setup.

| Variable                | Description                              |
|-------------------------|------------------------------------------|
| `POSTGRES_DB`           | Database name                            |
| `POSTGRES_USER`         | Database user                            |
| `POSTGRES_PASSWORD`     | Database password                        |
| `ConnectionStrings__Default` | Full connection string for EF Core   |
| `Mailer__BaseUrl`       | Internal URL of the Laravel mailer service |

## Running Tests

```bash
docker compose run web dotnet test
```

## Built With

| Technology  | Role                                      |
|-------------|-------------------------------------------|
| ASP.NET Core (.NET 9) | Core backend + Razor Pages frontend    |
| Entity Framework Core | ORM + Code First migrations            |
| PostgreSQL 16 | Primary database                        |
| Tesseract OCR | KYC document processing                  |
| ClosedXML    | Excel report generation                   |
| Laravel      | Email microservice                        |
| Tailwind CSS | UI styling                                |
| Docker Compose | Full environment orchestration          |