using Application.Abstractions.Messaging;
using Application.Customers.GetMyOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealersAndDistributors.Server.Controllers;

public class CustomerOrdersController : VersionNeutralApiController
{
    [HttpGet("me/orders")]
    [Authorize(Policy = "CustomersOnly")]
    public async Task<IResult> GetMyOrders(
        IQueryHandler<GetMyOrdersQuery, List<MyOrderDto>> handler,
        CancellationToken ct)
    {
        var cid = User.FindFirst("cid")?.Value;
        if (!Guid.TryParse(cid, out var customerId))
            return Results.Unauthorized();

        var result = await handler.Handle(new GetMyOrdersQuery(customerId), ct);
        return Results.Ok(result);
    }
}
