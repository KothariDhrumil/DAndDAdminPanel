using Application.Abstractions.Messaging;
using Application.Customers.Create;
using Application.Customers.Update;
using Application.Customers.Links;
using Application.Customers.Queries;
using Application.Customers.Hierarchy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using AuthPermissions.BaseCode.CommonCode;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers;

public record LinkExistingChildRequest(Guid ParentGlobalCustomerId, Guid ChildGlobalCustomerId, int? TenantId);

public class CustomersController : VersionNeutralApiController
{
    // Tenant admin adds a customer and optionally maps to current tenant (if TenantId omitted, read from current user claim)
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
            var tenantId = User.GetTenantId();
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
            var tenantId = User.GetTenantId();
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
            var tenantId = User.GetTenantId();
            command.TenantId = (int)tenantId;
        }
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    // List all customers with their tenant mappings (central)
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
        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, result);
        return Ok(result.Data);
    }

    // List customers for a specific tenant (central link)
    [HttpGet("by-tenant")]
    [Authorize]
    [OpenApiOperation("List customers mapped to a specific tenant.", "")]
    public async Task<ActionResult> ListByTenantAsync(
        [FromServices] IQueryHandler<ListCustomersByTenantQuery, PagedResult<List<TenantCustomerDto>>> handler,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var tenantId = User.GetTenantId();
        if (tenantId == null)
            return BadRequest(Result.Failure(Error.Validation("TenantIdMissing", "Tenant id not provided or claim missing.")));

        var result = await handler.Handle(new ListCustomersByTenantQuery(tenantId.Value, pageNumber, pageSize), ct);
        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, result);
        return Ok(result.Data);
    }

    // NEW: List tenant-local customer profiles from RetailDbContext (per-tenant data)
    [HttpGet("tenant-profiles")]
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

    // Search by phone (central)
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

    [HttpPost("child")]
    [Authorize]
    [OpenApiOperation("Create a child customer under a parent (hierarchical franchise).", "")]
    public async Task<ActionResult> CreateChildAsync(
        [FromServices] ICommandHandler<CreateChildCustomerCommand, Guid> handler,
        [FromBody] CreateChildCustomerCommand command,
        CancellationToken ct)
    {
        if (!command.TenantId.HasValue)
        {
            var tenantId = User.GetTenantId();
            command.TenantId = tenantId;
        }

        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    [HttpPost("child/link")]
    [Authorize]
    [OpenApiOperation("Link existing customer as child under a parent (hierarchy).", "")]
    public async Task<ActionResult> LinkExistingChildAsync(
        [FromServices] ICommandHandler<SetFranchiseParentCommand> handler,
        [FromBody] LinkExistingChildRequest request,
        CancellationToken ct)
    {
        var tenantId = request.TenantId ?? User.GetTenantId();
        if (tenantId == null)
            return BadRequest(Result.Failure(Error.Validation("TenantIdMissing", "Tenant id not provided or claim missing.")));

        var cmd = new SetFranchiseParentCommand
        {
            TenantId = tenantId.Value,
            GlobalCustomerId = request.ChildGlobalCustomerId,
            NewParentGlobalCustomerId = request.ParentGlobalCustomerId
        };
        var result = await handler.Handle(cmd, ct);
        return Ok(result);
    }
}
