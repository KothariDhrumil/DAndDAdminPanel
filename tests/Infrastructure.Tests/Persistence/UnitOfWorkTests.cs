using Application.Common.Interfaces;
using Domain.Customers;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Infrastructure.Tests.Persistence;

/// <summary>
/// Unit tests for the UnitOfWork implementation
/// </summary>
public class UnitOfWorkTests
{
    [Fact]
    public void UnitOfWork_Should_Be_Created_With_Valid_Dependencies()
    {
        // Arrange
        var mockContext = new Mock<RetailDbContext>(
            new DbContextOptionsBuilder<RetailDbContext>().Options,
            null!, null!, null!, null!);
        var cache = new MemoryCache(new MemoryCacheOptions());

        // Act
        var unitOfWork = new UnitOfWork(mockContext.Object, cache);

        // Assert
        unitOfWork.Should().NotBeNull();
        cache.Dispose();
    }

    [Fact]
    public void UnitOfWork_Should_Provide_Products_Repository()
    {
        // Arrange
        var mockContext = new Mock<RetailDbContext>(
            new DbContextOptionsBuilder<RetailDbContext>().Options,
            null!, null!, null!, null!);
        var cache = new MemoryCache(new MemoryCacheOptions());
        var unitOfWork = new UnitOfWork(mockContext.Object, cache);

        // Act
        var productsRepo = unitOfWork.Products;

        // Assert
        productsRepo.Should().NotBeNull();
        productsRepo.Should().BeAssignableTo<IRepository<Product>>();
        
        cache.Dispose();
        unitOfWork.Dispose();
    }

    [Fact]
    public void UnitOfWork_Should_Provide_Routes_Repository()
    {
        // Arrange
        var mockContext = new Mock<RetailDbContext>(
            new DbContextOptionsBuilder<RetailDbContext>().Options,
            null!, null!, null!, null!);
        var cache = new MemoryCache(new MemoryCacheOptions());
        var unitOfWork = new UnitOfWork(mockContext.Object, cache);

        // Act
        var routesRepo = unitOfWork.Routes;

        // Assert
        routesRepo.Should().NotBeNull();
        routesRepo.Should().BeAssignableTo<IRepository<Route>>();
        
        cache.Dispose();
        unitOfWork.Dispose();
    }

    [Fact]
    public void UnitOfWork_Should_Provide_PurchaseUnits_Repository()
    {
        // Arrange
        var mockContext = new Mock<RetailDbContext>(
            new DbContextOptionsBuilder<RetailDbContext>().Options,
            null!, null!, null!, null!);
        var cache = new MemoryCache(new MemoryCacheOptions());
        var unitOfWork = new UnitOfWork(mockContext.Object, cache);

        // Act
        var purchaseUnitsRepo = unitOfWork.PurchaseUnits;

        // Assert
        purchaseUnitsRepo.Should().NotBeNull();
        
        cache.Dispose();
        unitOfWork.Dispose();
    }

    [Fact]
    public void UnitOfWork_Should_Return_Same_Repository_Instance_On_Multiple_Calls()
    {
        // Arrange
        var mockContext = new Mock<RetailDbContext>(
            new DbContextOptionsBuilder<RetailDbContext>().Options,
            null!, null!, null!, null!);
        var cache = new MemoryCache(new MemoryCacheOptions());
        var unitOfWork = new UnitOfWork(mockContext.Object, cache);

        // Act
        var repo1 = unitOfWork.Products;
        var repo2 = unitOfWork.Products;

        // Assert
        repo1.Should().BeSameAs(repo2);
        
        cache.Dispose();
        unitOfWork.Dispose();
    }
}
