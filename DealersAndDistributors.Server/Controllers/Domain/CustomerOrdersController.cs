using Application.Abstractions.Messaging;
using Application.Customers.GetMyOrders;
using Application.Domain.Orders;
using Application.Domain.Orders.CreateCustomerOrder;
using Application.Domain.Orders.DeliveredCustomerOrders;
using Application.Domain.Orders.UndeliveredCustomerOrder;
using Application.Domain.Orders.UpdateCustomerOrder;
using AuthPermissions.BaseCode.CommonCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class CustomerOrdersController : VersionedApiController
{
    [HttpPost]
    [Authorize]
    public async Task<IResult> CreateOrder(
        ICommandHandler<CreateCustomerOrderCommand, int> handler,
        [FromBody] CreateCustomerOrderCommand command,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return Results.Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IResult> UpdateOrder(
        ICommandHandler<UpdateCustomerOrderCommand, bool> handler,
        int id,
        [FromBody] UpdateCustomerOrderCommand command,
        CancellationToken ct)
    {
        command.OrderId = id;
        var result = await handler.Handle(command, ct);
        return Results.Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IResult> DeleteOrder(
        ICommandHandler<DeleteCustomerOrderCommand, bool> handler,
        int id,
        [FromBody] DeleteCustomerOrderCommand command,
        CancellationToken ct)
    {
        command.OrderId = id;
        var result = await handler.Handle(command, ct);
        return Results.Ok(result);
    }

    [HttpGet("pending")]
    [Authorize]
    public async Task<IResult> GetPendingOrders(
        IQueryHandler<GetPendingCustomerOrdersByRouteIdQuery, List<CustomerOrderItemDto>> handler,
        [FromQuery] GetPendingCustomerOrdersByRouteIdQuery query,
        CancellationToken ct)
    {
        var result = await handler.Handle(query, ct);
        return Results.Ok(result);
    }

    [HttpGet("delivered")]
    [Authorize]
    public async Task<IResult> GetDeliveredOrders(
        IQueryHandler<GetDeliveredCustomerOrdersQuery, PagedResult<List<CustomerOrderItemDto>>> handler,
        [FromQuery] GetDeliveredCustomerOrdersQuery query,
        CancellationToken ct)
    {
        var result = await handler.Handle(query, ct);
        return Results.Ok(result);
    }

    [HttpGet("undelivered/{customerId}")]
    [Authorize]
    public async Task<IResult> GetUndeliveredOrdersByCustomerId(
        IQueryHandler<GetUndeliveredCustomerOrdersByCustomerIdQuery, UndeliveredCustomerOrderItemDto> handler,
        Guid customerId,        
        CancellationToken ct)
    {
        var query = new GetUndeliveredCustomerOrdersByCustomerIdQuery { CustomerId = customerId };
        var result = await handler.Handle(query, ct);
        return Results.Ok(result);
    }
}
