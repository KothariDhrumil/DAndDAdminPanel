using Application.Common.Interfaces;
using Domain.Customers;
using FluentAssertions;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Tests.Persistence;

/// <summary>
/// Unit tests for the generic Repository implementation
/// </summary>
public class RepositoryTests : IDisposable
{
    private readonly DbContext _context;
    private readonly IMemoryCache _cache;
    private readonly IRepository<Product> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _cache = new MemoryCache(new MemoryCacheOptions());
        _repository = new Repository<Product>(_context, _cache, enableCaching: true);
    }

    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<DbContext> options) : base(options) { }
        
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);
        }
    }

    [Fact]
    public async Task AddAsync_Should_Add_Entity_To_Context()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            BasePrice = 100.50m,
            IGST = 18m,
            CGST = 9m,
            Order = 1,
            DataKey = "test-key"
        };

        // Act
        await _repository.AddAsync(product);
        await _context.SaveChangesAsync();

        // Assert
        var testContext = (TestDbContext)_context;
        var addedProduct = await testContext.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");
        addedProduct.Should().NotBeNull();
        addedProduct!.Name.Should().Be("Test Product");
        addedProduct.BasePrice.Should().Be(100.50m);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Entity_From_Database()
    {
        // Arrange
        var product = new Product
        {
            Name = "Product to Get",
            BasePrice = 200m,
            IGST = 18m,
            CGST = 9m,
            Order = 2,
            DataKey = "test-key"
        };
        var testContext = (TestDbContext)_context;
        testContext.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        var retrievedProduct = await _repository.GetByIdAsync(product.Id);

        // Assert
        retrievedProduct.Should().NotBeNull();
        retrievedProduct!.Name.Should().Be("Product to Get");
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Entities()
    {
        // Arrange
        var products = new[]
        {
            new Product { Name = "Product 1", BasePrice = 100m, IGST = 18m, CGST = 9m, Order = 1, DataKey = "test-key" },
            new Product { Name = "Product 2", BasePrice = 200m, IGST = 18m, CGST = 9m, Order = 2, DataKey = "test-key" },
            new Product { Name = "Product 3", BasePrice = 300m, IGST = 18m, CGST = 9m, Order = 3, DataKey = "test-key" }
        };
        var testContext = (TestDbContext)_context;
        testContext.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // Act
        var allProducts = await _repository.GetAllAsync();

        // Assert
        allProducts.Should().HaveCount(3);
    }

    [Fact]
    public async Task FindAsync_Should_Return_Filtered_Entities()
    {
        // Arrange
        var products = new[]
        {
            new Product { Name = "Product A", BasePrice = 100m, IGST = 18m, CGST = 9m, Order = 1, DataKey = "test-key" },
            new Product { Name = "Product B", BasePrice = 200m, IGST = 18m, CGST = 9m, Order = 2, DataKey = "test-key" },
            new Product { Name = "Product C", BasePrice = 300m, IGST = 18m, CGST = 9m, Order = 3, DataKey = "test-key" }
        };
        var testContext = (TestDbContext)_context;
        testContext.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // Act
        var filteredProducts = await _repository.FindAsync(p => p.BasePrice > 150m);

        // Assert
        filteredProducts.Should().HaveCount(2);
        filteredProducts.Should().AllSatisfy(p => p.BasePrice.Should().BeGreaterThan(150m));
    }

    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        // Arrange
        var testContext = (TestDbContext)_context;
        var product = new Product
        {
            Name = "Original Name",
            BasePrice = 100m,
            IGST = 18m,
            CGST = 9m,
            Order = 1,
            DataKey = "test-key"
        };
        testContext.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        product.Name = "Updated Name";
        _repository.Update(product);
        await _context.SaveChangesAsync();

        // Assert
        var updatedProduct = await testContext.Products.FindAsync(product.Id);
        updatedProduct!.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task Remove_Should_Delete_Entity()
    {
        // Arrange
        var testContext = (TestDbContext)_context;
        var product = new Product
        {
            Name = "Product to Delete",
            BasePrice = 100m,
            IGST = 18m,
            CGST = 9m,
            Order = 1,
            DataKey = "test-key"
        };
        testContext.Products.Add(product);
        await _context.SaveChangesAsync();
        var productId = product.Id;

        // Act
        _repository.Remove(product);
        await _context.SaveChangesAsync();

        // Assert
        var deletedProduct = await testContext.Products.FindAsync(productId);
        deletedProduct.Should().BeNull();
    }

    [Fact]
    public async Task AnyAsync_Should_Return_True_When_Entities_Exist()
    {
        // Arrange
        var testContext = (TestDbContext)_context;
        var product = new Product
        {
            Name = "Test Product",
            BasePrice = 100m,
            IGST = 18m,
            CGST = 9m,
            Order = 1,
            DataKey = "test-key"
        };
        testContext.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _repository.AnyAsync(p => p.Name == "Test Product");

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task CountAsync_Should_Return_Correct_Count()
    {
        // Arrange
        var testContext = (TestDbContext)_context;
        var products = new[]
        {
            new Product { Name = "Product 1", BasePrice = 100m, IGST = 18m, CGST = 9m, Order = 1, DataKey = "test-key" },
            new Product { Name = "Product 2", BasePrice = 200m, IGST = 18m, CGST = 9m, Order = 2, DataKey = "test-key" }
        };
        testContext.Products.AddRange(products);
        await _context.SaveChangesAsync();

        // Act
        var count = await _repository.CountAsync(p => p.BasePrice >= 100m);

        // Assert
        count.Should().Be(2);
    }

    [Fact]
    public async Task Caching_Should_Work_For_GetById()
    {
        // Arrange
        var testContext = (TestDbContext)_context;
        var product = new Product
        {
            Name = "Cached Product",
            BasePrice = 100m,
            IGST = 18m,
            CGST = 9m,
            Order = 1,
            DataKey = "test-key"
        };
        testContext.Add(product);
        await _context.SaveChangesAsync();
        var productId = product.Id;

        // Act - First call should cache the result
        var firstCall = await _repository.GetByIdAsync(productId);
        
        // Detach the entity and modify it directly
        _context.Entry(product).State = EntityState.Detached;
        var productToUpdate = await testContext.Products.FindAsync(productId);
        productToUpdate!.Name = "Modified Product";
        await _context.SaveChangesAsync();
        
        // Second call should return cached value (not modified value)
        var secondCall = await _repository.GetByIdAsync(productId);

        // Assert
        firstCall.Should().NotBeNull();
        firstCall!.Name.Should().Be("Cached Product");
        secondCall.Should().NotBeNull();
        // The cache should return the same cached instance
        secondCall!.Name.Should().Be("Cached Product");
    }

    public void Dispose()
    {
        _context.Dispose();
        _cache.Dispose();
    }
}
