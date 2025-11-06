using Domain.AbstactClass;
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

    public string Address { get; set; } = string.Empty;
    public double OpeningBalance { get; set; }
    public decimal OutstandingBalance { get; set; }
    public bool TaxEnabled { get; set; }
    public bool CourierChargesApplicable { get; set; }
    public string GSTNumber { get; set; } = string.Empty;
    public string GSTName { get; set; } = string.Empty;
    public double CreditLimit { get; set; }

    // Link to Route
    public int? RouteId { get; set; }
    public Route? Route { get; set; }

    // Order within route
    public int SequenceNo { get; set; }  // Determines visiting order

    public int? PriceTierId { get; set; }
    public PriceTier? PriceTier { get; set; } = default!;

    public ICollection<CustomerProduct> CustomerProducts { get; set; } = new List<CustomerProduct>();
}
