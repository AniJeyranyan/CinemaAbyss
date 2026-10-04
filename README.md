# CinemaAbyss — .NET 8 / Clean Architecture

A C# / ASP.NET Core 8 translation of the [architecture-cinemaabyss](../architecture-cinemaabyss)
Go-based microservices project. Each service is a standalone Clean Architecture
solution (Domain → Application → Infrastructure → Api), all collected into one
multi-project solution: `CinemaAbyss.sln`.

The original Go repository is untouched; this is a from-scratch .NET implementation
of the same target architecture (monolith + Strangler Fig migration + Kafka events).

## Solution layout

```
CinemaAbyss.sln
src/
  Shared/CinemaAbyss.SharedKernel/        # Entity<TId>, Result<T>, snake_case JSON policy
  Monolith/                               # users, movies, payments, subscriptions
    CinemaAbyss.Monolith.Domain
    CinemaAbyss.Monolith.Application      # MediatR commands/queries, repository interfaces
    CinemaAbyss.Monolith.Infrastructure   # EF Core + Npgsql, migrations, repositories
    CinemaAbyss.Monolith.Api              # Controllers, Program.cs, Dockerfile
  Movies/                                 # movies microservice (Strangler Fig target)
  Proxy/                                  # API Gateway implementing Strangler Fig routing
  Events/                                 # Kafka producer + consumer for domain events
tests/
  CinemaAbyss.{Monolith,Movies,Proxy,Events}.UnitTests
docker-compose.yml
```

Dependencies only point inward (`Api → Infrastructure/Application → Domain → SharedKernel`),
and `Domain`/`Application` never reference ASP.NET Core, EF Core, or Confluent.Kafka types —
those live only in each service's `Infrastructure`/`Api` layer.

## Services

| Service | Port | Responsibility |
|---|---|---|
| `monolith` | 8080 | Users, Movies, Payments, Subscriptions (owns the DB schema/migrations) |
| `movies-service` | 8081 | Movies only — extracted microservice, shares the DB during migration |
| `events-service` | 8082 | Publishes and consumes `movie-events` / `user-events` / `payment-events` on Kafka |
| `proxy-service` | 8000 | API Gateway — Strangler Fig routing in front of all of the above |

### Strangler Fig routing (`proxy-service`)

`CinemaAbyss.Proxy.Domain.Routing.RouteResolver` is a pure function (no I/O) that decides,
per request path, which backend to hit:

- `/health` → answered locally by the gateway (`Strangler Fig Proxy is healthy`)
- `/api/movies/health` → always `movies-service`
- `/api/movies*` → **gradual migration**:
  - `Proxy__GradualMigration=false` → 100% to `movies-service` (fully cut over)
  - `Proxy__GradualMigration=true` → `Proxy__MoviesMigrationPercent`% of requests to
    `movies-service`, the rest to `monolith`
- `/api/events*` → `events-service`
- everything else (`/api/users`, `/api/payments`, `/api/subscriptions`, …) → `monolith`

Change `Proxy__MoviesMigrationPercent` in `docker-compose.yml` and restart `proxy-service`
to see traffic shift between the monolith and the microservice.

### Events (`events-service`)

`POST /api/events/movie|user|payment` publishes a JSON envelope to the matching Kafka topic
and returns the resulting partition/offset. A background `KafkaConsumerBackgroundService`
in the same process subscribes to all three topics and logs every message it consumes —
satisfying the "the service both produces and consumes" requirement.

## Running locally

```bash
docker-compose up -d --build
```

- Monolith: http://localhost:8080
- Movies microservice: http://localhost:8081
- Events microservice: http://localhost:8082
- API Gateway (Proxy): http://localhost:8000
- Kafka UI: http://localhost:8090

```bash
curl http://localhost:8000/api/movies
curl http://localhost:8000/health
curl -X POST http://localhost:8000/api/events/movie \
  -H "Content-Type: application/json" \
  -d '{"movie_id":1,"title":"Inception","action":"viewed"}'
```

Stop everything:

```bash
docker-compose down -v
```

## Running without Docker

Requires the .NET 8 SDK and a reachable Postgres/Kafka (e.g. `docker-compose up postgres kafka zookeeper -d`).

```bash
dotnet run --project src/Monolith/CinemaAbyss.Monolith.Api
dotnet run --project src/Movies/CinemaAbyss.Movies.Api
dotnet run --project src/Events/CinemaAbyss.Events.Api
dotnet run --project src/Proxy/CinemaAbyss.Proxy.Api
```

Each service reads its connection info from `appsettings.json`, overridable via environment
variables using the standard ASP.NET Core double-underscore convention
(e.g. `ConnectionStrings__Default`, `Proxy__MoviesMigrationPercent`, `Kafka__BootstrapServers`).

## Tests

```bash
dotnet test
```

20 unit tests cover the Application-layer command/query handlers of all four services,
with the Proxy's `RouteResolver` (the Strangler Fig decision logic) and
`ForwardHttpRequestCommandHandler` tested in isolation from HTTP/ASP.NET Core.

## Database schema

The `monolith` service is the schema owner: it runs its EF Core migrations
(`CinemaAbyss.Monolith.Infrastructure/Persistence/Migrations`) and seeds sample data on
startup. `movies-service` only queries the already-created `movies` table — it does not
run migrations itself, mirroring how a microservice extracted via Strangler Fig shares its
source-of-truth database with the monolith until it's fully cut over.
