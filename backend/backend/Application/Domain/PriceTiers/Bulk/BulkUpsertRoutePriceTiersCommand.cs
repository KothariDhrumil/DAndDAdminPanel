using Application.Abstractions.Messaging;
using System.Collections.Generic;

namespace Application.Domain.PriceTiers.Bulk;

public sealed class BulkUpsertRoutePriceTiersCommand : ICommand
{
    public List<RoutePriceTierUpsertDto> RouteTiers { get; set; } = new();
}

public sealed class RoutePriceTierUpsertDto
{
    public int RouteId { get; set; }
    public int PriceTierId { get; set; }
}
