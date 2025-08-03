using AuthPermissions.BaseCode.CommonCode;
using Domain;
using Domain.Todos;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IRetailDbContext : IDataKeyFilterReadOnly
{
    public string DataKey { get; }

    DbSet<RetailOutlet> RetailOutlets { get; }

    DbSet<TodoItem> TodoItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
