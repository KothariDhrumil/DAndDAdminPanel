
using AuthPermissions.BaseCode.DataLayer.Classes.SupportTypes;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Multitenancy;

public class MoveHierarchicalTenantRequest
{
    public int TenantId { get; set; }

    public int ParentId { get; set; }
}

