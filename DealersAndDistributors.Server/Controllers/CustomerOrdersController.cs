using Application.Abstractions.Messaging;
using Application.Customers.GetMyOrders;
using Application.Customers.Orders;
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
        var cid = User.FindFirst("cid")?.Value;
        if (string.IsNullOrEmpty(cid))
            return Results.Unauthorized();

        var result = await handler.Handle(new GetMyOrdersQuery(cid), ct);
        return Results.Ok(result);
    }

    [HttpPost("orders")]
    [Authorize]
    public async Task<IResult> CreateOrder(
        ICommandHandler<CreateCustomerOrderCommand, int> handler,
        [FromBody] CreateCustomerOrderCommand command,
        CancellationToken ct)
    {
        if (!command.TenantId.HasValue)
        {
            var tenantIdClaim = User.GetTenantIdFromUser();
            if (tenantIdClaim.HasValue)
                command.TenantId = tenantIdClaim.Value;
        }

        // fallback to customer id from claims if not provided
        if (string.IsNullOrWhiteSpace(command.GlobalCustomerId))
        {
            var cid = User.FindFirst("cid")?.Value;
            if (!string.IsNullOrWhiteSpace(cid))
                command.GlobalCustomerId = cid;
        }

        var result = await handler.Handle(command, ct);
        return Results.Ok(result);
    }
}
