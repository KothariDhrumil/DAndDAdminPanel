using Application.Abstractions.Messaging;
using Application.Customers.GetMyOrders;
using Application.Customers.Orders;
using AuthPermissions.BaseCode.CommonCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealersAndDistributors.Server.Controllers;

public class CustomerOrdersController : VersionNeutralApiController
{
    [HttpGet]
    //[Authorize(Policy = "CustomersOnly")]
    public async Task<IResult> GetMyOrders([FromQuery] string? globalCustomerId, 
        IQueryHandler<GetMyOrdersQuery, List<MyOrderDto>> handler,
        CancellationToken ct)
    {
        Guid cid = Guid.Empty;
        int? tenantId = null;
        if (string.IsNullOrEmpty(globalCustomerId))
        {
            var cidString = User.GetGlobalCustomerId();
            if (!Guid.TryParse(cidString, out var customerId))
                return Results.Unauthorized();
            cid = customerId;
        }
        else
        {
            if (!Guid.TryParse(globalCustomerId, out var customerId))
                return Results.Unauthorized();
            cid = customerId;
            tenantId = User.GetTenantId();
        }

        var result = await handler.Handle(new GetMyOrdersQuery(cid,tenantId), ct);
        return Results.Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IResult> CreateOrder(
        ICommandHandler<CreateCustomerOrderCommand, int> handler,
        [FromBody] CreateCustomerOrderCommand command,
        CancellationToken ct)
    {

        if (!command.TenantId.HasValue)
        {
            var tenantIdClaim = User.GetTenantId();
            if (tenantIdClaim.HasValue)
                command.TenantId = tenantIdClaim.Value;
        }

        if (command.GlobalCustomerId == Guid.Empty)
        {
            var cidString = User.GetGlobalCustomerId();
            if (Guid.TryParse(cidString, out var cid))
                command.GlobalCustomerId = cid;
        }

        var result = await handler.Handle(command, ct);
        return Results.Ok(result);
    }
}
