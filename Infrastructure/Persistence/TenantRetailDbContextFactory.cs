using AuthPermissions.BaseCode.DataLayer.EfCode;
using AuthPermissions.AspNetCore.ShardingServices;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using AuthPermissions.BaseCode.CommonCode;
using Application.Abstractions.Data;
using Application.Abstractions.Persistence;
using AuthPermissions.AspNetCore.GetDataKeyCode;

namespace Infrastructure.Persistence;

public sealed class TenantRetailDbContextFactory : ITenantRetailDbContextFactory
{
    private readonly DbContextOptions<RetailDbContext> _options;
    private readonly AuthPermissionsDbContext _authDb;
    private readonly IGetSetShardingEntries _sharding;
    private readonly Infrastructure.DomainEvents.IDomainEventsDispatcher _dispatcher;

    public TenantRetailDbContextFactory(
        DbContextOptions<RetailDbContext> options,
        AuthPermissionsDbContext authDb,
        IGetSetShardingEntries sharding,
        Infrastructure.DomainEvents.IDomainEventsDispatcher dispatcher)
    {
        _options = options;
        _authDb = authDb;
        _sharding = sharding;
        _dispatcher = dispatcher;
    }

    public async Task<IRetailDbContext> CreateAsync(int tenantId, CancellationToken ct)
    {
        var tenant = await _authDb.Tenants.AsNoTracking().SingleAsync(t => t.TenantId == tenantId, ct);
        var connectionString = _sharding.FormConnectionString(tenant.DatabaseInfoName);
        var dataKey = tenant.GetTenantDataKey();
        var stub = new StubGetShardingDataFromUser(connectionString, dataKey);
        var context = new RetailDbContext(_options, stub, _dispatcher);

        // Apply pending migrations (if any) for this tenant database
        try
        {
            var pending = await context.Database.GetPendingMigrationsAsync(ct);
            if (pending.Any())
            {
                await context.Database.MigrateAsync(ct);
            }
        }
        catch (Exception ex)
        {
            // Optional: log the migration failure; for now we rethrow to surface issues early
            throw new InvalidOperationException($"Failed to apply migrations for tenant {tenantId}.", ex);
        }

        return context;
    }

    private sealed class StubGetShardingDataFromUser : IGetShardingDataFromUser
    {
        public StubGetShardingDataFromUser(string connection, string dataKey)
        {
            ConnectionString = connection;
            DataKey = dataKey;
        }
        public string DataKey { get; }
        public string ConnectionString { get; }
    }
}
