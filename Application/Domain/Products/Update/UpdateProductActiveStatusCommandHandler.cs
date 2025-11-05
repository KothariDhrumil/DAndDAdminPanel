using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using SharedKernel;

namespace Application.Domain.Products.Update;

public sealed class UpdateProductActiveStatusCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateProductActiveStatusCommand>
{
    public async Task<Result> Handle(UpdateProductActiveStatusCommand command, CancellationToken ct)
    {
        var product = await unitOfWork.Products.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (product == null)
            return Result.Failure(Error.NotFound("ProductNotFound", "Product not found."));
        product.IsActive = command.IsActive;
        unitOfWork.Products.Update(product);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
