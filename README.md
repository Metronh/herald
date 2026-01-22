# Article Management Microservice

A modern .NET 9/8 microservice for managing articles with distributed caching and JWT authentication.

## Features 
- Article CRUD operations
- Title-based search
- Distributed caching with Redis
- JWT-based authorization (With roles)
- Health check endpoints
- Containerized deployment

## Tech Stack 🛠️
- .NET 9/8 Minimal APIs
- MongoDB
- PostgresDB
- Redis (HybridCache for L2 caching)
- JWT Authentication with roles
- Docker & Docker Compose
- Swagger/OpenAPI
- GitHub Action

## Folder Structure
### Backend
```
🗂️ ArticleService
 ┣ 📂ArticleService
 ┃ ┣ 📂AppSettings
 ┃ ┣ 📂Database
 ┃ ┣ 📂Endpoints
 ┃ ┣ 📂Entities
 ┃ ┣ 📂Extensions
 ┃ ┣ 📂Interfaces
 ┃ ┣ 📂Middleware
 ┃ ┣ 📂Models
 ┃ ┣ 📂Repository
 ┃ ┣ 📂Services
 ┃ ┣ 📜Program.cs
 ┣ 📂ArticleServiceTests
 ┃ ┣ 📂ServicesTest
```
🗂️ UserService
```
📦UserService
 ┣ 📂UserService
 ┃ ┣ 📂AppSettings
 ┃ ┣ 📂Database
 ┃ ┣ 📂Endpoints
 ┃ ┣ 📂Entities
 ┃ ┣ 📂Extensions
 ┃ ┣ 📂Interfaces
 ┃ ┣ 📂Middleware
 ┃ ┣ 📂Models
 ┃ ┣ 📂Repository
 ┃ ┣ 📂Services
 ┃ ┣ 📂Validation
 ┃ ┣ 📜Program.cs
 ┣ 📂UserServiceTests
 ┃ ┣ 📂ServicesTests
 ┃ ┣ 📂ValidationTests
```

## Docker
```
📦docker
 ┣ 📂MongoDb
 ┗ 📂SQL
```

## Setup
```
📦SetUp
 ┣ 📂AppSettings
 ┣ 📂Data
 ┣ 📂Database
 ┣ 📂Endpoints
 ┣ 📂Extensions
 ┣ 📂Helpers
 ┣ 📂Interfaces
 ┣ 📂Middleware
 ┣ 📂Models
 ┣ 📂Repository
 ┣ 📂Services
 ┣ 📂SyntheticData
 ┣ 📜Dockerfile
 ┣ 📜Program.cs
```

## Running Locally 🚀
1. Go to terminal open project and type ```docker compose up -d --build``` (Make sure .env file is there and appsettings.development too as well as compose.override.yaml for open ports)
2. Run setup project on IDE
3. Hit ```/SetUpProject``` endpoint to seed with dummy data.
4. Run both UserService and ArticleService
5. You're up and running 🏃‍♀️

## Running with docker-compose
1. Go to terminal open project and type ```docker compose up -d --build``` (Make sure .env file is there and appsettings.development too)
2. The ports for each service
    - SetUp: http://localhost:8082/swagger/index.html
    - UserService: http://localhost:8080/swagger/index.html
    - ArticleService: http://localhost:8081/swagger/index.html

## Shutting Down ✋🏿
1. Go to terminal open project and type ```docker compose down -v --rmi all```
2. Shut down all instances of running in your IDE.