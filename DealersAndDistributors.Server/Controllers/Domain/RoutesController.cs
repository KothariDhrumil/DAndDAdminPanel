using Application.Abstractions.Messaging;
using Application.Domain.Routes.Get;
using Application.Domain.Routes.GetById;
using Application.Domain.Routes.Create;
using Application.Domain.Routes.Update;
using Application.Domain.Routes.Delete;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class RoutesController : VersionedApiController
{
    [HttpGet("with-salesman")]
    public async Task<IActionResult> GetRoutesWithSalesManAsync(
        IQueryHandler<GetRouteWithSalesManQuery, List<GetRouteWithSalesManResponse>> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new GetRouteWithSalesManQuery(), cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetRoutesAsync(
        IQueryHandler<GetRouteQuery, List<GetRouteResponse>> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new GetRouteQuery(), cancellationToken));
    }

    [HttpGet("{id:int}/customers")]
    public async Task<IActionResult> GetCustomerByRouteIdAsync(
        int id,
        IQueryHandler<GetCustomerByRouteIdQuery, List<GetCustomerByRouteIdResponse>> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new GetCustomerByRouteIdQuery(id), cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRouteByIdAsync(int id,
        IQueryHandler<GetRouteByIdQuery, RouteResponse> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new GetRouteByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRouteAsync(
        [FromBody] CreateRouteCommand command,
        ICommandHandler<CreateRouteCommand, string> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(command, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRouteAsync(int id,
        [FromBody] UpdateRouteCommand command,
        ICommandHandler<UpdateRouteCommand> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRouteAsync(int id,
        ICommandHandler<DeleteRouteCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new DeleteRouteCommand { Id = id }, cancellationToken);
        return Ok(result);
    }
}
