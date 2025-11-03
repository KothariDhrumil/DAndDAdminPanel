using FluentAssertions;
using Testcontainers.MsSql;

namespace Infrastructure.Tests;

/// <summary>
/// Sample test class demonstrating xUnit, FluentAssertions, and Testcontainers setup.
/// Replace with actual infrastructure tests targeting repositories, DbContext, or external services.
/// For real database integration tests, use Testcontainers to spin up test databases.
/// </summary>
public class SampleInfrastructureTests
{
    [Fact]
    public void Sample_Test_Should_Pass()
    {
        // Arrange
        var expected = 42;
        
        // Act
        var actual = expected;
        
        // Assert
        actual.Should().Be(42);
    }
    
    [Fact]
    public async Task Sample_Testcontainer_Test_Should_Create_Container_Configuration()
    {
        // This test demonstrates how to configure a SQL Server test container
        // In real tests, you would start the container, run migrations, and test database operations
        
        // Arrange
        var msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();
        
        // Assert
        msSqlContainer.Should().NotBeNull();
        
        // Note: Starting the container requires Docker to be running
        // await msSqlContainer.StartAsync();
        // var connectionString = msSqlContainer.GetConnectionString();
        // await msSqlContainer.StopAsync();
    }
}
