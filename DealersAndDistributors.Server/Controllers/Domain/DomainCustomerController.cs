using Application.Abstractions.Messaging;
using Application.Customers.Queries;
using AuthPermissions.BaseCode.CommonCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class DomainCustomerController : VersionedApiController
{
    // NEW: List tenant-local customer profiles from RetailDbContext (per-tenant data)
    [HttpGet]
    [Authorize]
    [OpenApiOperation("List per-tenant customer profiles (from tenant Retail DB).", "")]
    public async Task<ActionResult> ListTenantProfilesAsync(
        [FromServices] IQueryHandler<ListTenantCustomerProfilesQuery, PagedResult<List<TenantCustomerProfileDto>>> handler,
        int? tenantId = null,
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken ct = default)
    {
        var effectiveTenantId = tenantId ?? User.GetTenantId();
        if (effectiveTenantId == null)
            return BadRequest(Result.Failure(Error.Validation("TenantIdMissing", "Tenant id not provided or claim missing.")));

        var result = await handler.Handle(
            new ListTenantCustomerProfilesQuery(effectiveTenantId.Value, pageNumber, pageSize, search),
            ct);

        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, result);
        return Ok(result.Data);
    }
}
