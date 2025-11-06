using Domain.Purchase;

namespace Application.Services.CustomerOrderPriceCalculation;

public interface ICustomerOrderPriceCalculationService
{
    Task<CustomerOrder> ApplyPricingAsync(CustomerOrder customerOrder);
}
