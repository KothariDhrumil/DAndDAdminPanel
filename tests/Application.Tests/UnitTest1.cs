using FluentAssertions;
using Moq;

namespace Application.Tests;

/// <summary>
/// Sample test class demonstrating xUnit, FluentAssertions, and Moq setup.
/// Replace with actual application tests targeting specific handlers, validators, or services.
/// </summary>
public class SampleApplicationTests
{
    [Fact]
    public void Sample_Test_Should_Pass()
    {
        // Arrange
        var expected = "Hello World";
        
        // Act
        var actual = expected;
        
        // Assert
        actual.Should().Be("Hello World");
    }
    
    [Fact]
    public void Sample_Mock_Test_Should_Demonstrate_Moq()
    {
        // Arrange
        var mockService = new Mock<ITransientService>();
        
        // Assert
        mockService.Should().NotBeNull();
    }
}
