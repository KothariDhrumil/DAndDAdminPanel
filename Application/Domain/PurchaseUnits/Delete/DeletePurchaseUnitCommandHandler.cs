using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PurchaseUnits.Delete;

public sealed class DeletePurchaseUnitCommandHandler(IRetailDbContext db) : ICommandHandler<DeletePurchaseUnitCommand>
{
    public async Task<Result> Handle(DeletePurchaseUnitCommand command, CancellationToken ct)
    {
        var unit = await db.PurchaseUnits.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (unit == null)
            return Result.Failure(Error.NotFound("PurchaseUnitNotFound", "Purchase unit not found."));
        db.PurchaseUnits.Remove(unit);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
