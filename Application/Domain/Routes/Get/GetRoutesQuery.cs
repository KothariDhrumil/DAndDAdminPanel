using Application.Abstractions.Messaging;
using Domain.Customers;

namespace Application.Domain.Routes.Get;

public sealed record GetRoutesQuery : IQuery<List<GetRouteResponse>>;

public sealed class GetRouteResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TenantUser { get; set; } = string.Empty;
    public Guid TenantUserId { get; set; }
    public bool IsActive { get; set; }

    public int? PriceTierId { get; set; }
    public string PriceTier { get; set; }
}
