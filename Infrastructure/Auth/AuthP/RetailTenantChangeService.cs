using AuthPermissions.AdminCode;
using AuthPermissions.AspNetCore.GetDataKeyCode;
using AuthPermissions.AspNetCore.ShardingServices;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.BaseCode.DataLayer.Classes;
using Domain;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;
using TestSupport.SeedDatabase;

namespace Infrastructure.Auth.AuthP;

public class RetailTenantChangeService : ITenantChangeService
{
    private readonly DbContextOptions<RetailDbContext> _options;
    private readonly ILogger _logger;
    private readonly IGetSetShardingEntries _shardingService;

    public IReadOnlyList<int> DeletedTenantIds { get; private set; } = default!;

    public RetailTenantChangeService(DbContextOptions<RetailDbContext> options, ILogger<RetailTenantChangeService> logger, IGetSetShardingEntries shardingService)
    {
        _options = options;
        _logger = logger;
        _shardingService = shardingService;
    }

    /// <summary>
    /// When a new AuthP Tenant is created, then this method is called. If you have a tenant-type entity in your
    /// application's database, then this allows you to create a new entity for the new tenant.
    /// You should apply multiple changes within a transaction so that if any fails then any previous changes will be rolled back.
    /// NOTE: With hierarchical tenants you cannot be sure that the tenant has, or will have, children so we always add a retail
    /// </summary>
    /// <param name="tenant"></param>
    /// <returns>Returns null if all OK, otherwise the create is rolled back and the return string is shown to the user</returns>
    public async Task<string?> CreateNewTenantAsync(Tenant tenant)
    {
        using var context = GetShardingSingleDbContext(tenant.DatabaseInfoName, tenant.GetTenantDataKey());
        if (context == null)
            return $"There is no connection string with the name {tenant.DatabaseInfoName}.";

        var databaseError = await CheckDatabaseAndPossibleMigrate(context, tenant, true);
        if (databaseError != null)
            return databaseError;

        if (tenant.HasOwnDb && context.RetailOutlets.IgnoreQueryFilters().Any())
            return
                $"The tenant's {nameof(Tenant.HasOwnDb)} property is true, but the database contains existing companies";


        context.Add(new RetailOutlet(tenant.TenantId, tenant.TenantFullName, tenant.GetTenantDataKey()));
        await context.SaveChangesAsync();

        return null;
    }

    //not used
    public async Task<string> SingleTenantUpdateNameAsync(Tenant tenant)
    {
        using var context = GetShardingSingleDbContext(tenant.DatabaseInfoName, tenant.GetTenantDataKey());
        if (context == null)
            return $"There is no connection string with the name {tenant.DatabaseInfoName}.";

        var companyTenant = await context.RetailOutlets
            .SingleOrDefaultAsync(x => x.AuthPTenantId == tenant.TenantId);
        if (companyTenant != null)
        {
            companyTenant.FullName = tenant.TenantFullName;
            await context.SaveChangesAsync();
        }

        return null;
    }

    /// <summary>
    /// This is called when the name of your Tenants is changed. This is useful if you use the tenant name in your multi-tenant data.
    /// NOTE: The created application's DbContext won't have a DataKey, so you will need to use IgnoreQueryFilters on any EF Core read.
    /// You should apply multiple changes within a transaction so that if any fails then any previous changes will be rolled back.
    /// </summary>
    /// <param name="tenantsToUpdate">This contains the tenants to update.</param>
    /// <returns>Returns null if all OK, otherwise the name change is rolled back and the return string is shown to the user</returns>
    public async Task<string?> HierarchicalTenantUpdateNameAsync(List<Tenant> tenantsToUpdate)
    {
        var tenantChild = tenantsToUpdate.First();

        using var context = GetShardingSingleDbContext(tenantChild.DatabaseInfoName, tenantChild.GetTenantDataKey());
        if (context == null)
            return $"There is no connection string with the name {tenantChild.DatabaseInfoName}.";
        var databaseError = await CheckDatabaseAndPossibleMigrate(context, tenantChild, true);
        if (databaseError != null)
            return databaseError;


        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            foreach (var tenant in tenantsToUpdate)
            {
                //Higher hierarchical levels don't have data in this example
                var retailOutletToUpdate =
                    await context.RetailOutlets
                        .IgnoreQueryFilters().SingleOrDefaultAsync(x => x.AuthPTenantId == tenant.TenantId);

                if (retailOutletToUpdate != null)
                {
                    retailOutletToUpdate.UpdateNames(tenant.TenantFullName);
                    await context.SaveChangesAsync();
                }

            }
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failure when trying to update a hierarchical tenant.");
            return "There was a system-level problem - see logs for more detail";
        }


