using AuthPermissions.AspNetCore.GetDataKeyCode;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using Domain;
using Microsoft.EntityFrameworkCore;
using AuthPermissions.BaseCode.CommonCode;

namespace Infrastructure.Persistence.Contexts;

public class RetailDbContext : DbContext, IDataKeyFilterReadOnly
{
    public string DataKey { get; }

    public RetailDbContext(DbContextOptions<RetailDbContext> options, IGetShardingDataFromUser shardingDataKeyAndConnect)
        : base(options)
    {
        // The DataKey is null when: no one is logged in, its a background service, or user hasn't got an assigned tenant
        // In these cases its best to set the data key that doesn't match any possible DataKey 
        DataKey = shardingDataKeyAndConnect?.DataKey ?? "stop any user without a DataKey to access the data";

        if (shardingDataKeyAndConnect?.ConnectionString != null)
            //NOTE: If no connection string is provided the DbContext will use the connection it was provided when it was registered
            //If you don't want that to happen, then remove the if above and the connection will be set to null (and fail) 
            Database.SetConnectionString(shardingDataKeyAndConnect.ConnectionString);
    }

    public DbSet<RetailOutlet> RetailOutlets => Set<RetailOutlet>();
    public DbSet<ShopStock> ShopStocks => Set<ShopStock>();
    public DbSet<ShopSale> ShopSales => Set<ShopSale>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // It is currently not possible to define multiple query filters on the same entity - only the last one will be applied.
        // However, you can define a single filter with multiple conditions using the logical AND operator (&& in C#).
        // See https://docs.microsoft.com/en-us/ef/core/querying/filters
        // This way you can chain multiple query filters for the entity.
        modelBuilder
           .AppendGlobalQueryFilter<IDataKeyFilterReadOnly>(s => s.DataKey.StartsWith(DataKey));

        modelBuilder.HasDefaultSchema("retail");

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IDataKeyFilterReadOnly).IsAssignableFrom(entityType.ClrType))
            {
                entityType.AddHierarchicalTenantReadOnlyQueryFilter(this);
            }
            else
            {
                throw new Exception(
                    $"You haven't added the {nameof(IDataKeyFilterReadOnly)} to the entity {entityType.ClrType.Name}");
            }

            foreach (var mutableProperty in entityType.GetProperties())
            {
                if (mutableProperty.ClrType == typeof(decimal))
                {
                    mutableProperty.SetPrecision(9);
                    mutableProperty.SetScale(2);
                }
            }
        }
    }
}
