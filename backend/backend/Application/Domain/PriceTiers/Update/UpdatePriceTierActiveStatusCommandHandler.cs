using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.PriceTiers.Update;

public sealed class UpdatePriceTierActiveStatusCommandHandler(IRetailDbContext db) : ICommandHandler<UpdatePriceTierActiveStatusCommand>
{
    public async Task<Result> Handle(UpdatePriceTierActiveStatusCommand command, CancellationToken ct)
    {
        var tier = await db.PriceTiers.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (tier == null)
            return Result.Failure(Error.NotFound("PriceTierNotFound", "Price tier not found."));
        tier.IsActive = command.IsActive;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
