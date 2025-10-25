using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PriceTiers.Update;

public sealed class UpdateCustomerPriceTierCommandHandler(IRetailDbContext db) : ICommandHandler<UpdateCustomerPriceTierCommand>
{
    public async Task<Result> Handle(UpdateCustomerPriceTierCommand command, CancellationToken ct)
    {
        var customer = await db.TenantCustomerProfiles.SingleOrDefaultAsync(x => x.TenantUserId == command.TenantUserId, ct);
        if (customer == null)
            return Result.Failure(Error.NotFound("CustomerNotFound", "Customer not found."));
        customer.PriceTierId = command.PriceTierId;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
