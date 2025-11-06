using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using Application.Services.CustomerOrderPriceCalculation;
using Domain.Purchase;
using SharedKernel;

namespace Application.Domain.Orders.CreateCustomerOrder;

internal sealed class CreateCustomerOrderCommandHandler(
    IUnitOfWork unitOfWork, ICustomerOrderPriceCalculationService customerOrderPriceCalculationService)
    : ICommandHandler<CreateCustomerOrderCommand, int>
{
    public async Task<Result<int>> Handle(CreateCustomerOrderCommand command, CancellationToken ct)
    {
        if (command.CustomerOrderDetails == null || !command.CustomerOrderDetails.Any())
            return Result.Failure<int>(Error.Validation("OrderDetailsMissing", "Order must have at least one detail."));

        var duplicateProducts = command.CustomerOrderDetails
                                        .GroupBy(i => i.ProductId)
                                        .Where(g => g.Count() > 1)
                                        .Select(g => g.Key)
                                        .ToList();

        if (duplicateProducts.Any())
            return Result.Failure<int>(
                Error.Validation("DuplicateProducts", $"Duplicate products found: {string.Join(", ", duplicateProducts)}"));


        var order = new CustomerOrder
        {
            CustomerId = command.CustomerId,
            Discount = command.Discount,
            Remarks = command.Remarks,
            ParcelCharge = command.ParcelCharge,
            IsPreOrder = command.IsPreOrder,
            OrderPlacedDate = DateTime.UtcNow,
            CustomerOrderDetails = command.CustomerOrderDetails.Select(d => new CustomerOrderDetail
            {
                ProductId = d.ProductId,
                Qty = d.Qty, 
            }).ToList()
        };
        await customerOrderPriceCalculationService.ApplyPricingAsync(order); 

        await unitOfWork.CustomerOrders.AddAsync(order, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success(order.Id);
    }
}
