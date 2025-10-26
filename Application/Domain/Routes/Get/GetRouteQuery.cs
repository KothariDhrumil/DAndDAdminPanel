using Application.Abstractions.Messaging;

namespace Application.Domain.Routes.Get;

public sealed record GetRouteQuery : IQuery<List<GetRouteResponse>>;

public sealed class GetRouteResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}