using Application.Abstractions.Data;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using LedgerEntity = Domain.Accounting.Ledger;

namespace Application.Services.Ledger;

internal sealed class LedgerService : ILedgerService
{
    private readonly IRetailDbContext _db;

    public LedgerService(IRetailDbContext db)
    {
        _db = db;
    }

    public async Task AddLedgerEntryAsync(
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
        CancellationToken ct)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(ct);
        
        try
        {
            // Get the last balance for this account
            var lastLedger = await _db.Ledgers
                .Where(l => l.AccountId == accountId)
                .OrderByDescending(l => l.Date)
                .ThenByDescending(l => l.Id)
                .FirstOrDefaultAsync(ct);

            decimal previousBalance = lastLedger?.Balance ?? 0;

            // Calculate new balance
            // Debit increases the balance (customer owes money)
            // Credit decreases the balance (customer pays money)
            decimal newBalance = ledgerType == LedgerType.Debit
                ? previousBalance + amount
                : previousBalance - amount;

            // Create new ledger entry
            var ledgerEntry = new LedgerEntity
            {
                Date = date,
                AccountId = accountId,
                AccountType = accountType,
                LedgerType = ledgerType,
                OperationType = operationType,
                OperationId = operationId,
                Amount = amount,
                Balance = newBalance,
                PaymentMode = paymentMode,
                Remarks = remarks,
                PerformedByUserId = performedByUserId
            };

            _db.Ledgers.Add(ledgerEntry);
            await _db.SaveChangesAsync(ct);

            // If this is a customer account, update the customer's outstanding balance
            if (accountType == AccountType.Customer)
            {
                await UpdateCustomerBalanceAsync(accountId, newBalance, ct);
            }

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task UpdateCustomerBalanceAsync(
        Guid customerId,
        decimal latestAmount,
        CancellationToken ct)
    {
        var customer = await _db.TenantCustomerProfiles
            .FirstOrDefaultAsync(c => c.TenantUserId == customerId, ct);

        if (customer != null)
        {
            customer.OutstandingBalance = latestAmount;
            await _db.SaveChangesAsync(ct);
        }
    }
}
