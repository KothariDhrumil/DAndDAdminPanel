using Application.Abstractions.Data;
using Application.Services.Ledger;
using Application.Services.RouteStock;
using Domain.Enums;
using Domain.Orders;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Purchases;

public interface IPurchaseReceivedService
{
    Task HandlePostReceiveAsync(Purchase purchase, Guid performedByUserId, CancellationToken ct);
}

internal sealed class PurchaseReceivedService : IPurchaseReceivedService
{
    private readonly IRetailDbContext _db;
    private readonly ILedgerService _ledgerService;
    private readonly IStockService _stockService;

    public PurchaseReceivedService(IRetailDbContext db, ILedgerService ledgerService, IStockService stockService)
    {
        _db = db;
        _ledgerService = ledgerService;
        _stockService = stockService;
    }

    public async Task HandlePostReceiveAsync(Purchase purchase, Guid performedByUserId, CancellationToken ct)
    {
        try
        {

            // Load order details if not already loaded
            if (purchase.PurchaseUnit == null)
            {
                await _db.PurchaseUnits
                    .Where(d => d.Id== purchase.PurchaseUnitId)
                    .LoadAsync(ct);
            }

            // Update stock for each product in the purchase
            await _stockService.UpdateStockForPurchaseAsync(purchase, ct);

            // Create Ledger Entry (Supplier account)
            await _ledgerService.AddLedgerEntryAsync(
                accountId: (Guid)purchase.PurchaseUnit.TenantUserId, // Use PurchaseUnitId or SupplierId as per your model
                accountType: AccountType.Supplier,
                operationType: OperationType.PurchaseConfirmed,
                ledgerType: LedgerType.Credit,
                amount: purchase.GrandTotal,
                performedByUserId: performedByUserId,
                remarks: "Purchase received",
                operationId: purchase.Id,
                paymentMode: PaymentMode.Credit,
                date: purchase.PurchaseDate,
                ct: ct);
        }
        catch
        {
            throw;
        }
    }
}
