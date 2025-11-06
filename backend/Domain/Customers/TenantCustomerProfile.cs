using System.ComponentModel.DataAnnotations;

namespace Domain.Customers;

/// <summary>
/// Per-tenant profile for a customer, stored in the tenant's Retail DB.
/// Supports hierarchical (franchise) structure via materialized path.
/// </summary>
public class TenantCustomerProfile : UserProfile
{
     /// <summary>
    /// Global customer identifier (CustomerAccount.GlobalCustomerId)
    /// </summary>
    public Guid GlobalCustomerId { get; set; }  

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
}
