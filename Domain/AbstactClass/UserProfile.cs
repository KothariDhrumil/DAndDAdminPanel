using AuthPermissions.BaseCode.CommonCode;
using System.ComponentModel.DataAnnotations;

namespace Domain.AbstactClass;

public abstract class UserProfile : IDataKeyFilterReadWrite, IDataKeyFilterReadOnly
{
    [Key]
    public Guid TenantUserId { get; private set; }

    [Required, MaxLength(100)]
    public string? FirstName { get; set; }

    [Required, MaxLength(100)]
    public string? LastName { get; set; }

    [Required, MaxLength(32)]
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Multi-tenant filter key populated automatically from the current user context
    /// </summary>
    public string DataKey { get; set; } = default!;
    /// <summary>
    /// AuthP TenantId this profile belongs to
    /// </summary>
    public int TenantId { get; set; }

    public bool IsActive { get; set; } = true;
}