using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Multitenancy;

public class UpdateHierarchicalTenantRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(AuthDbConstants.TenantFullNameSize)]
    public string TenantName { get; set; } = default!;

    public int TenantId { get; set; }
}

