using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Services.Orders;
using Application.Services.PurchasePriceCalculation;
using Application.Services.Purchases;
using Domain.Orders;
using Domain.Purchase;
using SharedKernel;

namespace Application.Domain.Purchases.Commands.DeliverDirectPurchase;

internal sealed class DeliverDirectPurchaseCommandHandler : ICommandHandler<DeliverDirectPurchaseCommand, int>
{
    private readonly IRetailDbContext _dbContext;
    private readonly IPurchasePriceCalculationService _priceService;
    private readonly IPurchaseReceivedService purchaseReceivedService;
    private readonly IUserContext userContext;

    public DeliverDirectPurchaseCommandHandler(IRetailDbContext dbContext,
                                               IPurchasePriceCalculationService priceService,
                                               IPurchaseReceivedService purchaseReceivedService,
                                               IUserContext userContext)
    {
        _dbContext = dbContext;
        _priceService = priceService;
        this.purchaseReceivedService = purchaseReceivedService;
        this.userContext = userContext;
    }

    public async Task<Result<int>> Handle(DeliverDirectPurchaseCommand command, CancellationToken ct)
    {
        if (command.PurchaseDetails == null || !command.PurchaseDetails.Any())
            return Result.Failure<int>(Error.Validation("DetailsMissing", "Purchase must have at least one detail."));
        await using var tx = await _dbContext.Database.BeginTransactionAsync(ct);
        var purchase = new Purchase
        {
            RouteId = command.RouteId,
            PurchaseUnitId = command.PurchaseUnitId,
            PurchaseDate = command.PurchaseDate,
            Discount = command.Discount,
            Tax = command.Tax,
            AdditionalTax = command.AdditionalTax,
            Remarks = command.Remarks,
            IsPreOrder = command.IsPreOrder,
            Type = (PurchaseType)command.Type,
            PurchaseDetails = command.PurchaseDetails.Select(d => new PurchaseDetail
            {
                ProductId = d.ProductId,
                Qty = d.Qty,
                Rate = d.Rate
            }).ToList()
        };

        await _priceService.ApplyPricingAsync(purchase);


        _dbContext.Purchases.Add(purchase);

        await _dbContext.SaveChangesAsync(ct);

        // Handle post-delivery accounting
        await purchaseReceivedService.HandlePostReceiveAsync(purchase, userContext.UserId, ct);

        // Step 5: Save and commit transaction
        await _dbContext.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return Result.Success(purchase.Id);
    }
}
