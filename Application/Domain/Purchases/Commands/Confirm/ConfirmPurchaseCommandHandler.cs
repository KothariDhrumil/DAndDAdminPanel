using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Services.PurchasePriceCalculation;
using Application.Services.Purchases;
using Domain.Accounting;
using Domain.Enums;
using Domain.Orders;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Purchases.Commands.Confirm;

public sealed class ConfirmPurchaseCommandHandler(
    IRetailDbContext db,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider,
    IPurchaseReceivedService purchaseReceivedService,
    IPurchasePriceCalculationService  priceService)
    : ICommandHandler<ConfirmPurchaseCommand>
{
    private readonly IDateTimeProvider dateTimeProvider = dateTimeProvider;
    private readonly IPurchaseReceivedService purchaseReceivedService = purchaseReceivedService;
    private readonly IPurchasePriceCalculationService _priceService = priceService;

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
            purchase.PurchaseDate = dateTimeProvider.Now;

            await _priceService.ApplyPricingAsync(purchase);

            await db.SaveChangesAsync(ct);

            // Handle post-delivery accounting
            await purchaseReceivedService.HandlePostReceiveAsync(purchase, userContext.UserId, ct);

            // Step 5: Save and commit transaction
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result.Success();
        }
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync(ct);
            return Result.Failure(Error.Failure("Purchase.ConfirmationFailed",
                "A database error occurred while confirming the purchase. Please try again or contact support."));
        }
        catch (InvalidOperationException)
        {
            await transaction.RollbackAsync(ct);
            return Result.Failure(Error.Failure("Purchase.ConfirmationFailed",
                "An invalid operation occurred while confirming the purchase. Please try again or contact support."));
        }
    }
}
