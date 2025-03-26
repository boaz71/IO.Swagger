
Math Calculator API

This is a simple ASP.NET Core 8 REST API that performs basic math operations between two numbers.
Supported operations: add, subtract, multiply, divide.

The API is protected using JWT authentication. A demo token is available through a public endpoint.

How to Run:
1. Make sure .NET 8 is installed.
2. Open the solution in Visual Studio or VS Code.
3. Run the project (F5 or Ctrl+F5).
4. Navigate to: http://localhost:50352/swagger/

JWT Authentication:
1. Send a GET request to: /api/auth/token
2. Copy the token from the response.
3. In Swagger, click the Authorize button.
4. Paste the token with the format: Bearer eyJhbGciOi...
5. You can now access protected endpoints.

Example Request:
POST /api/calculator/calculate
Headers:
  Content-Type: application/json
  Authorization: Bearer {your_token}
  X-Operation-Type: add

Body:
{
  "number1": 8,
  "number2": 4
}

Expected Response:
{
  "result": 12
}

Validation:
- Only numeric values (double) are accepted in the body.
- Operation must be one of: add, subtract, multiply, divide.
- Invalid inputs such as division by zero or unknown operations return a 400 error.

Tests:
Unit tests cover all operations and edge cases.
An integration test is included that simulates a full request using a real JWT.

To run tests:
dotnet test

Project Structure:
- Controllers/          - API endpoints
- Models/               - Swagger-generated models
- Enums/                - Operation type enum
- Startup.cs            - App configuration (JWT, Swagger, etc.)
- appsettings.json      - JWT secret stored here
- MathCalculator.Tests/ - Test project

YAML:
The API was defined using an OpenAPI 3.0 YAML file on SwaggerHub.
Code was generated using the ASP.NET Core Server Stub.

Docker Support:
docker build -t mathapi .
docker run -d -p 8080:8080 mathapi
to browese:  http://localhost:8080/swagger/


Notes:
- Token expiration is currently set to 30 minutes.
- The default port inside Docker is set to 8080

*** -- ***
A Dockerfile is included to allow building and running the API inside a container.
Due to local environment limitations (Windows 10 Home + WSL instability), the image build was not fully tested locally.
However, the configuration follows standard .NET 8 Docker practices and is expected to work on most environments.
