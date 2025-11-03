namespace Application.Domain.Purchases.Queries.GetPendingRoutes;

public class PendingRouteResponse
{
    public int RouteId { get; set; }
    public string RouteName { get; set; } = string.Empty;
    public int PendingPurchaseCount { get; set; }
    public decimal TotalPendingAmount { get; set; }
}
