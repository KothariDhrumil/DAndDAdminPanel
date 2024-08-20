using Application.ShopSales;
using Application.ShopStock;
using AuthPermissions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace DealersAndDistributors.Server.Controllers;

public class ShopsController : VersionedApiController
{
    [HttpPost("stock")]
    [HasPermission(Permissions.StockRead)]
    public Task<List<ShopStockDto>> GetStock(SearchShopStockRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("sales")]
    [HasPermission(Permissions.SalesRead)]
    public Task<List<ShopSaleDto>> GetSales(SearchShopSaleRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("till")]
    [HasPermission(Permissions.SalesSell)]
    public Task Till(CreateSaleItemRequest request)
    {
        return Mediator.Send(request);
    }
}

