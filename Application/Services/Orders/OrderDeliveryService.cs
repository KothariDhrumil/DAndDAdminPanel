using Application.Abstractions.Data;
using Application.Services.Ledger;
using Domain.Enums;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Orders;

internal sealed class OrderDeliveryService : IOrderDeliveryService
{
    private readonly IRetailDbContext _db;
    private readonly ILedgerService _ledgerService;

    public OrderDeliveryService(IRetailDbContext db, ILedgerService ledgerService)
    {
        _db = db;
        _ledgerService = ledgerService;
    }

    public async Task HandlePostDeliveryAsync(CustomerOrder order, Guid performedByUserId, CancellationToken ct)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(ct);

        try
        {
            // Load order details if not already loaded
            if (!order.CustomerOrderDetails.Any())
            {
                await _db.CustomerOrderDetails
                    .Where(d => d.CustomerOrderId == order.Id)
                    .LoadAsync(ct);
            }

            // Update Stock for each product in the order
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

                await _db.SaveChangesAsync(ct);
            }

            // Create Ledger Entry
            await _ledgerService.AddLedgerEntryAsync(
                accountId: order.CustomerId,
                accountType: AccountType.Customer,
                operationType: OperationType.OrderPlaced,
                ledgerType: LedgerType.Debit,
                amount: order.GrandTotal,
                performedByUserId: performedByUserId,
                remarks: "Order delivered",
                operationId: order.Id,
                paymentMode: PaymentMode.Credit,
                date: order.OrderDeliveryDate,
                ct: ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}
