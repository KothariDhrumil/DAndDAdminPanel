# Testing Infrastructure

This directory contains the comprehensive automated testing setup for the DealersAndDistributors .NET backend application following clean architecture principles.

## Project Structure

```
tests/
├── Application.Tests      # Unit tests for Application layer
├── Infrastructure.Tests   # Integration tests for Infrastructure layer
└── WebApi.Tests          # End-to-end API tests for WebApi layer
```

## Test Projects

### Application.Tests

Tests for the Application layer including:
- CQRS handlers (Commands & Queries)
- Validators (FluentValidation)
- Application services
- Domain logic

**Technology Stack:**
- xUnit 2.9.2 - Test framework
- FluentAssertions 8.8.0 - Fluent assertion library
- Moq 4.20.72 - Mocking framework
- NSubstitute 5.3.0 - Alternative mocking framework

### Infrastructure.Tests

Integration tests for the Infrastructure layer including:
- Repository implementations
- Database operations
- External service integrations
- Data persistence

**Technology Stack:**
- xUnit 2.9.2 - Test framework
- FluentAssertions 8.8.0 - Fluent assertion library
- Moq 4.20.72 - Mocking framework
- NSubstitute 5.3.0 - Alternative mocking framework
- Testcontainers 4.8.1 - Container-based testing
- Testcontainers.MsSql 4.8.1 - SQL Server test containers

### WebApi.Tests

End-to-end API tests for the WebApi layer including:
- HTTP endpoint testing
- Controller testing
- Authentication/Authorization
- Integration testing

**Technology Stack:**
- xUnit 2.9.2 - Test framework
- FluentAssertions 8.8.0 - Fluent assertion library
- Moq 4.20.72 - Mocking framework
- Microsoft.AspNetCore.Mvc.Testing 9.0.10 - WebApplicationFactory for API testing

## Running Tests

### Run All Tests
```bash
dotnet test
```

### Run Tests for a Specific Project
```bash
dotnet test tests/Application.Tests/Application.Tests.csproj
dotnet test tests/Infrastructure.Tests/Infrastructure.Tests.csproj
dotnet test tests/WebApi.Tests/WebApi.Tests.csproj
```

### Run Tests with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Run Tests in Watch Mode
```bash
dotnet watch test --project tests/Application.Tests/Application.Tests.csproj
```

## CI/CD Integration

All test projects are compatible with GitHub Actions and other CI/CD platforms. They target .NET 9.0 and include:
- `coverlet.collector` for code coverage reporting
- `xunit.runner.visualstudio` for Visual Studio and CI integration

## Test Naming Conventions

Follow these naming conventions for tests:

```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    
    // Act
    
    // Assert
}
```

## Best Practices

1. **Isolation**: Each test should be independent and not rely on other tests
2. **AAA Pattern**: Use Arrange-Act-Assert pattern for clarity
3. **FluentAssertions**: Use fluent assertions for readable test assertions
4. **Testcontainers**: Use for real database integration tests (requires Docker)
5. **WebApplicationFactory**: Use for end-to-end API testing
6. **Mocking**: Use Moq or NSubstitute for mocking dependencies

## Docker Requirements

For Infrastructure.Tests using Testcontainers:
- Docker must be installed and running
- Tests will automatically start and stop SQL Server containers
- Container images are pulled automatically on first run

## Example Tests

### Application Layer Test Example
```csharp
[Fact]
public async Task CreatePlan_WithValidData_ShouldCreateSuccessfully()
{
    // Arrange
    var mockRepository = new Mock<IPlanRepository>();
    var handler = new CreatePlanCommandHandler(mockRepository.Object);
    var command = new CreatePlanCommand { Name = "Test Plan" };
    
    // Act
    var result = await handler.Handle(command, CancellationToken.None);
    
    // Assert
    result.Should().NotBeNull();
    result.IsSuccess.Should().BeTrue();
}
```

### Infrastructure Layer Test Example
```csharp
[Fact]
public async Task Repository_Insert_ShouldPersistToDatabase()
{
    // Arrange
    await using var msSqlContainer = new MsSqlBuilder().Build();
    await msSqlContainer.StartAsync();
    var connectionString = msSqlContainer.GetConnectionString();
    
    // ... setup DbContext with connection string
    
    // Act
    // ... perform database operations
    
    // Assert
    // ... verify data was persisted
    
    await msSqlContainer.StopAsync();
}
```

### WebApi Layer Test Example
```csharp
[Fact]
public async Task GetPlans_ShouldReturnOkResult()
{
    // Arrange
    var factory = new WebApplicationFactory<Program>();
    var client = factory.CreateClient();
    
    // Act
    var response = await client.GetAsync("/api/plans");
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

## Contributing

When adding new tests:
1. Follow the existing project structure
2. Add tests to the appropriate test project
3. Use the established naming conventions
4. Ensure all tests pass before committing
5. Aim for high code coverage of business logic

## Resources

- [xUnit Documentation](https://xunit.net/)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Moq Documentation](https://github.com/moq/moq4)
- [Testcontainers Documentation](https://dotnet.testcontainers.org/)
- [WebApplicationFactory Documentation](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
