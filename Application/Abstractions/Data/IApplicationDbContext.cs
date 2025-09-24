using AuthPermissions.BaseCode.CommonCode;
using Domain;
using Domain.Orders;
using Domain.Todos;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IRetailDbContext : IDataKeyFilterReadOnly
{
    public string DataKey { get; }

    DbSet<RetailOutlet> RetailOutlets { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<Order> Orders { get; }
    DbSet<TenantCustomerProfile> TenantCustomerProfiles { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
