using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Products.Update;

public sealed class UpdateProductActiveStatusCommandHandler(IRetailDbContext db) : ICommandHandler<UpdateProductActiveStatusCommand>
{
    public async Task<Result> Handle(UpdateProductActiveStatusCommand command, CancellationToken ct)
    {
        var product = await db.Products.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (product == null)
            return Result.Failure(Error.NotFound("ProductNotFound", "Product not found."));
        product.IsActive = command.IsActive;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
