using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Orders;

public sealed class GetPendingCustomerOrdersByRouteIdQuery : IQuery<List<PendingCustomerOrdersDto>>
{
    public int? RouteId { get; set; }
}

internal sealed class GetPendingCustomerOrdersByRouteIdQueryHandler(
    IRetailDbContext db)
    : IQueryHandler<GetPendingCustomerOrdersByRouteIdQuery, List<PendingCustomerOrdersDto>>
{
    public async Task<Result<List<PendingCustomerOrdersDto>>> Handle(GetPendingCustomerOrdersByRouteIdQuery query, CancellationToken ct)
    {

        var command = db.CustomerOrders
            .AsNoTracking()
            .OrderBy(x => x.Customer.SequenceNo).ThenBy(x => x.Customer.FirstName).ThenBy(x => x.Customer.LastName)
            .Where(o => o.IsDelivered == false);

        if (query.RouteId.HasValue)
            command = command.Where(o => o.Customer.RouteId == query.RouteId.Value);

        var items = await command
            .Select(o => new PendingCustomerOrdersDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderPlacedDate = o.OrderPlacedDate,
                Amount = o.Amount,
                GrandTotal = o.GrandTotal,
                RouteName = o.Customer.Route.Name,
                CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
            })

            .ToListAsync(ct);

        return Result.Success(items);
    }
}
