using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PriceTiers.Delete;

public sealed class DeletePriceTierCommandHandler(IRetailDbContext db) : ICommandHandler<DeletePriceTierCommand>
{
    public async Task<Result> Handle(DeletePriceTierCommand command, CancellationToken ct)
    {
        var tier = await db.PriceTiers.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (tier == null)
            return Result.Failure(Error.NotFound("PriceTierNotFound", "Price tier not found."));
        db.PriceTiers.Remove(tier);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
