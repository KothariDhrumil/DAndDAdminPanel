using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using SharedKernel;

namespace Application.Domain.Products.Delete;

public sealed class DeleteProductCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        var product = await unitOfWork.Products.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (product == null)
            return Result.Failure(Error.NotFound("ProductNotFound", "Product not found."));
        unitOfWork.Products.Remove(product);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
