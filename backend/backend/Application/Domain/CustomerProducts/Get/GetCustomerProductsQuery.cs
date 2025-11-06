using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;

namespace Application.Domain.CustomerProducts.Get;

public sealed record GetCustomerProductsQuery(Guid CustomerId) : IQuery<List<CustomerProductResponse>>;

public sealed class CustomerProductResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public decimal SalesRate { get; set; }
    public bool IsActive { get; set; }
}
