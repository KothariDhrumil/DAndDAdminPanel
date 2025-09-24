using Application.Abstractions.Messaging;
using Application.Customers.GetMyOrders;
using AuthPermissions.BaseCode.CommonCode;
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
        var customerId = User.GetUserIdFromUser();

        var result = await handler.Handle(new GetMyOrdersQuery(customerId), ct);
        return Results.Ok(result);
    }
}
