using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PurchaseUnits.Update;

public sealed class UpdatePurchaseUnitActiveStatusCommandHandler(IRetailDbContext db) : ICommandHandler<UpdatePurchaseUnitActiveStatusCommand>
{
    public async Task<Result> Handle(UpdatePurchaseUnitActiveStatusCommand command, CancellationToken ct)
    {
        var unit = await db.PurchaseUnits.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (unit == null)
            return Result.Failure(Error.NotFound("PurchaseUnitNotFound", "Purchase unit not found."));
        unit.IsActive = command.IsActive;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
