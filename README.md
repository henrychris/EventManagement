# EventManagement
1. API
The API is the entry point of the application where it runs from. This part is responsible for handling HTTP requests, routing, and serving as the interface to the external world. It orchestrates the interaction between different modules and exposes endpoints for clients to communicate with the application.

Key features of the API part:

HTTP Endpoints: It defines and manages HTTP endpoints for various functionalities provided by the application.
Authentication and Authorization: Manages user authentication and authorization using JWT tokens.
Middleware: Utilizes middleware components to handle cross-cutting concerns such as exception handling.
2. EventModule
The EventModule is a class library responsible for handling event-related services. It encapsulates the logic for creating, updating, and managing events within the application. This module defines the data models, business logic, and data access operations related to events.

Key features of the EventModule:

Event Services: Provides services for creating, updating, and managing events.
Data Access: Defines data access and persistence operations for events.
Validation: Utilizes validators to ensure data integrity and enforce business rules related to events.
3. UserModule
The UserModule is a class library that focuses on user-related services. It manages user registration, authentication, and user-specific operations. This module defines data models, business logic, and data access operations related to users.

Key features of the UserModule:

User Services: Offers services for user registration, authentication, and user-specific operations.
Data Access: Defines data access and persistence operations for user-related data.
Validation: Utilizes validators to ensure user data integrity and enforce business rules related to users.
4. Shared
The Shared part is a class library that contains shared packages, entities, and code used by multiple modules. It serves as a central repository for common resources that can be reused across different parts of the application.

Key features of the Shared part:

Shared Entities: Contains common data models and entities used by both EventModule and UserModule.
Utilities: Provides utility functions and components that are shared among different parts of the application.
Constants: Holds shared constants, configurations, and settings.
Getting Started
To get started with the modular monolith application, follow these steps:

Clone the repository or obtain the application source code.

Set up your development environment, including required dependencies, databases, and .NET tools.

Build and run the application using the API project as the entry point.

Explore the different modules and their functionalities based on your requirements.

Contributing
We welcome contributions from the community to enhance the application. You can contribute by improving existing features, adding new functionality, fixing bugs, or providing documentation.
