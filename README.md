# Herald

A .NET microservice system for managing articles and users, with REST APIs, distributed caching, JWT authentication, and async email communications via RabbitMQ and Azure Functions.

## Services

**ArticleService** — Article CRUD with title-based search and Redis caching (.NET 9)

**UserService** — User registration, login, JWT token issuance, and session management (.NET 8)

**CommunicationsFunctions** — Azure Functions app for sending transactional emails via [Resend](https://resend.com). Triggered via HTTP or RabbitMQ queue. (.NET 9)

**SetUp** — Seed service for populating MongoDB and PostgreSQL with synthetic data from CSV files. Not intended for production use.

## Tech Stack

- **.NET 8 / 9** — Minimal APIs & Azure Functions (isolated worker)
- **MongoDB** — Article document storage
- **PostgreSQL** — User and session storage (via Npgsql + Dapper)
- **Redis** — Distributed caching (HybridCache)
- **RabbitMQ + MassTransit** — Message bus for async email events
- **Resend** — Transactional email delivery
- **JWT** — Authentication & role-based authorization
- **FluentValidation** — Request validation
- **Docker & Docker Compose** — Containerization
- **Swagger / OpenAPI** — API documentation
- **GitHub Actions** — CI/CD

## Project Structure

```
📦 herald
 ┣ 📂 Backend
 ┃ ┣ 📂 ArticleService
 ┃ ┃ ┣ 📂 ArticleService          # Main service
 ┃ ┃ ┗ 📂 ArticleServiceTests
 ┃ ┃   ┗ 📂 ServicesTest
 ┃ ┣ 📂 CommunicationsFunctions   # Azure Functions email service
 ┃ ┃ ┗ 📂 CommunicationsFunctions
 ┃ ┃   ┣ 📂 Functions             # HTTP + Queue triggers
 ┃ ┃   ┣ 📂 EmailTemplates        # HTML email templates
 ┃ ┃   ┗ 📂 Messaging/Events
 ┃ ┗ 📂 UserService
 ┃   ┣ 📂 UserService             # Main service
 ┃   ┗ 📂 UserServiceTests
 ┃     ┗ 📂 ValidationTests
 ┣ 📂 Setup
 ┃ ┗ 📂 SetUp                     # Data seeding service
 ┃   ┗ 📂 SyntheticData           # articles.csv, users.csv
 ┗ 📂 docker
   ┣ 📂 MongoDb                   # Dockerfile + init script
   ┗ 📂 SQL                       # Dockerfile + schema SQL
```

## Prerequisites

- [Docker](https://www.docker.com/) and Docker Compose
- [.NET 9 SDK](https://dotnet.microsoft.com/download) (for running locally outside Docker)
- A configured `.env` file (see below)
- `appsettings.Development.json` in each service
- `compose.override.yaml` for port exposure when developing locally

## Environment Configuration

Create a `.env` file in the root directory. Required variables:

```env
# Environment
ENVIRONMENT=

# MongoDB
MONGODBUSERNAME=
MONGODBPASSWORD=
MongoDbConnectionString=

# PostgreSQL
POSTGRESUSER=
POSTGRESPASSWORD=
PostgresConnectionString=

# Redis
RedisConnectionString=

# JWT
JwtToken=
JwtIssuer=
JwtAudience=
JwtExpiryTime=

# Resend (CommunicationsFunctions)
ResendApiKey=
ResendFromAddress=

# RabbitMQ
RabbitMQConnectionString=
RabbitMQUsername=
RabbitMQPassword=
```

> Each service also reads from its own `appsettings.Development.json`. Refer to those files for the full list of expected keys.

## Running Locally (IDE + Docker)

Use this approach when developing — services run in your IDE against Dockerized infrastructure.

1. Start infrastructure:
   ```bash
   docker compose up -d --build
   ```
   > Requires `.env`, `appsettings.Development.json` per service, and `compose.override.yaml` for port bindings.

2. Run the **SetUp** project from your IDE.

3. Seed dummy data:
   ```
   POST /SetUpProject
   ```

4. Run **UserService**, **ArticleService**, and **CommunicationsFunctions** from your IDE.

## Running with Docker Compose (Full Stack)

1. Start all services:
   ```bash
   docker compose up -d --build
   ```

2. Services will be available at:

   | Service                 | URL                                      |
   |-------------------------|------------------------------------------|
   | SetUp                   | http://localhost:8082/swagger/index.html |
   | UserService             | http://localhost:8080/swagger/index.html |
   | ArticleService          | http://localhost:8081/swagger/index.html |
   | CommunicationsFunctions | http://localhost:7071                    |

## Shutting Down

```bash
docker compose down -v --rmi all
```

If running services in your IDE, stop those instances manually as well.