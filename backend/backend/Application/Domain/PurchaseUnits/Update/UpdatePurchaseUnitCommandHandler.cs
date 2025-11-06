using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PurchaseUnits.Update;

public sealed class UpdatePurchaseUnitCommandHandler(IRetailDbContext db) : ICommandHandler<UpdatePurchaseUnitCommand>
{
    public async Task<Result> Handle(UpdatePurchaseUnitCommand command, CancellationToken ct)
    {
        var unit = await db.PurchaseUnits.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (unit == null)
            return Result.Failure(Error.NotFound("PurchaseUnitNotFound", "Purchase unit not found."));
        unit.Name = command.Name;
        unit.IsInternal = command.IsInternal;
        unit.Address = command.Address;
        unit.IsTaxable = command.IsTaxable;
        unit.TenantUserId = command.TenantUserId;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
