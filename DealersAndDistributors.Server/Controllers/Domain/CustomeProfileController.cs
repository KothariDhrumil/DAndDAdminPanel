using Application.Abstractions.Messaging;
using Application.Customers.Queries;
using Application.Customers.Update;
using AuthPermissions.BaseCode.CommonCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class CustomerProfileController : VersionedApiController
{
    // NEW: List tenant-local customer profiles from RetailDbContext (per-tenant data)
    [HttpGet]
    [Authorize]
    [OpenApiOperation("List per-tenant customer profiles (from tenant Retail DB).", "")]
    public async Task<ActionResult> ListTenantProfilesAsync(
        [FromServices] IQueryHandler<ListTenantCustomerProfilesQuery, PagedResult<List<TenantCustomerProfileDto>>> handler,
        int? tenantId = null,
        int? routeId = null,
        int pageNumber = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken ct = default)
    {
        var effectiveTenantId = tenantId ?? User.GetTenantId();
        if (effectiveTenantId == null)
            return BadRequest(Result.Failure(Error.Validation("TenantIdMissing", "Tenant id not provided or claim missing.")));

        var result = await handler.Handle(
            new ListTenantCustomerProfilesQuery(effectiveTenantId.Value, pageNumber, pageSize, search, routeId),
            ct);

        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, result);
        return Ok(result.Data);
    }


    // HttpGet to retrieve tenant-local customer profile from RetailDbContext (per-tenant data)
    [HttpGet("{globalCustomerId:guid}")]
    [Authorize]
    [OpenApiOperation("Get per-tenant customer profile (from tenant Retail DB).", "")]
    public async Task<ActionResult> GetTenantProfileAsync(
        [FromServices] IQueryHandler<GetTenantCustomerProfileQuery, TenantCustomerDetailProfileDto> handler,
        Guid globalCustomerId,
        int? tenantId = null,
        CancellationToken ct = default)
    {
        var effectiveTenantId = tenantId ?? User.GetTenantId();
        if (effectiveTenantId == null)
            return BadRequest(Result.Failure(Error.Validation("TenantIdMissing", "Tenant id not provided or claim missing.")));
        var result = await handler.Handle(
            new GetTenantCustomerProfileQuery(globalCustomerId),
            ct);
        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, result);
        return Ok(result.Data);
    }


    // Update tenant-local customer profile in RetailDbContext (per-tenant data)
    [HttpPut]
    [Authorize]
    [OpenApiOperation("Update per-tenant customer profile (in tenant Retail DB).", "")]
    public async Task<ActionResult> UpdateTenantProfileAsync(
        [FromServices] ICommandHandler<UpdateCustomerProfileByTenantCommand> handler,
        [FromBody] UpdateCustomerProfileByTenantCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, result);
        return Ok(result);
    }

    [HttpPut("{customerId:guid}/route")]
    public async Task<IActionResult> UpdateCustomerRouteAsync(
    Guid customerId,
    [FromBody] UpdateCustomerRouteCommand command,
    ICommandHandler<UpdateCustomerRouteCommand> handler,
    CancellationToken cancellationToken)
    {
        command.TenantUserId = customerId;
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("route-customer-sequence")]
    [Authorize]
    [OpenApiOperation("Update customer sequence for all customers in a route.", "")]
    public async Task<IActionResult> UpdateCustomerRouteSequenceAsync(
        [FromQuery] int routeId,
        [FromBody] List<CustomerSequenceUpdateDto> updates,
        [FromServices] ICommandHandler<UpdateCustomerRouteSequenceCommand> handler,
        CancellationToken ct)
    {
        var command = new UpdateCustomerRouteSequenceCommand
        {
            RouteId = routeId,
            Updates = updates
        };
        var result = await handler.Handle(command, ct);
        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, result);
        return Ok(result);
    }
}
