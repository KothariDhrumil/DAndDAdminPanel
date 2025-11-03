using Application.Abstractions.Messaging;

namespace Application.Domain.Purchases.Queries.GetPendingRoutes;

public sealed class GetPendingRoutesQuery : IQuery<List<PendingRouteResponse>>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
