using Application.Abstractions.Messaging;
using Application.Domain.PurchaseUnitProducts.Get;
using Application.Domain.PurchaseUnitProducts.Upsert;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class PurchaseUnitProductsController : VersionedApiController
{
    [HttpGet("{purchaseUnitId:int}")]
    public async Task<IActionResult> GetPurchaseUnitProductsAsync(
        int purchaseUnitId,
        IQueryHandler<GetPurchaseUnitProductsQuery, List<PurchaseUnitProductResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetPurchaseUnitProductsQuery(purchaseUnitId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{purchaseUnitId:int}/upsert")]
    public async Task<IActionResult> UpsertPurchaseUnitProductsAsync(
        int purchaseUnitId,
        [FromBody] UpsertPurchaseUnitProductsCommand command,
        ICommandHandler<UpsertPurchaseUnitProductsCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{purchaseUnitId:int}/not-assigned")]
    public async Task<IActionResult> GetNotAssignedPurchaseUnitProductsAsync(
        int purchaseUnitId,
        IQueryHandler<GetNotAssignedPurchaseUnitProductsQuery, List<NotAssignedPurchaseUnitProductResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetNotAssignedPurchaseUnitProductsQuery(purchaseUnitId), cancellationToken);
        return Ok(result);
    }
}
