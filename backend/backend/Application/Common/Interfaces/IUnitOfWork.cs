using Domain.Customers;
using Domain.Purchase;
using Domain.Accounting;
using Domain.Orders;

namespace Application.Common.Interfaces;

/// <summary>
/// Unit of Work interface to handle transactions across multiple repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repository for Product entities
    /// </summary>
    IRepository<Product> Products { get; }

    /// <summary>
    /// Repository for Route entities
    /// </summary>
    IRepository<Route> Routes { get; }

    /// <summary>
    /// Repository for PurchaseUnit entities
    /// </summary>
    IRepository<PurchaseUnit> PurchaseUnits { get; }

    /// <summary>
    /// Repository for TenantCustomerProfile entities
    /// </summary>
    IRepository<TenantCustomerProfile> TenantCustomerProfiles { get; }

    /// <summary>
    /// Repository for CustomerOrder entities
    /// </summary>
    IRepository<CustomerOrder> CustomerOrders { get; }

    /// <summary>
    /// Repository for CustomerOrderDetail entities
    /// </summary>
    IRepository<CustomerOrderDetail> CustomerOrderDetails { get; }

    /// <summary>
    /// Repository for Ledger entities
    /// </summary>
    IRepository<Ledger> Ledgers { get; }

    /// <summary>
    /// Repository for Stock entities
    /// </summary>
    IRepository<Stock> Stocks { get; }

    /// <summary>
    /// Saves all changes made in this unit of work to the database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a database transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
