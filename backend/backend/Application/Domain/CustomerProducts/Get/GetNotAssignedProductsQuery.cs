using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;

namespace Application.Domain.CustomerProducts.Get;

public sealed record GetNotAssignedProductsQuery(Guid CustomerId) : IQuery<List<NotAssignedProductResponse>>;

public sealed class NotAssignedProductResponse
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ThumbnailPath { get; set; } = string.Empty;
    public decimal SalesRate { get; set; }
}
