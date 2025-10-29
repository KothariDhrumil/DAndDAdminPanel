using Domain.Enums;

namespace Application.Services.Ledger;

public interface ILedgerService : IScopedService
{
    Task AddLedgerEntryAsync(
        Guid accountId,
        AccountType accountType,
        OperationType operationType,
        LedgerType ledgerType,
        decimal amount,
        Guid performedByUserId,
        string remarks,
        int operationId,
        PaymentMode paymentMode,
        DateTime date,
        CancellationToken ct);

    Task UpdateCustomerBalanceAsync(
        Guid customerId,
        decimal latestAmount,
        CancellationToken ct);
}
