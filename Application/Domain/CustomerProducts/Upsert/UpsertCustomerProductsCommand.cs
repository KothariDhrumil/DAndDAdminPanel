using Application.Abstractions.Messaging;

namespace Application.Domain.CustomerProducts.Upsert;

public sealed class UpsertCustomerProductsCommand : ICommand
{
    public Guid CustomerId { get; set; }
    public List<CustomerProductUpsertDto> Products { get; set; } = new();
}

public sealed class CustomerProductUpsertDto
{
    public int ProductId { get; set; }
    public decimal SalesRate { get; set; }
    public bool IsActive { get; set; }
}
