using AuthPermissions.BaseCode.CommonCode;
using Domain;
using Domain.Orders;
using Domain.Todos;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Domain.Accounting;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Abstractions.Data;

public interface IRetailDbContext : IDataKeyFilterReadOnly
{
    public string DataKey { get; }
    DatabaseFacade Database { get; }
    DbSet<RetailOutlet> RetailOutlets { get; }
    DbSet<TenantCustomerProfile> TenantCustomerProfiles { get; }
    DbSet<TenantUserProfile> TenantUserProfiles { get; }
    DbSet<TodoItem> TodoItems { get; }
    DbSet<Order> Orders { get; }
    DbSet<LedgerEntry> LedgerEntries { get; }
    DbSet<UserType> UserTypes { get; }
    DbSet<Route> Routes { get; }
    DbSet<Product> Products { get; }
    DbSet<CustomerProduct> CustomerProducts { get; }
    DbSet<PriceTier> PriceTiers { get; }
    DbSet<PriceTierProduct> PriceTierProducts { get; }
    DbSet<PurchaseUnit> PurchaseUnits { get; }
    DbSet<PurchaseUnitProduct> PurchaseUnitProducts { get; }
    DbSet<Purchase> Purchases { get; }
    DbSet<PurchaseDetail> PurchaseDetails { get; }
    DbSet<CustomerOrder> CustomerOrders { get; }
    DbSet<CustomerOrderDetail> CustomerOrderDetails { get; }
    DbSet<Ledger> Ledgers { get; }
    DbSet<RouteStock> RouteStocks { get; }
    DbSet<WarehouseStock> WarehouseStocks { get; }
    DbSet<StockTransaction> StockTransactions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
