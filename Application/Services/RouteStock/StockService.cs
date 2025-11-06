using Application.Abstractions.Data;
using Domain.Purchase;

namespace Application.Services.RouteStock;

internal sealed class StockService : IStockService
{
    private readonly IRetailDbContext _db;

    public StockService(IRetailDbContext db)
    {
        _db = db;
    }

    public Task AddStockAsync(int productId, int quantity, int? routeId, StockTransactionType type, int? referenceId = null)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetLiveGodownStock(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetLiveRouteStock(int routeId, int productId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateStockForDeliveryAsync(CustomerOrder order, CancellationToken ct)
    {
        if (order.RouteId.HasValue)
        {
            // Normalize to date-only for comparison (improves query performance)
            var deliveryDate = order.OrderDeliveryDate.Date;

            var tx = order.CustomerOrderDetails.Where(x => x.Qty != 0).Select(x =>
            new StockTransaction
            {
                ProductId = x.ProductId,
                RouteId = order.RouteId, // null means Godown
                Quantity = x.Qty,
                Type = StockTransactionType.SaleFromRoute,
                ReferenceId = order.Id,
                TransactionDate = deliveryDate
            });

            await _db.StockTransactions.AddRangeAsync(tx);
        }
    }


    public async Task UpdateStockForPurchaseAsync(Purchase purchase, CancellationToken ct)
    {
        if (purchase.Type == PurchaseType.Route)
        {
            // Normalize to date-only for comparison (improves query performance)
            var deliveryDate = purchase.OrderPickupDate.Value.Date;

            // warehouse purchase
            var tx = purchase.PurchaseDetails.Where(x => x.Qty != 0).Select(x =>
            new StockTransaction
            {
                ProductId = x.ProductId,
                RouteId = purchase.RouteId, // null means Godown
                Quantity = x.Qty,
                Type = StockTransactionType.PurchaseFromWarehouse,
                ReferenceId = purchase.Id,
                TransactionDate = deliveryDate
            });
            await _db.StockTransactions.AddRangeAsync(tx);

            // add in route stock 
            var tx2 = purchase.PurchaseDetails.Where(x => x.Qty != 0).Select(x =>
            new StockTransaction
            {
                ProductId = x.ProductId,
                RouteId = purchase.RouteId, // null means Godown
                Quantity = x.Qty,
                Type = StockTransactionType.SaleToRoute,
                ReferenceId = purchase.Id,
                TransactionDate = deliveryDate
            });
            await _db.StockTransactions.AddRangeAsync(tx2);
        }

        if (purchase.Type == PurchaseType.Vendor)
        {
            // Normalize to date-only for comparison (improves query performance)
            var deliveryDate = purchase.OrderPickupDate.Value.Date;

            var tx = purchase.PurchaseDetails.Where(x => x.Qty != 0).Select(x =>
            new StockTransaction
            {
                ProductId = x.ProductId,
                RouteId = purchase.RouteId, // null means Godown
                Quantity = x.Qty,
                Type = StockTransactionType.PurchaseFromVendor,
                ReferenceId = purchase.Id,
                TransactionDate = deliveryDate
            });
            await _db.StockTransactions.AddRangeAsync(tx);

        }

    }
}
