using Application.Abstractions.Messaging;

namespace Application.Domain.Purchases.Queries.GetById;

public sealed record GetPurchaseByIdQuery(int PurchaseId) : IQuery<PurchaseDetailResponse>;
