using Domain.AbstactClass;

namespace Domain.Customers;

public class Product : AuditableEntity
{
    public required string Name { get; set; }
    public string? ImagePath { get; set; }
    public string? ThumbnailPath { get; set; }
    public string? Description { get; set; }
    public string? HSNCode { get; set; }
    public decimal IGST { get; set; }
    public decimal CGST { get; set; }
    public decimal BasePrice { get; set; }
    public int Order { get; set; }
    public string? HindiContent { get; set; }
    public ICollection<CustomerProduct> CustomerProducts { get; set; } = new List<CustomerProduct>();
}
