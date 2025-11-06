using Application.Abstractions.Data;
using Application.Services.Ledger;
using Application.Services.RouteStock;
using Domain.Enums;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Orders;

internal sealed class OrderDeliveryService : IOrderDeliveryService
{
    private readonly IRetailDbContext _db;
    private readonly ILedgerService _ledgerService;
    private readonly IStockService _stockService;

    public OrderDeliveryService(IRetailDbContext db, ILedgerService ledgerService, IStockService stockService)
    {
        _db = db;
        _ledgerService = ledgerService;
        _stockService = stockService;
    }

    public async Task HandlePostDeliveryAsync(CustomerOrder order, Guid performedByUserId, CancellationToken ct)
    {

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
            // Update stock
            await _stockService.UpdateStockForDeliveryAsync(order, ct);

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
        }
        catch
        {
            
            throw;
        }
    }
}
