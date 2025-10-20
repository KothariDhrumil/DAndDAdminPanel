using Application.Abstractions.Data;
using Application.Abstractions.ImageHandling;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Products.Update;

public sealed class UpdateProductCommandHandler(IRetailDbContext db, IImageService imageService) : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await db.Products.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (product == null)
            return Result.Failure(Error.NotFound("ProductNotFound", "Product not found."));
        product.Name = command.Name;
        product.Description = command.Description;
        product.HSNCode = command.HSNCode;
        product.IGST = command.IGST;
        product.CGST = command.CGST;
        product.BasePrice = command.BasePrice;
        product.Order = command.Order;
        product.HindiContent = command.HindiContent;

        if (command.ImageFile != null && command.ImageFile.Length > 0)
        {
            var (imageWebPath, thumbnailWebPath) = await imageService.SaveImageAsync(command.ImageFile, ct);
            product.ImagePath = imageWebPath;
            product.ThumbnailPath = thumbnailWebPath;
        }

        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
