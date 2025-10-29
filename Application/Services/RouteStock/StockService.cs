using Application.Abstractions.Data;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.RouteStock;

internal sealed class StockService : IStockService
{
    private readonly IRetailDbContext _db;

    public StockService(IRetailDbContext db)
    {
        _db = db;
    }

    public async Task UpdateStockForDeliveryAsync(CustomerOrder order, CancellationToken ct)
    {
        if (order.RouteId.HasValue)
        {
            // Normalize to date-only for comparison (improves query performance)
            var deliveryDate = order.OrderDeliveryDate.Date;

            foreach (var detail in order.CustomerOrderDetails)
            {
                var stock = await _db.Stocks
                    .FirstOrDefaultAsync(s =>
                        s.RouteId == order.RouteId.Value &&
                        s.ProductId == detail.ProductId &&
                        s.Date == deliveryDate,
                        ct);

                if (stock == null)
                {
                    // Create new stock entry
                    stock = new Stock
                    {
                        RouteId = order.RouteId.Value,
                        ProductId = detail.ProductId,
                        Date = deliveryDate,
                        QtyPurchased = 0,
                        QtySold = detail.Qty,
                        Return = 0,
                        Waste = 0,
                        InEating = 0,
                        ItemLoss = 0,
                        Sample = 0
                    };
                    _db.Stocks.Add(stock);
                }
                else
                {
                    // Update existing stock
                    stock.QtySold += detail.Qty;
                }
            }
        }
    }
}
