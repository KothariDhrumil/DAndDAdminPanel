using Domain.Purchase;

namespace Application.Services.Orders;

public interface IOrderDeliveryService : IScopedService
{
    Task HandlePostDeliveryAsync(CustomerOrder order, Guid performedByUserId, CancellationToken ct);
}
