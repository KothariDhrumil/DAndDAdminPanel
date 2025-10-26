using Application.Abstractions.Messaging;

namespace Application.Domain.Routes.Get;

public sealed class GetPreviousCustomerInRouteQuery : IQuery<TenantCustomerProfileDto?>
{
    public int RouteId { get; set; }
    public Guid CustomerId { get; set; }
}
