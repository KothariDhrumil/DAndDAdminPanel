using Application.Abstractions.Messaging;
using Application.SupportTickets.Create;
using Application.SupportTickets.Models;
using Application.SupportTickets.Queries;
using Application.SupportTickets.Update;
using AuthPermissions.BaseCode.CommonCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/support-tickets")]
public class SupportTicketsController : VersionNeutralApiController
{
    // 1. Create ticket
    [HttpPost]
    [OpenApiOperation("Create a support ticket.")]
    public async Task<ActionResult> CreateAsync(
        [FromServices] ICommandHandler<CreateSupportTicketCommand, int> handler,
        [FromBody] CreateSupportTicketCommand command,
        CancellationToken ct)
    {
        command.TenantId = User.GetTenantId();
        command.UserId = User.GetUserIdFromUser();
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    // 2. Search / list (pagination, filters, search, sorting)
    [HttpGet]
    [OpenApiOperation("Search support tickets with pagination, filter, search, sort.")]
    public async Task<ActionResult> SearchAsync(
        [FromServices] IQueryHandler<SearchSupportTicketsQuery, PagedResult<List<SupportTicketListItemDto>>> handler,
        [FromQuery] SearchSupportTicketsQuery query,
        CancellationToken ct)
    {
        var result = await handler.Handle(query, ct);
        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, result);

        // Return the PagedResult directly so paging fields are top-level
        return Ok(result.Data);
    }

    // 3. Get details
    [HttpGet("{id:int}")]
    [OpenApiOperation("Get support ticket by id.")]
    public async Task<ActionResult> GetByIdAsync(
        [FromServices] IQueryHandler<GetSupportTicketByIdQuery, SupportTicketDetailDto> handler,
        int id,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetSupportTicketByIdQuery { Id = id }, ct);
        return Ok(result);
    }

    // 4. Update ticket
    [HttpPut("{id:int}")]
    [OpenApiOperation("Update support ticket by id.")]
    public async Task<ActionResult> UpdateAsync(
        [FromServices] ICommandHandler<UpdateSupportTicketCommand> handler,
        int id,
        [FromBody] UpdateSupportTicketCommand command,
        CancellationToken ct)
    {
        command.Id = id;
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    // Convenience endpoints
    [HttpPatch("{id:int}/state")]
    [OpenApiOperation("Set support ticket state.")]
    public async Task<ActionResult> SetStateAsync(
        [FromServices] ICommandHandler<SetSupportTicketStateCommand> handler,
        int id,
        [FromBody] SetSupportTicketStateCommand command,
        CancellationToken ct)
    {
        command.Id = id;
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    [HttpPost("{id:int}/notes")]
    [OpenApiOperation("Add a note to support ticket.")]
    public async Task<ActionResult> AddNoteAsync(
        [FromServices] ICommandHandler<AddSupportTicketNoteCommand> handler,
        int id,
        [FromBody] AddSupportTicketNoteCommand command,
        CancellationToken ct)
    {
        command.Id = id;
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    [HttpPost("{id:int}/resolve")]
    [OpenApiOperation("Resolve support ticket.")]
    public async Task<ActionResult> ResolveAsync(
        [FromServices] ICommandHandler<ResolveSupportTicketCommand> handler,
        int id,
        [FromBody] ResolveSupportTicketCommand command,
        CancellationToken ct)
    {
        command.Id = id;
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }
}
