using Application.Common.Interfaces;
using Domain.Accounting;
using Domain.Customers;
using Domain.Orders;
using Domain.Purchase;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Persistence;

/// <summary>
/// Unit of Work implementation to handle transactions across multiple repositories
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly RetailDbContext _context;
    private readonly IMemoryCache _cache;
    private IDbContextTransaction? _transaction;

    private IRepository<Product>? _products;
    private IRepository<Route>? _routes;
    private IRepository<PurchaseUnit>? _purchaseUnits;
    private IRepository<TenantCustomerProfile>? _tenantCustomerProfiles;
    private IRepository<CustomerOrder>? _customerOrders;
    private IRepository<CustomerOrderDetail>? _customerOrderDetails;
    private IRepository<Ledger>? _ledgers;
    private IRepository<Stock>? _stocks;

    public UnitOfWork(RetailDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public IRepository<Product> Products =>
        _products ??= new Repository<Product>(_context, _cache, enableCaching: true);

    public IRepository<Route> Routes =>
        _routes ??= new Repository<Route>(_context, _cache, enableCaching: true);

    public IRepository<PurchaseUnit> PurchaseUnits =>
        _purchaseUnits ??= new Repository<PurchaseUnit>(_context, _cache, enableCaching: true);

    public IRepository<TenantCustomerProfile> TenantCustomerProfiles =>
        _tenantCustomerProfiles ??= new Repository<TenantCustomerProfile>(_context, _cache, enableCaching: false);

    public IRepository<CustomerOrder> CustomerOrders =>
        _customerOrders ??= new Repository<CustomerOrder>(_context, _cache, enableCaching: false);

    public IRepository<CustomerOrderDetail> CustomerOrderDetails =>
        _customerOrderDetails ??= new Repository<CustomerOrderDetail>(_context, _cache, enableCaching: false);

    public IRepository<Ledger> Ledgers =>
        _ledgers ??= new Repository<Ledger>(_context, _cache, enableCaching: false);

    public IRepository<Stock> Stocks =>
        _stocks ??= new Repository<Stock>(_context, _cache, enableCaching: false);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
