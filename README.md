# EventManagement
A test application for managing events, built as a loosely coupled monolith. The API project is responsible for running all others.

## API
The API is the entry point for the application. This part is responsible for handling HTTP requests, routing, and serving as the interface to the external world. It exposes endpoints for clients to communicate with the application.

### Features

- HTTP Endpoints: It defines and manages HTTP endpoints for various functionalities provided by the application.
- Authentication and Authorization: Manages user authentication and authorization using JWT tokens.
- Middleware: Utilizes middleware components to handle cross-cutting concerns such as exception handling.

### Configuration

Before running the API project, make sure you have configured the required secrets using the dotnet secrets tool. You
need to set up the JwtSettings secret, which includes the following properties:

```
{
  "JwtSettings": {
    "Issuer": "https://id.xxx.com",
    "Audience": "https://xxx.com",
    "Key": "xxx",
    "TokenLifetimeInHours": 1
  }
}
```

- **Issuer**: The issuer of the JWT token.
- **Audience**: The intended audience for the JWT token.
- **Key**: Your secret key used to sign and verify JWT tokens.
- **TokenLifetimeInHours**: The lifetime of JWT tokens in hours.

## EventModule
The EventModule is a class library responsible for handling event-related services. This module defines the data models, business logic, and data access operations related to events.

## UserModule
The UserModule is a class library that focuses on user-related services. This module defines data models, business logic, and data access operations related to users.

## Shared
The Shared project is a class library that contains packages, entities, and code used by other modules. It serves as a central repository for common resources that can be reused across different parts of the application.

### Features

- Shared Entities: Contains common data models and entities used by both all other projects.
- Utilities: Provides utility functions and components that are shared among different parts of the application.
- Constants: Holds shared constants, configurations, and settings.

## Getting Started
To get started follow these steps:

1. Clone the repository or obtain the application source code.
2. Ensure Docker is installed on your system and running.
3. Update `docker.compose.yml` to use the guid matching your secrets.json file.
4. Navigate to the API directory, and run `docker compose up`. This will pull required images, build the project image and run the containers.
5. The API container tends to run ahead of the database, so restart it so it seeds properly.
6. Use the postman collection [here](https://documenter.getpostman.com/view/22039666/2s9YXk2LAj) to test the API.

## Notes
1. To debug, change the server name in the database connection string to `localhost`. Ensure this container, `mcr.microsoft.com/mssql/server:2022-latest` is running.
2. The project uses dotnet secrets for ease of development. Of course, a production app would use a key vault.
