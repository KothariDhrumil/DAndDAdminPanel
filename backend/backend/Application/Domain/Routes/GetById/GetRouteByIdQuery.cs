using Application.Abstractions.Messaging;

namespace Application.Domain.Routes.GetById;

public sealed record GetRouteByIdQuery(int Id) : IQuery<RouteResponse>;

public sealed class RouteResponse
{
    public string Name { get; set; } = string.Empty;
    public Guid TenantUserId { get; set; }
    public bool IsActive { get; set; }
    public int Id { get; set; }
}
