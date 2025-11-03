using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Accounting;
using Domain.Enums;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Purchases.Commands.Confirm;

public sealed class ConfirmPurchaseCommandHandler(
    IRetailDbContext db,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider) 
    : ICommandHandler<ConfirmPurchaseCommand>
{
    public async Task<Result> Handle(ConfirmPurchaseCommand command, CancellationToken ct)
    {
        // Use transaction for atomicity
        using var transaction = await db.Database.BeginTransactionAsync(ct);
        
        try
        {
            var purchase = await db.Purchases
                .Include(p => p.PurchaseDetails)
                .FirstOrDefaultAsync(p => p.Id == command.PurchaseId, ct);

            if (purchase == null)
                return Result.Failure(Error.NotFound("Purchase.NotFound", "Purchase not found"));

            if (purchase.IsConfirmed)
                return Result.Failure(Error.Failure("Purchase.AlreadyConfirmed", 
                    "Purchase is already confirmed"));

            // Mark as confirmed
            purchase.IsConfirmed = true;
            purchase.IsPreOrder = false;

            // Create stock transactions for route purchases
            if (purchase.RouteId.HasValue)
            {
                foreach (var detail in purchase.PurchaseDetails)
                {
                    var stock = await db.Stocks
                        .FirstOrDefaultAsync(s => 
                            s.RouteId == purchase.RouteId.Value &&
                            s.ProductId == detail.ProductId &&
                            s.Date.Date == purchase.PurchaseDate.Date, ct);

                    if (stock != null)
                    {
                        stock.QtyPurchased += detail.Qty;
                    }
                    else
                    {
                        db.Stocks.Add(new Stock
                        {
                            RouteId = purchase.RouteId.Value,
                            ProductId = detail.ProductId,
                            Date = purchase.PurchaseDate.Date,
                            QtyPurchased = detail.Qty,
                            QtySold = 0,
                            Return = 0,
                            Waste = 0,
                            InEating = 0,
                            ItemLoss = 0,
                            Sample = 0
                        });
                    }
                }
            }

            // Create ledger entry for the purchase
            if (purchase.PickupSalesmanId.HasValue)
            {
                db.Ledgers.Add(new Ledger
                {
                    Date = purchase.PurchaseDate,
                    AccountId = purchase.PickupSalesmanId.Value,
                    AccountType = AccountType.Staff,
                    LedgerType = LedgerType.Debit,
                    OperationType = OperationType.PurchaseConfirmed,
                    OperationId = purchase.Id,
                    Amount = purchase.GrandTotal,
                    PaymentMode = PaymentMode.Cash,
                    Remarks = $"Purchase confirmed - {purchase.Type}",
                    PerformedByUserId = userContext.UserId
                });
            }

            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            return Result.Failure(Error.Failure("Purchase.ConfirmationFailed", 
                $"Failed to confirm purchase: {ex.Message}"));
        }
    }
}
