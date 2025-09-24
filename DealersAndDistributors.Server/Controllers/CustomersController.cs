using Application.Abstractions.Messaging;
using Application.Customers.Create;
using Application.Customers.Update;
using Application.Customers.Links;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DealersAndDistributors.Server.Controllers;

public record CustomerResponseDto(Guid GlobalCustomerId, string PhoneNumber, string FirstName, string LastName);
public record CommandResponseDto(bool Success, string? Error);

public class CustomersController : VersionNeutralApiController
{
    // Tenant admin adds a customer and optionally maps to current tenant (if TenantId omitted, read from user claim)
    [HttpPost]
    [Authorize]
    [OpenApiOperation("Create a customer (auto-provisions Identity user). Optionally map to a tenant.")]
    public async Task<ActionResult<CommandResponseDto>> CreateAsync(
        [FromServices] ICommandHandler<CreateCustomerCommand, Guid> handler,
        [FromBody] CreateCustomerCommand command,
        CancellationToken ct)
    {
        // If TenantId not provided explicitly, try to pick from current user’s tenant claim
        if (!command.TenantId.HasValue)
        {
            var tenantIdClaim = User.FindFirst("TenantId")?.Value;
            if (int.TryParse(tenantIdClaim, out var tenantId))
                command.TenantId = tenantId;
        }

        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Ok(new CommandResponseDto(true, null))
            : BadRequest(new CommandResponseDto(false, result.Error.ToString()));
    }

    [HttpPut]
    [Authorize]
    [OpenApiOperation("Update a customer basic fields.")]
    public async Task<ActionResult<CommandResponseDto>> UpdateAsync(
        [FromServices] ICommandHandler<UpdateCustomerCommand> handler,
        [FromBody] UpdateCustomerCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Ok(new CommandResponseDto(true, null))
            : BadRequest(new CommandResponseDto(false, result.Error.ToString()));
    }

    [HttpPost("link")]
    [Authorize]
    [OpenApiOperation("Link an existing customer to a tenant by phone number.")]
    public async Task<ActionResult<CommandResponseDto>> LinkAsync(
        [FromServices] ICommandHandler<LinkCustomerToTenantCommand> handler,
        [FromBody] LinkCustomerToTenantCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Ok(new CommandResponseDto(true, null))
            : BadRequest(new CommandResponseDto(false, result.Error.ToString()));
    }   

    [HttpPost("unlink")]
    [Authorize]
    [OpenApiOperation("Unlink an existing customer from a tenant by phone number.")]
    public async Task<ActionResult<CommandResponseDto>> UnlinkAsync(
        [FromServices] ICommandHandler<UnlinkCustomerFromTenantCommand> handler,
        [FromBody] UnlinkCustomerFromTenantCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Ok(new CommandResponseDto(true, null))
            : BadRequest(new CommandResponseDto(false, result.Error.ToString()));
    }
}
