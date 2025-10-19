using AuthPermissions.BaseCode.CommonCode;
using System.ComponentModel.DataAnnotations;

namespace Domain.Customers;

public class UserType : IDataKeyFilterReadWrite, IDataKeyFilterReadOnly
{
    [Key]
    public int UserTypeId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string DataKey { get; set; } = default!;
}