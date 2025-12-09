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
 ┣ 📂Tests
 ┃ ┣ 📂ServicesTests
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
## Resources
```
📦Resources
 ┣ 📜articles_1000_with_text.csv
 ┣ 📜generateNames.py
 ┗ 📜users.csv
```

## Setup
```
📦Setup
 ┗ 📂SetUp
 ┃ ┣ 📂SetUp
 ┃ ┃ ┣ 📂AppSettings
 ┃ ┃ ┣ 📂Database
 ┃ ┃ ┣ 📂Endpoints
 ┃ ┃ ┣ 📂Extensions
 ┃ ┃ ┣ 📂Helpers
 ┃ ┃ ┣ 📂Interfaces
 ┃ ┃ ┣ 📂Middleware
 ┃ ┃ ┣ 📂Models
 ┃ ┃ ┣ 📂Properties
 ┃ ┃ ┣ 📂Repository
 ┃ ┃ ┣ 📂Services
 ┃ ┃ ┣ 📜Program.cs
```

## Running Locally 🚀
1. Go to terminal open project and type ```docker compose up -d```
2. Run setup project
    - note: make sure in AppSettings all connection strings match and the location of articles_1000_with_text.csv and users.csv are there.
3. Hit ```/SetUpProject``` endpoint
4. Run both UserService and ArticleService
5. You're up and running 🏃‍♀️

## Shutting Down ✋🏿
1. Go to terminal open project and type ```docker compose down```
    - Note: If you want to delete the volumes make sure you use this ```docker compose down -v```
2. Shut down all instances of running in your IDE.