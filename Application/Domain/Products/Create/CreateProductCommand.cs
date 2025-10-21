using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Domain.Products.Create;

public sealed class CreateProductCommand : ICommand<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? HSNCode { get; set; }
    public decimal IGST { get; set; }
    public decimal CGST { get; set; }
    public decimal BasePrice { get; set; }
    public int? Order { get; set; }
    public string? HindiContent { get; set; }
    public IFormFile? ImageFile { get; set; } // main image upload, optional
}
