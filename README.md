# Student Management API

A RESTful Student Management API built using **C# and ASP.NET Core Minimal APIs (.NET 10)**. This project demonstrates backend development concepts including CRUD operations, database integration, dependency injection, middleware, JWT authentication, and role-based authorization.

## Technologies Used

- **Language:** C#
- **Framework:** ASP.NET Core Minimal APIs (.NET 10)
- **ORM:** Entity Framework Core
- **Database:** SQLite
- **Authentication:** ASP.NET Core Identity
- **Authorization:** JWT Bearer Authentication and Role-Based Authorization
- **API Documentation:** Swagger / OpenAPI
- **Version Control:** Git and GitHub

## Features

### Student Management
- Create, retrieve, update, and delete student records.
- Search students by name.
- Filter students by age.
- Retrieve student records using pagination.

### Authentication & Authorization
- User registration and login.
- Password management through ASP.NET Core Identity.
- JWT-based authentication.
- Role-based authorization with three roles:
  - **Student:** Can view student records.
  - **Teacher:** Can view, create, and update student records.
  - **Admin:** Has full access, including deleting student records.

### Backend Architecture
- Entity Framework Core for database operations.
- SQLite for persistent data storage.
- Dependency Injection for service registration.
- Separate service and endpoint layers.
- Request validation.
- Custom exception-handling middleware.
- Application logging.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Git
- Visual Studio Code or another C# development environment

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/thekrishal/StudentApi.git
   ```

2. Navigate into the project directory:

   ```bash
   cd StudentApi
   ```

3. Restore dependencies:

   ```bash
   dotnet restore
   ```

4. Apply database migrations:

   ```bash
   dotnet ef database update
   ```

5. Run the application:

   ```bash
   dotnet run
   ```

6. Open the Swagger URL displayed in the terminal to explore and test the API.

## Authentication

The API uses JWT Bearer Authentication.

1. Register a user.
2. Log in to receive a JWT.
3. Copy the returned token.
4. Click the **Authorize** button in Swagger.
5. Enter the token using the Bearer authentication scheme.
6. Access resources according to the user's assigned role.

Newly registered users are automatically assigned the **Student** role.

## Learning Objectives

This project was developed as a hands-on learning experience to understand:

- Building RESTful APIs with ASP.NET Core Minimal APIs.
- Working with Entity Framework Core and relational databases.
- Implementing dependency injection and service-based architecture.
- Handling validation, exceptions, and application logging.
- Understanding authentication and authorization.
- Implementing JWT authentication and role-based access control.
- Managing source code with Git and GitHub.

## Future Improvements

- Automated unit and integration testing.
- Improved configuration and secret management.
- Deployment to a cloud hosting platform.
- Additional security and performance improvements.

## Author
Krishal Adhikari

GitHub: [@thekrishal](https://github.com/thekrishal)

---

*Built as a hands-on project to develop practical backend development skills using C# and ASP.NET Core.*