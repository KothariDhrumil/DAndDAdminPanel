using AuthPermissions.BaseCode.CommonCode;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Customers;

namespace Domain.Orders;

public class Order : IDataKeyFilterReadOnly
{
    [Key]
    public int Id { get; set; }

    public DateTime OrderedAt { get; set; } = DateTime.UtcNow;

    public decimal Total { get; set; }

    // Local tenant profile reference (replaces previous GlobalCustomerId string)
    public int TenantCustomerId { get; set; }

    [ForeignKey(nameof(TenantCustomerId))]
    public TenantCustomerProfile CustomerProfile { get; set; } = default!;

    // Required for multi-tenant filtering
    public string DataKey { get; private set; } = default!;

    public void SetDataKey(string dataKey)
    {
        DataKey = dataKey;
    }
}
