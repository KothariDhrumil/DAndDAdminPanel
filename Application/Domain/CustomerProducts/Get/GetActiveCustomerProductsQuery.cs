using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;

namespace Application.Domain.CustomerProducts.Get;

public sealed record GetActiveCustomerProductsQuery(Guid CustomerId) : IQuery<List<ActiveCustomerProductResponse>>;

public sealed class ActiveCustomerProductResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal SalesRate { get; set; }
}
