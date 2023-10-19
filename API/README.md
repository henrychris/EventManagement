# API

The API project serves as the entry point for the entire application. It's responsible for handling HTTP requests,
managing authentication and authorization, and orchestrating interactions between different modules.

The project is configured to use JWT for authentication and authorization. Users are required to
authenticate using a valid JWT token to access secured endpoints.

## Configuration

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

## Getting Started

To get started with the API project, follow these steps:

1. Clone the repository or obtain the application source code.
2. Open a terminal or command prompt and navigate to the API project's directory.

3. Build the project using the dotnet build command.
4. Run the project using the dotnet run command. This will start the API and make it accessible via the configured URL.

If you're in a development environment, you can access the Swagger documentation by navigating to the Swagger UI at
/swagger/index.html in your browser.

You can use your favorite API client (e.g., Postman) to test the endpoints, but make sure to include a valid JWT
for secured endpoints. The JWT can be generated using the `Login/Register` endpoints in `UserModule.cs`.