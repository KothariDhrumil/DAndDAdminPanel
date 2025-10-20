using AuthPermissions.BaseCode.CommonCode;
using Domain;
using Domain.Orders;
using Domain.Todos;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Domain.Accounting;

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
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
