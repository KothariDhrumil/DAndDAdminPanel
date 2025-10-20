using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Products.Delete;

public sealed class DeleteProductCommandHandler(IRetailDbContext db) : ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        var product = await db.Products.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (product == null)
            return Result.Failure(Error.NotFound("ProductNotFound", "Product not found."));
        db.Products.Remove(product);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