        return null;
    }

    //Not used
    public Task<string> SingleTenantDeleteAsync(Tenant tenant)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// This is used with hierarchical tenants to either
    /// a) delete all the application-side data with the given DataKey, or
    /// b) soft-delete the data.
    /// You should apply multiple changes within a transaction so that if any fails then any previous changes will be rolled back
    /// Notes:
    /// - The created application's DbContext won't have a DataKey, so you will need to use IgnoreQueryFilters on any EF Core read
    /// - You can provide information of what you have done by adding public parameters to this class.
    ///   The TenantAdmin <see cref="AuthTenantAdminService.DeleteTenantAsync"/> method returns your class on a successful Delete
    /// </summary>
    /// <param name="tenantsInOrder">The tenants to delete with the children first in case a higher level links to a lower level</param>
    /// <returns>Returns null if all OK, otherwise the AuthP part of the delete is rolled back and the return string is shown to the user</returns>

    public async Task<string?> HierarchicalTenantDeleteAsync(List<Tenant> tenantsInOrder)
    {
        var firstTenant = tenantsInOrder.First();
        using var context = GetShardingSingleDbContext(firstTenant.DatabaseInfoName, firstTenant.GetTenantDataKey());
        if (context == null)
            return $"There is no connection string with the name {firstTenant.DatabaseInfoName}.";

        //If the database doesn't exist then log it and return
        if (!await context.Database.CanConnectAsync())
        {
            _logger.LogWarning("DeleteTenantData: asked to remove tenant data / database, but no database found. " +
                               $"Tenant name = {firstTenant?.TenantFullName ?? "- not available -"}");
            return null;
        }


        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        try
        {
            var deletedTenantIds = new List<int>();
            foreach (var tenant in tenantsInOrder)
            {
                //Higher hierarchical levels don't have data in this example, so it only tries to delete data if there is a RetailOutlet
                var retailOutletToDelete =
                    await context.RetailOutlets
                        .IgnoreQueryFilters().SingleOrDefaultAsync(x => x.AuthPTenantId == tenant.TenantId);
                if (retailOutletToDelete != null)
                {
                    //yes, its a shop so delete all the stock / sales 
                    var deleteSalesSql =
                        $"DELETE FROM retail.{nameof(RetailDbContext.ShopSales)} WHERE DataKey = '{tenant.GetTenantDataKey()}'";
                    await context.Database.ExecuteSqlRawAsync(deleteSalesSql);
                    var deleteStockSql =
                        $"DELETE FROM retail.{nameof(RetailDbContext.ShopStocks)} WHERE DataKey = '{tenant.GetTenantDataKey()}'";
                    await context.Database.ExecuteSqlRawAsync(deleteStockSql);

                    context.Remove(retailOutletToDelete); //finally delete the RetailOutlet
                    await context.SaveChangesAsync();
                    deletedTenantIds.Add(tenant.TenantId);
                }
            }

            await transaction.CommitAsync();
            DeletedTenantIds = deletedTenantIds.AsReadOnly();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failure when trying to delete a hierarchical tenant.");
            return "There was a system-level problem - see logs for more detail";
        }



        return null; //null means OK, otherwise the delete is rolled back and the return string is shown to the user
    }


    /// <summary>
    /// This is used with hierarchical tenants, where you move one tenant (and its children) to another tenant
    /// This requires you to change the DataKeys of each application's tenant data, so they link to the new tenant.
    /// Also, if you contain the name of the tenant in your data, then you need to update its new FullName
    /// Notes:
    /// - The created application's DbContext won't have a DataKey, so you will need to use IgnoreQueryFilters on any EF Core read
    /// - You can get multiple calls if move a higher level
    /// </summary>
    /// <param name="tenantToUpdate">The data to update each tenant. This starts at the parent and then recursively works down the children</param>
    /// <returns>Returns null if all OK, otherwise AuthP part of the move is rolled back and the return string is shown to the user</returns>
    public async Task<string?> MoveHierarchicalTenantDataAsync(List<(string oldDataKey, Tenant tenantToMove)> tenantToUpdate)
    {

        var firstTenant = tenantToUpdate.First();
        using var context = GetShardingSingleDbContext(firstTenant.tenantToMove.DatabaseInfoName, firstTenant.tenantToMove.GetTenantDataKey());
        if (context == null)
            return $"There is no connection string with the name {firstTenant.tenantToMove.DatabaseInfoName}.";

        //If the database doesn't exist then log it and return
        if (!await context.Database.CanConnectAsync())
        {
            _logger.LogWarning("DeleteTenantData: asked to remove tenant data / database, but no database found. " +
                               $"Tenant name = {firstTenant.tenantToMove?.TenantFullName ?? "- not available -"}");
            return null;
        }


        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            foreach (var tuple in tenantToUpdate)
            {
                //Higher hierarchical levels don't have data in this example, so it only tries to move data if there is a RetailOutlet
                var retailOutletMove =
                    await context.RetailOutlets
                        .IgnoreQueryFilters()
                        .SingleOrDefaultAsync(x => x.AuthPTenantId == tuple.tenantToMove.TenantId);
                if (retailOutletMove != null)
                {
                    //yes, its a shop so move all the stock / sales 

                    //This code will update the DataKey of every entity that has the IDataKeyFilterReadOnly interface
                    foreach (var entityType in context.Model.GetEntityTypes()
                                 .Where(x => typeof(IDataKeyFilterReadOnly).IsAssignableFrom(x.ClrType)))
                    {
                        var updateDataKey = $"UPDATE {entityType.FormSchemaTableFromModel()} " +
                                           $"SET DataKey = '{tuple.tenantToMove.GetTenantDataKey()}' WHERE DataKey = '{tuple.oldDataKey}'";
                        await context.Database.ExecuteSqlRawAsync(updateDataKey);
                    }

                    retailOutletMove.UpdateNames(tuple.tenantToMove.TenantFullName);
                    await context.SaveChangesAsync();
                }
            }

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failure when trying to Move a hierarchical tenant.");
            return "There was a system-level problem - see logs for more detail";
        }

        return null;
    }

    public async Task<string> MoveToDifferentDatabaseAsync(string oldDatabaseInfoName, string oldDataKey,
        Tenant updatedTenant)
    {
        //NOTE: The oldContext and newContext have the correct DataKey so you don't have to use IgnoreQueryFilters.
        var oldContext = GetShardingSingleDbContext(oldDatabaseInfoName, oldDataKey);
        if (oldContext == null)
            return $"There is no connection string with the name {oldDatabaseInfoName}.";

        var newContext = GetShardingSingleDbContext(updatedTenant.DatabaseInfoName, updatedTenant.GetTenantDataKey());
        if (newContext == null)
            return $"There is no connection string with the name {updatedTenant.DatabaseInfoName}.";

        var databaseError = await CheckDatabaseAndPossibleMigrate(newContext, updatedTenant, true);
        if (databaseError != null)
            return databaseError;

        await using var transactionNew = await newContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var invoicesWithLineItems = await oldContext.RetailOutlets.AsNoTracking().ToListAsync();


            //NOTE: writing the entities to the database will set the DataKey on a non-sharding tenant,
            //but if its a sharding tenant then the DataKey won't be changed, BUT if you want the DataKey cleared out see the RetailTenantChangeService.MoveHierarchicalTenantDataAsync to manually set the DataKey
            var resetter = new DataResetter(newContext);
            //This resets the primary / foreign keys to their default value ready to write into the new database
            //This method comes from my EfCore.TestSupport library as was used to store data and add it back.
            //see the extract part documentation vai https://github.com/JonPSmith/EfCore.TestSupport/wiki/Seed-from-Production-feature
            resetter.ResetKeysEntityAndRelationships(invoicesWithLineItems);

            newContext.AddRange(invoicesWithLineItems);

            var companyTenant = await oldContext.RetailOutlets.AsNoTracking().SingleOrDefaultAsync();
            if (companyTenant != null)
            {
                companyTenant.RetailOutletId = default;
                newContext.Add(companyTenant);
            }

            await newContext.SaveChangesAsync();

            //Now we try to delete the old data
            await using var transactionOld = await oldContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                await DeleteTenantData(oldDataKey, oldContext);

                await transactionOld.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failure when trying to delete the original tenant data after the copy over.");
                return "There was a system-level problem - see logs for more detail";
            }

            await transactionNew.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failure when trying to copy the tenant data to the new database.");
            return "There was a system-level problem - see logs for more detail";
        }

        return null;
    }

    private async Task DeleteTenantData(string dataKey, RetailDbContext context, Tenant? tenant = null)
    {
        if (tenant?.HasOwnDb == true)
        {
            //The tenant its own database, then you should drop the database, but that depends on what SQL Server provider you use.
            //In this case I can the database because it is on a local SqlServer server.
            await context.Database.EnsureDeletedAsync();
            return;
        }

        //else we remove all the data with the DataKey of the tenant
        var deleteSalesSql = $"DELETE FROM retail.{nameof(RetailDbContext.ShopSales)} WHERE DataKey = '{dataKey}'";
        await context.Database.ExecuteSqlRawAsync(deleteSalesSql);
        var deleteStockSql = $"DELETE FROM retail.{nameof(RetailDbContext.ShopStocks)} WHERE DataKey = '{dataKey}'";
        await context.Database.ExecuteSqlRawAsync(deleteStockSql);

        var companyTenant = await context.RetailOutlets.SingleOrDefaultAsync();
        if (companyTenant != null)
        {
            context.Remove(companyTenant);
            await context.SaveChangesAsync();
        }
    }

    private RetailDbContext? GetShardingSingleDbContext(string databaseDataName, string dataKey)
    {
        var connectionString = _shardingService.FormConnectionString(databaseDataName);
        if (connectionString == null)
            return null;

        return new RetailDbContext(_options, new StubGetShardingDataFromUser(connectionString, dataKey));
    }

    /// <summary>
    /// This check is a database is there 
    /// </summary>
    /// <param name="context">The context for the new database</param>
    /// <param name="tenant"></param>
    /// <param name="migrateEvenIfNoDb">If using local SQL server, Migrate will create the database.
    /// That doesn't work on Azure databases</param>
    /// <returns></returns>
    private static async Task<string?> CheckDatabaseAndPossibleMigrate(RetailDbContext context, Tenant tenant,
        bool migrateEvenIfNoDb)
    {
        //Thanks to https://stackoverflow.com/questions/33911316/entity-framework-core-how-to-check-if-database-exists
        //There are various options to detect if a database is there - this seems the clearest
        if (!await context.Database.CanConnectAsync())
        {
            //The database doesn't exist
            if (migrateEvenIfNoDb)
                await context.Database.MigrateAsync();
            else
            {
                return $"The database defined by the connection string '{tenant.DatabaseInfoName}' doesn't exist.";
            }
        }
        else if (!await context.Database.GetService<IRelationalDatabaseCreator>().HasTablesAsync())
            //The database exists but needs migrating
            await context.Database.MigrateAsync();

        return null;
    }

    private class StubGetShardingDataFromUser : IGetShardingDataFromUser
    {
        public StubGetShardingDataFromUser(string connectionString, string dataKey)
        {
            ConnectionString = connectionString;
            DataKey = dataKey;
        }

        public string DataKey { get; }
        public string ConnectionString { get; }
    }

}