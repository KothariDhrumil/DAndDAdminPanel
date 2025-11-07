namespace Application.Domain.Purchases.Queries.GetSummary;

public class PurchaseSummaryResponse
{
    public int TotalPurchases { get; set; }
    public int ConfirmedPurchases { get; set; }
    public int PreOrders { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ConfirmedAmount { get; set; }
    public decimal PreOrderAmount { get; set; }
}
