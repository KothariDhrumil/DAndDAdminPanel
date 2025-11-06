using Application.Abstractions.Messaging;
using Application.Domain.PriceTiers.Get;
using Application.Domain.PriceTiers.Create;
using Application.Domain.PriceTiers.Update;
using Application.Domain.PriceTiers.Bulk;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Application.Domain.PriceTiers.Delete;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class PriceTiersController : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetPriceTiersAsync(
        IQueryHandler<GetPriceTiersQuery, List<PriceTierResponse>> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new GetPriceTiersQuery(), cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePriceTierAsync(
        [FromBody] CreatePriceTierCommand command,
        ICommandHandler<CreatePriceTierCommand, int> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(command, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePriceTierAsync(int id,
        [FromBody] UpdatePriceTierCommand command,
        ICommandHandler<UpdatePriceTierCommand> handler,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        return Ok(await handler.Handle(command, cancellationToken));
    }

    [HttpPatch("{id:int}/active")]
    public async Task<IActionResult> UpdatePriceTierActiveStatusAsync(
        int id,
        [FromBody] UpdatePriceTierActiveStatusCommand command,
        ICommandHandler<UpdatePriceTierActiveStatusCommand> handler,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("bulk-upsert-products")]
    public async Task<IActionResult> BulkUpsertPriceTierProductsAsync(
        [FromBody] BulkUpsertPriceTierProductsCommand command,
        ICommandHandler<BulkUpsertPriceTierProductsCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("bulk-upsert-route-tiers")]
    public async Task<IActionResult> BulkUpsertRoutePriceTiersAsync(
        [FromBody] BulkUpsertRoutePriceTiersCommand command,
        ICommandHandler<BulkUpsertRoutePriceTiersCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePriceTierAsync(
        int id,
        ICommandHandler<DeletePriceTierCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new DeletePriceTierCommand { Id = id }, cancellationToken);
        return Ok(result);
    }

    [HttpGet("bulk-upsert-products")]
    public async Task<IActionResult> GetBulkPriceTierProductsAsync(
        IQueryHandler<GetBulkPriceTierProductsQuery, List<PriceTierProductBulkResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetBulkPriceTierProductsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("bulk-upsert-route-tiers")]
    public async Task<IActionResult> GetBulkRoutePriceTiersAsync(
        IQueryHandler<GetBulkRoutePriceTiersQuery, List<RoutePriceTierBulkResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetBulkRoutePriceTiersQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("route/{routeId:int}")]
    public async Task<IActionResult> UpdateRoutePriceTierAsync(
        int routeId,
        [FromBody] UpdateRoutePriceTierCommand command,
        ICommandHandler<UpdateRoutePriceTierCommand> handler,
        CancellationToken cancellationToken)
    {
        command.RouteId = routeId;
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("customer/{tenantUserId:guid}")]
    public async Task<IActionResult> UpdateCustomerPriceTierAsync(
        Guid tenantUserId,
        [FromBody] UpdateCustomerPriceTierCommand command,
        ICommandHandler<UpdateCustomerPriceTierCommand> handler,
        CancellationToken cancellationToken)
    {
        command.TenantUserId = tenantUserId;
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }
}
