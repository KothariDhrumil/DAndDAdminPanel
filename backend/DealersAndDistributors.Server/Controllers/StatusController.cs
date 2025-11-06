using AuthPermissions.AspNetCore;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.SupportCode.DownStatusCode;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Shared;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers;

public class StatusController : VersionNeutralApiController
{
    private readonly ISetRemoveStatus _status;

    public StatusController(ISetRemoveStatus status)
    {
        _status = status;
    }

    [HttpGet]
    [HasPermission(Permissions.AppStatusList)]
    [OpenApiOperation("Get a list of tenant's up/down status.", "")]
    public ActionResult<List<KeyValuePair<string, string>>> GetList()
    {
        return Ok(Result.Success(_status.GetAllDownKeyValues()));
    }

    [HttpPost()]
    [HasPermission(Permissions.AppStatusAllDown)]
    [OpenApiOperation("Set app status to down for maintenance for everyone.", "")]
    public IActionResult TakeAllDown(ManuelAppDownDto data)
    {
        data.UserId = User.GetUserIdFromUser();
        data.StartedUtc = DateTime.UtcNow;

        _status.SetAppDown(data);
        return Ok(Result.Success());
    }

    [HttpPost("{tenantId:int}")]
    [HasPermission(Permissions.AppStatusTenantDown)]
    [OpenApiOperation("Set app status to down for maintenance for a specific tenant.", "")]
    public async Task<IActionResult> TakeTenantDown(int tenantId)
    {
        await _status.SetTenantDownWithDelayAsync(TenantDownVersions.ManualDown, tenantId);
        return Ok(Result.Success());
    }

    /// <summary>
    /// This can remove ANY down status from the list
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpPost("remove/{key}")]
    [HasPermission(Permissions.AppStatusRemove)]
    [OpenApiOperation("Remove a tenant from the down for maintenance list.", "")]
    public IActionResult Remove(string key)
    {
        _status.RemoveAnyDown(key);
        return Ok(Result.Success());
    }
}

