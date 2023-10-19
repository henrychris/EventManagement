# EventManagement
A test application for managing events, built as a loosely coupled monolith. The API project is responsible for running all others.

## API
The API is the entry point for the application. This part is responsible for handling HTTP requests, routing, and serving as the interface to the external world. It exposes endpoints for clients to communicate with the application.

### Features

- HTTP Endpoints: It defines and manages HTTP endpoints for various functionalities provided by the application.
- Authentication and Authorization: Manages user authentication and authorization using JWT tokens.
- Middleware: Utilizes middleware components to handle cross-cutting concerns such as exception handling.

## EventModule
The EventModule is a class library responsible for handling event-related services. This module defines the data models, business logic, and data access operations related to events.

## UserModule
The UserModule is a class library that focuses on user-related services. This module defines data models, business logic, and data access operations related to users.

## Shared
The Shared part is a class library that contains shared packages, entities, and code used by multiple modules. It serves as a central repository for common resources that can be reused across different parts of the application.

### Features

- Shared Entities: Contains common data models and entities used by both all other projects.
- Utilities: Provides utility functions and components that are shared among different parts of the application.
- Constants: Holds shared constants, configurations, and settings.

## Getting Started
To get started with the modular monolith application, follow these steps:

1. Clone the repository or obtain the application source code.

2. Set up your development environment, including required dependencies, databases, and .NET tools.

3. Build and run the application using the API project as the entry point.

4. Explore the different modules and their functionalities based on your requirements.
