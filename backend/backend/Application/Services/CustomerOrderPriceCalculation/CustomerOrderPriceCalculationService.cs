using Application.Abstractions.Data;
using Domain.Purchase;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CustomerOrderPriceCalculation;

public class CustomerOrderPriceCalculationService : ICustomerOrderPriceCalculationService
{
    private readonly IRetailDbContext _db;
    public CustomerOrderPriceCalculationService(IRetailDbContext db)
    {
        _db = db;
    }

    public async Task<CustomerOrder> ApplyPricingAsync(CustomerOrder order)
    {
        // 1. Load all product rates
        var productIds = order.CustomerOrderDetails.Select(x => x.ProductId).Distinct().ToList();

        var products = await _db.CustomerProducts
            .Include(x => x.Product)
            .Where(p => productIds.Contains(p.ProductId) && Equals(p.CustomerId, order.CustomerId))
            .ToDictionaryAsync(p => p.ProductId, p => p);

        decimal totalBaseAmount = 0;
        decimal totalTax = 0;
        decimal totalDiscount = 0;

        foreach (var detail in order.CustomerOrderDetails)
        {
            if (!products.TryGetValue(detail.ProductId, out var product))
                continue;

            var rate = product.SalesRate;
            var qty = detail.Qty;

            var baseAmount = qty * rate;

            // Apply discount (if order-level discount should be applied equally to each item)
            decimal lineDiscountPct = order.Discount ?? 0;
            decimal discountAmt = baseAmount * lineDiscountPct / 100;
            decimal taxableAmount = baseAmount - discountAmt;

            // Calculate tax on discounted amount
            decimal igst = taxableAmount * product.Product.IGST / 100;
            decimal cgst = taxableAmount * product.Product.CGST / 100;
            //decimal sgst = taxableAmount * product.Product.SGST / 100;

            detail.Rate = Math.Round(rate, 2, MidpointRounding.AwayFromZero);
            detail.Amount = Math.Round(taxableAmount, 2, MidpointRounding.AwayFromZero);
            //detail.Discount = discountAmt;
            detail.IGST = Math.Round(igst, 2, MidpointRounding.AwayFromZero);
            detail.CGST = Math.Round(cgst, 2, MidpointRounding.AwayFromZero);
            //detail.SGST = sgst;

            totalBaseAmount += taxableAmount;
            totalDiscount += discountAmt;
            totalTax += igst + cgst; // + sgst;
        }

        // 3. Summary
        order.Amount = Math.Round(totalBaseAmount, 2, MidpointRounding.AwayFromZero);
        order.Tax = Math.Round(totalTax, 2, MidpointRounding.AwayFromZero);

        decimal parcelCharge = order.ParcelCharge ?? 0;
        order.GrandTotal = Math.Round(totalBaseAmount + totalTax + parcelCharge, 2, MidpointRounding.AwayFromZero);

        return order;
    }

}
