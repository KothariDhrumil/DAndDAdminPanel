using Application.Abstractions.Messaging;
using Application.Domain.PurchaseUnits.Get;
using Application.Domain.PurchaseUnits.GetById;
using Application.Domain.PurchaseUnits.Create;
using Application.Domain.PurchaseUnits.Update;
using Application.Domain.PurchaseUnits.Delete;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class PurchaseUnitsController : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetPurchaseUnitsAsync(
        IQueryHandler<GetPurchaseUnitsQuery, List<GetPurchaseUnitResponse>> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new GetPurchaseUnitsQuery(), cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPurchaseUnitByIdAsync(int id,
        IQueryHandler<GetPurchaseUnitByIdQuery, PurchaseUnitResponse> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new GetPurchaseUnitByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePurchaseUnitAsync(
        [FromBody] CreatePurchaseUnitCommand command,
        ICommandHandler<CreatePurchaseUnitCommand, int> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(command, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePurchaseUnitAsync(int id,
        [FromBody] UpdatePurchaseUnitCommand command,
        ICommandHandler<UpdatePurchaseUnitCommand> handler,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        return Ok(await handler.Handle(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePurchaseUnitAsync(int id,
        ICommandHandler<DeletePurchaseUnitCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new DeletePurchaseUnitCommand { Id = id }, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("{id:int}/active")]
    public async Task<IActionResult> UpdatePurchaseUnitActiveStatusAsync(
        int id,
        [FromBody] UpdateActiveStatusDto updateActiveStatusDto,
        ICommandHandler<UpdatePurchaseUnitActiveStatusCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePurchaseUnitActiveStatusCommand { Id = id, IsActive = updateActiveStatusDto.IsActive };
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }
    public class UpdateActiveStatusDto
    {
        public bool IsActive { get; set; }
    }
}
