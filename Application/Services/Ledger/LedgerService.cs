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

            // If this is a customer account, update the customer's outstanding balance
            if (accountType == AccountType.Customer)
            {
                await UpdateCustomerBalanceAsync(accountId, newBalance, ct);
            }
        }
        catch
        {
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

        }
    }

    public async Task<IReadOnlyList<LedgerEntryPassbookDto>> GetLedgerEntriesAsync(
        Guid accountId,
        AccountType accountType,
        DateTime? from,
        DateTime? to,
        CancellationToken ct)
    {
        // Get opening balance (last entry before 'from' date if filtering by date)
        decimal openingBalance = 0;
        if (from.HasValue)
        {
            var lastEntryBeforeFrom = await _db.Ledgers
                .Where(l => l.AccountId == accountId && l.AccountType == accountType && l.Date < from.Value)
                .OrderByDescending(l => l.Date)
                .ThenByDescending(l => l.Id)
                .FirstOrDefaultAsync(ct);

            openingBalance = lastEntryBeforeFrom?.Balance ?? 0;
        }

        var query = _db.Ledgers
            .Where(l => l.AccountId == accountId && l.AccountType == accountType);

        if (from.HasValue)
        {
            query = query.Where(l => l.Date >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(l => l.Date <= to.Value);
        }

        var ledgerEntries = await query
            .OrderBy(l => l.Date)
            .ThenBy(l => l.Id)
            .Select(l => new
            {
                l.Id,
                l.Date,
                l.OperationType,
                l.LedgerType,
                l.Amount,
                l.Balance,
                l.PaymentMode,
                l.Remarks
            })
            .ToListAsync(ct);

        var passbookEntries = new List<LedgerEntryPassbookDto>();
        decimal runningBalance = openingBalance;

        foreach (var entry in ledgerEntries)
        {
            decimal previousBalance = runningBalance;
            decimal currentBalance = entry.Balance ?? runningBalance;
            
            // Update running balance for next iteration
            runningBalance = currentBalance;

            var description = GetOperationDescription(entry.OperationType);

            passbookEntries.Add(new LedgerEntryPassbookDto(
                entry.Id,
                entry.Date,
                description,
                entry.LedgerType,
                entry.Amount,
                previousBalance,
                currentBalance,
                entry.PaymentMode,
                entry.Remarks
            ));
        }

        return passbookEntries;
    }

    private static string GetOperationDescription(OperationType operationType)
    {
        return operationType switch
        {
            OperationType.OrderPlaced => "Order Placed",
            OperationType.PaymentReceived => "Payment Received",
            OperationType.Refund => "Refund",
            OperationType.RouteExpense => "Route Expense",
            OperationType.SalaryCredit => "Salary Credit",
            OperationType.OpeningBalance => "Opening Balance",
            _ => operationType.ToString()
        };
    }
}
