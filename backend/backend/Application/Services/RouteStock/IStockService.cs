using Domain.Purchase;

namespace Application.Services.RouteStock;

public interface IStockService : IScopedService
{
    Task UpdateStockForDeliveryAsync(
        CustomerOrder order,
        CancellationToken ct);
}
