using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Customers;
using SharedKernel;

namespace Application.Domain.Products.Create;

public sealed class CreateProductCommandHandler(IRetailDbContext db) : ICommandHandler<CreateProductCommand, int>
{
    public async Task<Result<int>> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var entity = new Product
        {
            Name = command.Name,
            Image = command.Image,
            Description = command.Description,
            HSNCode = command.HSNCode,
            IGST = command.IGST,
            CGST = command.CGST,
            BasePrice = command.BasePrice,
            Order = command.Order,
            HindiContent = command.HindiContent,          
        };
        db.Products.Add(entity);
        await db.SaveChangesAsync(ct);
        return Result.Success(entity.Id);
    }
}
