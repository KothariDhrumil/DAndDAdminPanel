using Domain.Purchase;

namespace Application.Services.PurchasePriceCalculation;

public interface IPurchasePriceCalculationService
{
    Task<Purchase> ApplyPricingAsync(Purchase purchase);
}
