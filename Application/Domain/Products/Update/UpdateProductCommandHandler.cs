using Application.Abstractions.ImageHandling;
using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using SharedKernel;

namespace Application.Domain.Products.Update;

public sealed class UpdateProductCommandHandler(IUnitOfWork unitOfWork, IImageService imageService) : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await unitOfWork.Products.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
        if (product == null)
            return Result.Failure(Error.NotFound("ProductNotFound", "Product not found."));
        product.Name = command.Name;
        product.Description = command.Description;
        product.HSNCode = command.HSNCode;
        product.IGST = command.IGST;
        product.CGST = command.CGST;
        product.BasePrice = command.BasePrice;
        product.Order = (command.Order is null || command.Order == 0) ? product.Order : (int)command.Order;
        product.HindiContent = command.HindiContent;

        if (command.ImageFile != null && command.ImageFile.Length > 0)
        {
            var (imageWebPath, thumbnailWebPath) = await imageService.SaveImageAsync(command.ImageFile, ct);
            product.ImagePath = imageWebPath;
            product.ThumbnailPath = thumbnailWebPath;
        }

        unitOfWork.Products.Update(product);
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
