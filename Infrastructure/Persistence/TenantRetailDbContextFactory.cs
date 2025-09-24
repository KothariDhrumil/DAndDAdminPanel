using AuthPermissions.BaseCode.DataLayer.EfCode;
using AuthPermissions.AspNetCore.ShardingServices;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using AuthPermissions.BaseCode.CommonCode;
using Application.Abstractions.Data;
using Application.Abstractions.Persistence;

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
        return new RetailDbContext(_options, stub, _dispatcher);
    }

    private sealed class StubGetShardingDataFromUser : AuthPermissions.AspNetCore.GetDataKeyCode.IGetShardingDataFromUser
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
