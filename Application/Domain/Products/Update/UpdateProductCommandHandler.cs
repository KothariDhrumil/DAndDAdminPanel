using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Domain.Products.Update;

public sealed class UpdateProductCommandHandler(IRetailDbContext db) : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await db.Products.SingleOrDefaultAsync(x => x.Id == command.Id, ct);
        if (product == null)
            return Result.Failure(Error.NotFound("ProductNotFound", "Product not found."));
        product.Name = command.Name;
        product.Image = command.Image;
        product.Description = command.Description;
        product.HSNCode = command.HSNCode;
        product.IGST = command.IGST;
        product.CGST = command.CGST;
        product.BasePrice = command.BasePrice;
        product.Order = command.Order;
        product.HindiContent = command.HindiContent;        
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}
