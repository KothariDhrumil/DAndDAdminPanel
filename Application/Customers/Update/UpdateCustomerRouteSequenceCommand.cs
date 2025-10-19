using Application.Abstractions.Messaging;

namespace Application.Customers.Update;

public sealed class UpdateCustomerRouteSequenceCommand : ICommand
{
    public int RouteId { get; set; }
    public List<CustomerSequenceUpdateDto> Updates { get; set; } = new();
}

public sealed class CustomerSequenceUpdateDto
{
    public Guid TenantUserId { get; set; }
    public int Sequence { get; set; }
}
