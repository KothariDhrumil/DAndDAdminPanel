using Domain.Enums;

namespace Application.Services.Ledger;

public sealed record LedgerEntryPassbookDto(
    int Id,
    DateTime Date,
    string Description,
    LedgerType LedgerType,
    decimal Amount,
    decimal PreviousBalance,
    decimal CurrentBalance,
    PaymentMode PaymentMode,
    string Remarks
);

public enum AccountTypeDTO
{
    Customer,
    TenantUser,
}