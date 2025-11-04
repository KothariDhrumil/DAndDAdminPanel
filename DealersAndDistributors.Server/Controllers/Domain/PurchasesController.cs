using Application.Abstractions.Messaging;
using Application.Domain.Purchases.Commands.Create;
using Application.Domain.Purchases.Commands.Update;
using Application.Domain.Purchases.Commands.Confirm;
using Application.Domain.Purchases.Commands.Delete;
using Application.Domain.Purchases.Queries.GetAll;
using Application.Domain.Purchases.Queries.GetById;
using Application.Domain.Purchases.Queries.GetSummary;
using Application.Domain.Purchases.Queries.GetPreOrders;
using Application.Domain.Purchases.Queries.GetConfirmedPurchases;
using Application.Domain.Purchases.Queries.GetPendingRoutes;
using Application.Domain.Purchases.Queries.GetUnconfirmedOrder;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Application.Domain.Purchases.Commands.DeliverDirectPurchase;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class PurchasesController : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllPurchasesAsync(
        [FromQuery] GetAllPurchasesQuery query,
        IQueryHandler<GetAllPurchasesQuery, List<PurchaseResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPurchaseByIdAsync(
        int id,
        IQueryHandler<GetPurchaseByIdQuery, PurchaseDetailResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetPurchaseByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetPurchaseSummaryAsync(
        [FromQuery] GetPurchaseSummaryQuery query,
        IQueryHandler<GetPurchaseSummaryQuery, PurchaseSummaryResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("pre-orders")]
    public async Task<IActionResult> GetPreOrdersAsync(
        [FromQuery] GetPreOrdersQuery query,
        IQueryHandler<GetPreOrdersQuery, List<PurchaseResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("confirmed")]
    public async Task<IActionResult> GetConfirmedPurchasesAsync(
        [FromQuery] GetConfirmedPurchasesQuery query,
        IQueryHandler<GetConfirmedPurchasesQuery, List<PurchaseResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("pending-routes")]
    public async Task<IActionResult> GetPendingRoutesAsync(
        [FromQuery] GetPendingRoutesQuery query,
        IQueryHandler<GetPendingRoutesQuery, List<PendingRouteResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchaseAsync(
        [FromBody] CreatePurchaseCommand command,
        ICommandHandler<CreatePurchaseCommand, int> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePurchaseAsync(
        int id,
        [FromBody] UpdatePurchaseCommand command,
        ICommandHandler<UpdatePurchaseCommand> handler,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/confirm")]
    public async Task<IActionResult> ConfirmPurchaseAsync(
        int id,
        ICommandHandler<ConfirmPurchaseCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new ConfirmPurchaseCommand { PurchaseId = id }, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePurchaseAsync(
        int id,
        ICommandHandler<DeletePurchaseCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new DeletePurchaseCommand { Id = id }, cancellationToken);
        return Ok(result);
    }

    [HttpGet("unconfirmed")]
    public async Task<IActionResult> GetUnconfirmedOrderAsync(
        [FromQuery] int routeId,
        [FromQuery] int purchaseUnitId,
        IQueryHandler<GetUnconfirmedPurchaseQuery, UnconfirmedPurchaseResponse?> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUnconfirmedPurchaseQuery(routeId, purchaseUnitId);
        var result = await handler.Handle(query, cancellationToken);
        // result.Value is null if not found
        return Ok(result);
    }

    [HttpPost("direct-purchase")]
    
    public async Task<IResult> DeliverDirectPurchaseAsync(
        ICommandHandler<DeliverDirectPurchaseCommand, int> handler,
        [FromBody] DeliverDirectPurchaseCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return Results.Ok(result);
    }
}
