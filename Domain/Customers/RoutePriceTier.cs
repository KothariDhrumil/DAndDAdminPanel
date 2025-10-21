using Domain.AbstactClass;

namespace Domain.Customers;

public class RoutePriceTier : AuditableBaseEntity
{
    public int RouteId { get; set; }
    public Route Route { get; set; } = default!;
    public int PriceTierId { get; set; }
    public PriceTier PriceTier { get; set; } = default!;
}
