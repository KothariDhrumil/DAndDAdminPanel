using AuthPermissions.BaseCode.CommonCode;

namespace Domain.Customers;

public class Route : IDataKeyFilterReadWrite, IDataKeyFilterReadOnly
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual TenantUserProfile  TenantUser { get; set; }
    public Guid TenantUserId { get; set; }
    public bool IsActive { get; set; }

    /// <summary>
    /// Multi-tenant filter key populated automatically from the current user context
    /// </summary>
    public string DataKey { get; set; } = default!;
}
