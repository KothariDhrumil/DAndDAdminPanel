using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Services.PurchasePriceCalculation;
using Domain.Purchase;
using SharedKernel;

namespace Application.Domain.Purchases.Commands.Create;

public sealed class CreatePurchaseCommandHandler(IRetailDbContext db, IPurchasePriceCalculationService purchasePriceCalculationService, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreatePurchaseCommand, int>
{
    private readonly IPurchasePriceCalculationService purchasePriceCalculationService = purchasePriceCalculationService;
    private readonly IDateTimeProvider dateTimeProvider = dateTimeProvider;

    public async Task<Result<int>> Handle(CreatePurchaseCommand command, CancellationToken ct)
    {
        await using var tx = await db.Database.BeginTransactionAsync(ct);

        var purchase = new Purchase
        {
            RouteId = command.RouteId,
            PurchaseUnitId = command.PurchaseUnitId,
            PurchaseDate = dateTimeProvider.Now,
            Amount = command.Amount,
            Discount = command.Discount,
            Tax = command.Tax,
            AdditionalTax = command.AdditionalTax,
            GrandTotal = command.GrandTotal,
            Remarks = command.Remarks,
            IsPreOrder = command.IsPreOrder,
            IsConfirmed = false, // Always starts as not confirmed
            PickupSalesmanId = command.PickupSalesmanId,
            ShippingCost = command.ShippingCost,
            Type = (PurchaseType)command.Type,
            PurchaseDetails = command.PurchaseDetails.Select(d => new PurchaseDetail
            {
                ProductId = d.ProductId,
                Qty = d.Qty,
                Rate = d.Rate,
                Tax = d.Tax,
                Amount = d.Amount
            }).ToList()
        };
        await purchasePriceCalculationService.ApplyPricingAsync(purchase);
        db.Purchases.Add(purchase);
        await db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return Result.Success(purchase.Id);
    }
}
