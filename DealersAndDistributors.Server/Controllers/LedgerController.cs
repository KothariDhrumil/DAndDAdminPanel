using Application.Abstractions.Messaging;
using Application.Accounting;
using Application.Accounting.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/ledger")]
public class LedgerController : VersionNeutralApiController
{
    [HttpPost]
    [OpenApiOperation("Create a ledger entry for a tenant customer or tenant user.")]
    public async Task<ActionResult> CreateAsync(
        [FromServices] ICommandHandler<CreateLedgerEntryCommand, long> handler,
        [FromBody] CreateLedgerEntryCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    [HttpGet]
    [OpenApiOperation("List ledger entries with pagination and filters.")]
    public async Task<ActionResult> ListAsync(
        [FromServices] IQueryHandler<ListLedgerEntriesQuery, PagedResult<List<LedgerEntryDto>>> handler,
        int tenantId,
        Guid tenantUserId,
        int pageNumber = 1,
        int pageSize = 20,
        DateTime? from = null,
        DateTime? to = null,
        CancellationToken ct = default)
    {
        var result = await handler.Handle(new ListLedgerEntriesQuery(tenantId, pageNumber, pageSize, from, to, tenantUserId), ct);
        if (!result.IsSuccess)
            return StatusCode(StatusCodes.Status400BadRequest, result);
        return Ok(result.Data);
    }
}
