using Domain.AbstactClass;
using Domain.Enums;

namespace Domain.Accounting;

public class Ledger : AuditableEntity
{
    public DateTime Date { get; set; }
    public Guid AccountId { get; set; }
    public AccountType AccountType { get; set; }
    public LedgerType LedgerType { get; set; }
    public OperationType OperationType { get; set; }
    public int OperationId { get; set; }
    public decimal Amount { get; set; }
    public decimal? Balance { get; set; }
    public PaymentMode PaymentMode { get; set; }
    public string Remarks { get; set; } = string.Empty;
    public Guid PerformedByUserId { get; set; }
}
