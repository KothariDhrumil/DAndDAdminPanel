using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Domain.PriceTiers.Bulk;

public sealed record GetBulkRoutePriceTiersQuery : IQuery<List<RoutePriceTierBulkResponse>>;

public sealed class RoutePriceTierBulkResponse
{
    public int RouteId { get; set; }
    public string RouteName { get; set; } = string.Empty;
    public int PriceTierId { get; set; }
    public string PriceTierName { get; set; } = string.Empty;
}
