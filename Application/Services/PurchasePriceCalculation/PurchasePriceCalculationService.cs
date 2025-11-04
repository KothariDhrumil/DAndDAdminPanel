using Application.Abstractions.Data;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.PurchasePriceCalculation;

public class PurchasePriceCalculationService : IPurchasePriceCalculationService
{
    private readonly IRetailDbContext _db;

    public PurchasePriceCalculationService(IRetailDbContext retailDbContext)
    {
        _db = retailDbContext;
    }
    public async Task<Purchase> ApplyPricingAsync(Purchase purchase)
    {
        // Example: Calculate total amount, discount, tax, grand total
        //decimal totalAmount = 0;
        //foreach (var detail in purchase.PurchaseDetails)
        //{
        //    // Calculate line item amount (Qty * Rate)
        //    detail.Amount = detail.Qty * detail.Rate;
        //    totalAmount += detail.Amount;
        //}

        //purchase.Amount = totalAmount;
        //purchase.Discount ??= 0;
        //purchase.Tax ??= 0;
        //purchase.AdditionalTax ??= 0;

        //// GrandTotal = Amount - Discount + Tax + AdditionalTax
        //purchase.GrandTotal = totalAmount - purchase.Discount.Value + purchase.Tax.Value + purchase.AdditionalTax.Value;

        //// Simulate async
        //await Task.CompletedTask;
        //return purchase;


        // 1. Load all product rates
        var productIds = purchase.PurchaseDetails.Select(x => x.ProductId).Distinct().ToList();

        var products = await _db.PurchaseUnitProducts
            .Include(x => x.Product)
            .Where(p => productIds.Contains(p.ProductId) && Equals(p.PurchaseUnitId, purchase.PurchaseUnitId))
            .ToDictionaryAsync(p => p.ProductId, p => p);

        decimal totalBaseAmount = 0;
        decimal totalTax = 0;
        decimal totalDiscount = 0;

        foreach (var detail in purchase.PurchaseDetails)
        {
            if (!products.TryGetValue(detail.ProductId, out var product))
                continue;

            var rate = product.PurchaseRate;
            var qty = detail.Qty;

            var baseAmount = qty * rate;

            // Apply discount (if order-level discount should be applied equally to each item)
            decimal lineDiscountPct = purchase.Discount ?? 0;
            decimal discountAmt = baseAmount * lineDiscountPct / 100;
            decimal taxableAmount = baseAmount - discountAmt;

            // Calculate tax on discounted amount
            decimal igst = taxableAmount * product.Product.IGST / 100;
            decimal cgst = taxableAmount * product.Product.CGST / 100;
            //decimal sgst = taxableAmount * product.Product.SGST / 100;

            detail.Rate = Math.Round(rate, 2, MidpointRounding.AwayFromZero);
            detail.Amount = Math.Round(taxableAmount, 2, MidpointRounding.AwayFromZero);
            //detail.Discount = discountAmt;
            //detail.IGST = Math.Round(igst, 2, MidpointRounding.AwayFromZero);
            //detail.CGST = Math.Round(cgst, 2, MidpointRounding.AwayFromZero);
            //detail.SGST = sgst;

            totalBaseAmount += taxableAmount;
            totalDiscount += discountAmt;
            totalTax += igst + cgst; // + sgst;
        }

        // 3. Summary
        purchase.Amount = Math.Round(totalBaseAmount, 2, MidpointRounding.AwayFromZero);
        purchase.Tax = Math.Round(totalTax, 2, MidpointRounding.AwayFromZero);

        decimal parcelCharge = purchase.ShippingCost ?? 0;
        purchase.GrandTotal = Math.Round(totalBaseAmount + totalTax + parcelCharge, 2, MidpointRounding.AwayFromZero);

        return purchase;
    }
}
