using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Purchases.Commands.Delete;

public sealed class DeletePurchaseCommandHandler(IRetailDbContext db) 
    : ICommandHandler<DeletePurchaseCommand>
{
    public async Task<Result> Handle(DeletePurchaseCommand command, CancellationToken ct)
    {
        var purchase = await db.Purchases
            .FirstOrDefaultAsync(p => p.Id == command.Id, ct);

        if (purchase == null)
            return Result.Failure(Error.NotFound("Purchase.NotFound", "Purchase not found"));

        if (purchase.IsConfirmed)
            return Result.Failure(Error.Failure("Purchase.CannotDelete", 
                "Cannot delete a confirmed purchase"));

        db.Purchases.Remove(purchase);
        await db.SaveChangesAsync(ct);
        
        return Result.Success();
    }
}
