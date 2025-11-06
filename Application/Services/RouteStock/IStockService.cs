using Domain.Purchase;

namespace Application.Services.RouteStock;

public interface IStockService : IScopedService
{
    Task UpdateStockForDeliveryAsync(
        CustomerOrder order,
        CancellationToken ct);

    Task UpdateStockForPurchaseAsync(Purchase purchase, CancellationToken cancellationToken);

    Task AddStockAsync(int productId, int quantity, int? routeId, StockTransactionType type, int? referenceId = null);
    Task<int> GetLiveGodownStock(int productId);
    Task<int> GetLiveRouteStock(int routeId, int productId);
}
