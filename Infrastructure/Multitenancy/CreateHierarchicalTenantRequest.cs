
using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Multitenancy;

public class CreateHierarchicalTenantRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(AuthDbConstants.TenantFullNameSize)]
    public string TenantName { get; set; } = default!;

    public int ParentId { get; set; }

    public int TenantId { get; set; }


    public string? ShardingName { get; set; }

    public bool? HasOwnDb { get; set; }
}

