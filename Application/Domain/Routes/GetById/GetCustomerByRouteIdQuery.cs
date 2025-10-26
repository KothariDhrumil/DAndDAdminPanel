using Application.Abstractions.Messaging;

namespace Application.Domain.Routes.GetById;

public sealed record GetCustomerByRouteIdQuery(int Id) : IQuery<List<GetCustomerByRouteIdResponse>>;

public sealed class GetCustomerByRouteIdResponse
{
    public string Name { get; set; } = string.Empty;
    public Guid TenantUserId { get; set; }
    public int SequenceNo { get; set; }
}
