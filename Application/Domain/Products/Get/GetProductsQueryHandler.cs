using Application.Abstractions.Messaging;
using Application.Common.Interfaces;
using SharedKernel;

namespace Application.Domain.Products.Get;

public sealed class GetProductsQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetProductsQuery, List<GetProductsResponse>>
{
    public async Task<Result<List<GetProductsResponse>>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        var products = await unitOfWork.Products.GetAllAsync(ct);
        
        var response = products.Select(x => new GetProductsResponse
        {
            Id = x.Id,
            Name = x.Name,
            ThumbnailPath = x.ThumbnailPath,
            Description = x.Description,
            HSNCode = x.HSNCode,
            IGST = x.IGST,
            CGST = x.CGST,
            BasePrice = x.BasePrice,
            Order = x.Order,
            HindiContent = x.HindiContent,
            IsActive = x.IsActive
        }).ToList();
        
        return Result.Success(response);
    }
}
