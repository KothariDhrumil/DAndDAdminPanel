using Application.Services.Ledger;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DealersAndDistributors.Server.Controllers;

public class LedgerController : VersionNeutralApiController
{
    private readonly ILedgerService _ledgerService;

    public LedgerController(ILedgerService ledgerService)
    {
        _ledgerService = ledgerService;
    }

    [HttpGet("account/{accountId}")]
    [Authorize]
    [OpenApiOperation("Get ledger entries (passbook) for an account with running balance.")]
    public async Task<ActionResult<IReadOnlyList<LedgerEntryPassbookDto>>> GetAccountLedgerAsync(
        [FromRoute] Guid accountId,
        [FromQuery] AccountType accountType,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        CancellationToken ct = default)
    {
        var entries = await _ledgerService.GetLedgerEntriesAsync(
            accountId,
            accountType,
            from,
            to,
            ct);

        return Ok(entries);
    }

    [HttpPost("add")]
    [Authorize]
    [OpenApiOperation("Add a manual ledger entry.")]
    public async Task<ActionResult> AddLedgerEntryAsync(
        [FromBody] AddLedgerEntryRequest request,
        CancellationToken ct)
    {
        await _ledgerService.AddLedgerEntryAsync(
            request.AccountId,
            request.AccountType,
            request.OperationType,
            request.LedgerType,
            request.Amount,
            request.PerformedByUserId,
            request.Remarks ?? string.Empty,
            request.OperationId,
            request.PaymentMode,
            request.Date ?? DateTime.UtcNow,
            ct);

        return Ok();
    }
}

public record AddLedgerEntryRequest(
    Guid AccountId,
    AccountType AccountType,
    OperationType OperationType,
    LedgerType LedgerType,
    decimal Amount,
    Guid PerformedByUserId,
    string? Remarks,
    int OperationId,
    PaymentMode PaymentMode,
    DateTime? Date
);
