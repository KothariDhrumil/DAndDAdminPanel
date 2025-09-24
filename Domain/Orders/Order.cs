using AuthPermissions.BaseCode.CommonCode;
using System.ComponentModel.DataAnnotations;

namespace Domain.Orders;

public class Order : IDataKeyFilterReadOnly
{
    [Key]
    public int Id { get; set; }

    public DateTime OrderedAt { get; set; } = DateTime.UtcNow;

    public decimal Total { get; set; }

    // Cross-tenant customer correlation id
    public string GlobalCustomerId { get; set; }

    // Required for multi-tenant filtering
    public string DataKey { get; private set; } = default!;

    public void SetDataKey(string dataKey)
    {
        DataKey = dataKey;
    }
}
