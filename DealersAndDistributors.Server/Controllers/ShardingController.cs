using AuthPermissions.AspNetCore;
using AuthPermissions.AspNetCore.ShardingServices;
using AuthPermissions.BaseCode.DataLayer.Classes;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace DealersAndDistributors.Server.Controllers;

public class ShardingController : VersionNeutralApiController
{
    private readonly IGetSetShardingEntries _shardingService;
    public ShardingController(IGetSetShardingEntries shardingService)
    {
        _shardingService = shardingService;
    }

    [HttpGet("get-all-sharding-entries")]
    public ActionResult<List<ShardingEntry>> GetShardingEntries()
    {
        var entries = _shardingService.GetAllShardingEntries();
        return Ok(entries);
    }



    [HttpGet("get-sharding-db-details")]
    public ActionResult<ShardingEntryEdit> GetShardingDBDetails()
    {
        var dto = new ShardingEntryEdit
        {
            AllPossibleConnectionNames = _shardingService.GetConnectionStringNames(),
            PossibleDatabaseTypes = _shardingService.PossibleDatabaseProviders
        };
        return Ok(dto);
    }

    //Create a new sharding entry
    [HttpPost]
    [HasPermission(Permissions.TenantCreate)]
    public ActionResult CreateShardingEntryAsync(ShardingEntry newEntry)
    {
        var status = _shardingService.AddNewShardingEntry(newEntry);

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(status.Message);
    }

    //Update an existing sharding entry
    [HttpPut]
    [HasPermission(Permissions.UpdateDatabaseInfo)]
    public ActionResult UpdateShardingEntryAsync(ShardingEntry updatedEntry)
    {
        var status = _shardingService.UpdateShardingEntry(updatedEntry);
        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(status.Message);
    }

    //Delete a sharding entry
    [HttpDelete]
    [HasPermission(Permissions.RemoveDatabaseInfo)]
    public ActionResult DeleteShardingEntryAsync(string connectionName)
    {
        var status = _shardingService.RemoveShardingEntry(connectionName);
        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(status.Message);
    }

    [HasPermission(Permissions.CheckDatabaseInfo)]
    [HttpGet("check-sharding-sources")]
    public IActionResult Check()
    {
        var status = _shardingService.CheckTwoShardingSources();

        return status.HasErrors
            ? throw new Exception(status.GetAllErrors())
            : Ok(status.Message);
    }
}


public class ShardingEntryEdit
{
    public ShardingEntry DatabaseInfo { get; set; }

    public IEnumerable<string> AllPossibleConnectionNames { get; set; }

    public string[] PossibleDatabaseTypes { get; set; }
}
