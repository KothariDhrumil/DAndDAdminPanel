using AuthPermissions.BaseCode.CommonCode;
using System.ComponentModel.DataAnnotations;

namespace Domain.Accounting;

/// <summary>
/// A generic ledger entry that can be associated with either a TenantCustomerProfile or a TenantUserProfile.
/// Stored in tenant Retail DB and filtered by DataKey.
/// </summary>
public class LedgerEntry : IDataKeyFilterReadOnly
{
    [Key]
    public long Id { get; set; }

    public DateTime EntryDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Positive: credit, Negative: debit
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Free text description or memo.
    /// </summary>
    [MaxLength(512)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Optional reference id (e.g., order id, salary payout id)
    /// </summary>
    public string? ReferenceId { get; set; }

    /// <summary>
    /// Link to the tenant user profile (e.g., employee), if any.
    /// </summary>
    public Guid TenantUserId { get; set; }

    /// <summary>
    /// Required for multi-tenant filtering
    /// </summary>
    public string DataKey { get; private set; } = default!;

    public void SetDataKey(string dataKey) => DataKey = dataKey;
}
