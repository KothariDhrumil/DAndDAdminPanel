using Application.Abstractions.Data;
using Application.Abstractions.ImageHandling;
using Application.Abstractions.Messaging;
using Domain.Customers;
using SharedKernel;


namespace Application.Domain.Products.Create;

public sealed class CreateProductCommandHandler(IRetailDbContext db, IImageService productImageService) : ICommandHandler<CreateProductCommand, int>
{
    public async Task<Result<int>> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var entity = new Product
        {
            Name = command.Name,
            Description = command.Description,
            HSNCode = command.HSNCode,
            IGST = command.IGST,
            CGST = command.CGST,
            BasePrice = command.BasePrice,
            Order = command.Order,
            HindiContent = command.HindiContent
        };

        if (command.ImageFile != null && command.ImageFile.Length > 0)
        {
            var (imageWebPath, thumbnailWebPath) = await productImageService.SaveImageAsync(command.ImageFile, ct);
            entity.ImagePath = imageWebPath;
            entity.ThumbnailPath = thumbnailWebPath;
        }

        db.Products.Add(entity);
        await db.SaveChangesAsync(ct);
        return Result.Success(entity.Id);
    }
}
