using AuthPermissions.BaseCode.CommonCode;
using Domain;
using Domain.Orders;
using Domain.Todos;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Domain.Accounting;
using Domain.Purchase;

namespace Application.Abstractions.Data;

public interface IRetailDbContext : IDataKeyFilterReadOnly
{
    public string DataKey { get; }
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
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
