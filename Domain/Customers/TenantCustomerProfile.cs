using AuthPermissions.BaseCode.CommonCode;
using System.ComponentModel.DataAnnotations;

namespace Domain.Customers;

/// <summary>
/// Per-tenant profile for a customer, stored in the tenant's Retail DB.
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
    /// AuthP TenantId this profile belongs to (redundant with DataKey but handy for denormalized reads)
    /// </summary>
    public int TenantId { get; set; }

    public string? DisplayName { get; set; }
   
    /// <summary>
    /// Multi-tenant filter key populated automatically from the current user context
    /// </summary>
    public string DataKey { get; set; } = default!;
}
