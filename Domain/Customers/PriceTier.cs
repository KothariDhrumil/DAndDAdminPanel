using Domain.AbstactClass;
using System.Collections.Generic;

namespace Domain.Customers;

public class PriceTier : AuditableBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<PriceTierProduct> Products { get; set; } = new List<PriceTierProduct>();
}
