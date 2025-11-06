using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Services.PurchasePriceCalculation;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Purchases.Commands.Update;

public sealed class UpdatePurchaseCommandHandler(IRetailDbContext db,IPurchasePriceCalculationService purchasePriceCalculationService) 
    : ICommandHandler<UpdatePurchaseCommand>
{
    private readonly IPurchasePriceCalculationService purchasePriceCalculationService = purchasePriceCalculationService;

    public async Task<Result> Handle(UpdatePurchaseCommand command, CancellationToken ct)
    {
        var purchase = await db.Purchases
            .Include(p => p.PurchaseDetails)
            .FirstOrDefaultAsync(p => p.Id == command.Id, ct);

        if (purchase == null)
            return Result.Failure(Error.NotFound("Purchase.NotFound", "Purchase not found"));

        if (purchase.IsConfirmed)
            return Result.Failure(Error.Failure("Purchase.AlreadyConfirmed", 
                "Cannot update a confirmed purchase"));
        
        await using var tx = await db.Database.BeginTransactionAsync(ct);

        purchase.RouteId = command.RouteId;
        purchase.PurchaseUnitId = command.PurchaseUnitId;
        purchase.PurchaseDate = command.PurchaseDate;
        purchase.OrderPickupDate = command.OrderPickupDate;
        purchase.Amount = command.Amount;
        purchase.Discount = command.Discount;
        purchase.Tax = command.Tax;
        purchase.AdditionalTax = command.AdditionalTax;
        purchase.GrandTotal = command.GrandTotal;
        purchase.Remarks = command.Remarks;
        purchase.PickupSalesmanId = command.PickupSalesmanId;
        purchase.ShippingCost = command.ShippingCost;
        purchase.Type = (PurchaseType)command.Type;

        // Remove existing details
        db.PurchaseDetails.RemoveRange(purchase.PurchaseDetails);

        // Add updated details
        purchase.PurchaseDetails = [.. command.PurchaseDetails.Select(d => new PurchaseDetail
        {
            PurchaseId = purchase.Id,
            ProductId = d.ProductId,
            Qty = d.Qty,
            Rate = d.Rate,
            Tax = d.Tax,
            Amount = d.Amount
        })];

        await purchasePriceCalculationService.ApplyPricingAsync(purchase);

        await db.SaveChangesAsync(ct);

        await tx.CommitAsync(ct);
        return Result.Success();
    }
}
