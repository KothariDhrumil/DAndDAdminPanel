# Repository + Unit of Work Pattern Usage Guide

This guide demonstrates how to use the newly implemented Repository and Unit of Work patterns in the DealersAndDistributors application.

## Overview

The Repository pattern abstracts data access logic and provides a consistent interface for CRUD operations, while the Unit of Work pattern manages transactions across multiple repositories.

## Features

- ✅ Generic Repository interface for CRUD operations
- ✅ Unit of Work for transaction management
- ✅ In-memory caching for read-heavy entities (Products, Routes, PurchaseUnits)
- ✅ Automatic cache invalidation on write operations
- ✅ Clean Architecture compliance
- ✅ Comprehensive unit tests

## Architecture

```
/Application
  /Common/Interfaces
    IRepository.cs         - Generic repository interface
    IUnitOfWork.cs         - Unit of work interface

/Infrastructure
  /Persistence
    /Repositories
      Repository.cs        - Generic repository implementation
    UnitOfWork.cs         - Unit of work implementation
```

## Basic Usage

### 1. Inject IUnitOfWork into Your Service

```csharp
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Product?> GetProductByIdAsync(int productId, CancellationToken ct)
    {
        // Cached read operation
        return await _unitOfWork.Products.GetByIdAsync(productId, ct);
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken ct)
    {
        // Filtered query
        return await _unitOfWork.Products.FindAsync(p => p.IsActive, ct);
    }
}
```

### 2. Create Operations

```csharp
public async Task<int> CreateProductAsync(Product product, CancellationToken ct)
{
    await _unitOfWork.Products.AddAsync(product, ct);
    return await _unitOfWork.SaveChangesAsync(ct);
}
```

### 3. Update Operations

```csharp
public async Task<int> UpdateProductAsync(Product product, CancellationToken ct)
{
    _unitOfWork.Products.Update(product);
    return await _unitOfWork.SaveChangesAsync(ct);
}
```

### 4. Delete Operations

```csharp
public async Task<int> DeleteProductAsync(Product product, CancellationToken ct)
{
    _unitOfWork.Products.Remove(product);
    return await _unitOfWork.SaveChangesAsync(ct);
}
```

### 5. Transaction Management

```csharp
public async Task ProcessOrderWithTransactionAsync(CustomerOrder order, CancellationToken ct)
{
    try
    {
        // Begin transaction
        await _unitOfWork.BeginTransactionAsync(ct);

        // Add order
        await _unitOfWork.CustomerOrders.AddAsync(order, ct);
        
        // Add order details
        await _unitOfWork.CustomerOrderDetails.AddRangeAsync(order.CustomerOrderDetails, ct);
        
        // Update stock
        foreach (var detail in order.CustomerOrderDetails)
        {
            var stock = await _unitOfWork.Stocks.FirstOrDefaultAsync(
                s => s.ProductId == detail.ProductId && s.RouteId == order.RouteId, 
                ct);
            
            if (stock != null)
            {
                stock.Quantity -= detail.Quantity;
                _unitOfWork.Stocks.Update(stock);
            }
        }

        // Commit all changes atomically
        await _unitOfWork.CommitTransactionAsync(ct);
    }
    catch
    {
        // Rollback on any error
        await _unitOfWork.RollbackTransactionAsync(ct);
        throw;
    }
}
```

## Caching

The following repositories have caching enabled by default:
- **Products** - 10-minute cache duration
- **Routes** - 10-minute cache duration
- **PurchaseUnits** - 10-minute cache duration

Cache is automatically invalidated when:
- Entities are added
- Entities are updated
- Entities are deleted

### Cache Behavior

- **GetByIdAsync**: Cached per entity ID
- **GetAllAsync**: Cached for all entities
- **FindAsync**: Not cached (dynamic queries)
- **Write operations**: Automatically invalidate cache

## Available Repository Methods

```csharp
// Read operations
Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

// Write operations
Task AddAsync(T entity, CancellationToken ct = default);
Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default);
void Update(T entity);
void Remove(T entity);
void RemoveRange(IEnumerable<T> entities);
```

## Available Repositories in UnitOfWork

```csharp
IRepository<Product> Products
IRepository<Route> Routes
IRepository<PurchaseUnit> PurchaseUnits
IRepository<TenantCustomerProfile> TenantCustomerProfiles
IRepository<CustomerOrder> CustomerOrders
IRepository<CustomerOrderDetail> CustomerOrderDetails
IRepository<Ledger> Ledgers
IRepository<Stock> Stocks
```

## Best Practices

1. **Always use CancellationToken** for async operations
2. **Use transactions** for operations that span multiple repositories
3. **Leverage caching** for read-heavy operations (Products, Routes, PurchaseUnits)
4. **Dispose UnitOfWork** properly (use `using` statements or dependency injection)
5. **Handle exceptions** from transaction operations appropriately
6. **Use FindAsync** for filtered queries instead of loading all entities

## Migration from Direct DbContext Usage

### Before (Direct DbContext)
```csharp
public class OldService
{
    private readonly IRetailDbContext _db;
    
    public async Task<Product?> GetProductAsync(int id)
    {
        return await _db.Products.FindAsync(id);
    }
    
    public async Task CreateProductAsync(Product product)
    {
        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync();
    }
}
```

### After (Repository Pattern)
```csharp
public class NewService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public async Task<Product?> GetProductAsync(int id, CancellationToken ct)
    {
        // Now with caching!
        return await _unitOfWork.Products.GetByIdAsync(id, ct);
    }
    
    public async Task CreateProductAsync(Product product, CancellationToken ct)
    {
        await _unitOfWork.Products.AddAsync(product, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}
```

## Testing

The repository pattern is fully unit tested. See:
- `tests/Infrastructure.Tests/Persistence/RepositoryTests.cs`
- `tests/Infrastructure.Tests/Persistence/UnitOfWorkTests.cs`

Example test:
```csharp
[Fact]
public async Task GetByIdAsync_Should_Return_Entity_From_Database()
{
    // Arrange
    var product = new Product { Name = "Test Product", BasePrice = 100m };
    await repository.AddAsync(product);
    await context.SaveChangesAsync();

    // Act
    var result = await repository.GetByIdAsync(product.Id);

    // Assert
    result.Should().NotBeNull();
    result!.Name.Should().Be("Test Product");
}
```

## Performance Considerations

- Caching reduces database round-trips for frequently accessed data
- Cache duration is 10 minutes by default (configurable in Repository.cs)
- Individual entity cache keys expire naturally; the 'All' cache is invalidated on writes
- For immediate consistency requirements, consider disabling caching or reducing cache duration
- Transaction overhead is minimal; use for logical units of work only

## Troubleshooting

### Cache Not Working
- Ensure `IMemoryCache` is registered in DI (automatically done by `AddRepositoryPattern()`)
- Check if caching is enabled for the specific repository (see UnitOfWork.cs)

### Transaction Errors
- Always wrap transaction logic in try-catch with rollback
- Ensure BeginTransactionAsync is called before operations
- Call CommitTransactionAsync to persist changes

### Performance Issues
- Use FindAsync instead of GetAllAsync + LINQ filtering
- Consider batch operations with AddRangeAsync/RemoveRange
- Profile cache hit rates if performance is critical

## Future Enhancements

Potential improvements for future iterations:
- Configurable cache duration per entity type
- Distributed caching support (Redis)
- Query specification pattern for complex queries
- Soft delete support at repository level
- Audit logging at repository level
