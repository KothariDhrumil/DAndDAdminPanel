using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Purchase;
using SharedKernel;

namespace Application.Domain.Purchases.Commands.Create;

public sealed class CreatePurchaseCommandHandler(IRetailDbContext db) 
    : ICommandHandler<CreatePurchaseCommand, int>
{
    public async Task<Result<int>> Handle(CreatePurchaseCommand command, CancellationToken ct)
    {
        var purchase = new global::Domain.Purchase.Purchase
        {
            RouteId = command.RouteId,
            PurchaseUnitId = command.PurchaseUnitId,
            PurchaseDate = command.PurchaseDate,
            OrderPickupDate = command.OrderPickupDate,
            Amount = command.Amount,
            Discount = command.Discount,
            Tax = command.Tax,
            AdditionalTax = command.AdditionalTax,
            GrandTotal = command.GrandTotal,
            Remarks = command.Remarks,
            IsPreOrder = command.IsPreOrder,
            IsConfirmed = false, // Always starts as not confirmed
            PickupSalesmanId = command.PickupSalesmanId,
            Type = command.Type,
            PurchaseDetails = command.PurchaseDetails.Select(d => new PurchaseDetail
            {
                ProductId = d.ProductId,
                Qty = d.Qty,
                Rate = d.Rate,
                Tax = d.Tax,
                Amount = d.Amount
            }).ToList()
        };

        db.Purchases.Add(purchase);
        await db.SaveChangesAsync(ct);
        
        return Result.Success(purchase.Id);
    }
}
