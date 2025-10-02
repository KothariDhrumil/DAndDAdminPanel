using AuthPermissions.BaseCode.CommonCode;
using System.ComponentModel.DataAnnotations;

namespace Domain.Customers;

/// <summary>
/// Per-tenant profile for a customer, stored in the tenant's Retail DB.
/// Supports hierarchical (franchise) structure via materialized path.
/// </summary>
public class TenantCustomerProfile : IDataKeyFilterReadOnly
{
    [Key]
    public int TenantCustomerId { get; private set; }

    /// <summary>
    /// Global customer identifier (CustomerAccount.GlobalCustomerId)
    /// </summary>
    public Guid GlobalCustomerId { get; set; }

    /// <summary>
    /// AuthP TenantId this profile belongs to
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// Optional parent global customer id (null for root)
    /// </summary>
    public Guid? ParentGlobalCustomerId { get; set; }

    /// <summary>
    /// Materialized path of hierarchy, format: |rootGuid|childGuid|...
    /// </summary>
    [MaxLength(1024)]
    public string HierarchyPath { get; set; } = string.Empty;

    /// <summary>
    /// Depth cached for quick filtering (root = 0)
    /// </summary>
    public byte Depth { get; set; }

    public string? DisplayName { get; set; }
    public string? Preferences { get; set; }
    public int LoyaltyPoints { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Multi-tenant filter key populated automatically from the current user context
    /// </summary>
    public string DataKey { get; set; } = default!;
}
