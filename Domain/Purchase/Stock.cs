using Domain.AbstactClass;
using Domain.Customers;

namespace Domain.Purchase;

public class Stock : AuditableEntity
{
    public int RouteId { get; set; }
    public Route Route { get; set; } = default!;
    
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
    
    public DateTime Date { get; set; }
    public int QtyPurchased { get; set; }
    public int QtySold { get; set; }
    public int Return { get; set; }
    public int Waste { get; set; }
    public int InEating { get; set; }
    public int ItemLoss { get; set; }
    public int Sample { get; set; }
    
    public int ClosingBalance => QtyPurchased - (QtySold + Return + Waste + InEating + ItemLoss + Sample);
}
