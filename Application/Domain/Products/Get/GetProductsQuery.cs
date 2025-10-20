using Application.Abstractions.Messaging;
using Domain.Customers;

namespace Application.Domain.Products.Get;

public sealed record GetProductsQuery : IQuery<List<GetProductsResponse>>;

public sealed class GetProductsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HSNCode { get; set; } = string.Empty;
    public decimal IGST { get; set; }
    public decimal CGST { get; set; }
    public decimal BasePrice { get; set; }
    public int Order { get; set; }
    public string HindiContent { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
