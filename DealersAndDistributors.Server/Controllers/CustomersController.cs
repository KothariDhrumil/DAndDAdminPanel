using Application.Abstractions.Messaging;
using Application.Customers.Create;
using Application.Customers.Update;
using Application.Customers.Links;
using Application.Customers.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using AuthPermissions.BaseCode.CommonCode;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers;

public record CommandResponseDto(bool Success, string? Error);

public class CustomersController : VersionNeutralApiController
{
    // Tenant admin adds a customer and optionally maps to current tenant (if TenantId omitted, read from user claim)
    [HttpPost]
    [Authorize]
    [OpenApiOperation("Create a customer (auto-provisions Identity user). Optionally map to a tenant.")]
    public async Task<ActionResult> CreateAsync(
        [FromServices] ICommandHandler<CreateCustomerCommand, Guid> handler,
        [FromBody] CreateCustomerCommand command,
        CancellationToken ct)
    {
        // If TenantId not provided explicitly, try to pick from current user’s tenant claim
        if (!command.TenantId.HasValue)
        {
            var tenantId = User.GetTenantIdFromUser();
            command.TenantId = tenantId;
        }

        var result = await handler.Handle(command, ct);
        return Ok(result);

    }

    [HttpPut]
    [Authorize]
    [OpenApiOperation("Update a customer basic fields.")]
    public async Task<ActionResult> UpdateAsync(
        [FromServices] ICommandHandler<UpdateCustomerCommand> handler,
        [FromBody] UpdateCustomerCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    [HttpPost("link")]
    [Authorize]
    [OpenApiOperation("Link an existing customer to a tenant by phone number.")]
    public async Task<ActionResult> LinkAsync(
        [FromServices] ICommandHandler<LinkCustomerToTenantCommand> handler,
        [FromBody] LinkCustomerToTenantCommand command,
        CancellationToken ct)
    {
        // If TenantId not provided explicitly, try to pick from current user’s tenant claim
        if (command.TenantId == 0)
        {
            var tenantId = User.GetTenantIdFromUser();
            command.TenantId = (int)tenantId;
        }
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    [HttpPost("unlink")]
    [Authorize]
    [OpenApiOperation("Unlink an existing customer from a tenant by phone number.")]
    public async Task<ActionResult> UnlinkAsync(
        [FromServices] ICommandHandler<UnlinkCustomerFromTenantCommand> handler,
        [FromBody] UnlinkCustomerFromTenantCommand command,
        CancellationToken ct)
    {
        // If TenantId not provided explicitly, try to pick from current user’s tenant claim
        if (command.TenantId == 0)
        {
            var tenantId = User.GetTenantIdFromUser();
            command.TenantId = (int)tenantId;
        }
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    // NEW: list all customers with their tenant mappings
    [HttpGet("with-tenants")]
    [Authorize]
    [OpenApiOperation("List customers including all tenant mappings.", "")]
    public async Task<ActionResult> ListWithTenantsAsync(
        [FromServices] IQueryHandler<ListCustomersWithTenantsQuery, PagedResult<List<CustomerWithTenantsDto>>> handler,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(new ListCustomersWithTenantsQuery(pageNumber, pageSize), ct);
        return Ok(result);
    }

    // NEW: list customers for a specific tenant
    [HttpGet("tenant/{tenantId:int}")]
    [Authorize]
    [OpenApiOperation("List customers mapped to a specific tenant.", "")]
    public async Task<ActionResult> ListByTenantAsync(
        [FromServices] IQueryHandler<ListCustomersByTenantQuery, PagedResult<List<TenantCustomerDto>>> handler,
        int tenantId,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(new ListCustomersByTenantQuery(tenantId, pageNumber, pageSize), ct);
        return Ok(result);
    }

    [HttpGet("search/by-phone")]
    [Authorize]
    [OpenApiOperation("Search a customer by phone number (no tenant details).", "")]
    public async Task<ActionResult> SearchByPhoneAsync(
        [FromServices] IQueryHandler<SearchCustomerByPhoneQuery, CustomerBasicDto?> handler,
        [FromQuery] string phone,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(new SearchCustomerByPhoneQuery(phone), ct);
        return Ok(result);
    }
}
