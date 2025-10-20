using Application.Abstractions.Messaging;

namespace Application.Domain.Products.Create;

public sealed class CreateProductCommand : ICommand<int>
{
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string HSNCode { get; set; } = string.Empty;
    public decimal IGST { get; set; }
    public decimal CGST { get; set; }
    public decimal BasePrice { get; set; }
    public int Order { get; set; }
    public string HindiContent { get; set; } = string.Empty;    
}
