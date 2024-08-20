using Application.ShopSales;
using Application.ShopStock;
using AuthPermissions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace DealersAndDistributors.Server.Controllers;

public class ShopsController : VersionedApiController
{
    [HttpPost("stock")]
    [HasPermission(Example7Permissions.StockRead)]
    public Task<List<ShopStockDto>> GetStock(SearchShopStockRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("sales")]
    [HasPermission(Example7Permissions.SalesRead)]
    public Task<List<ShopSaleDto>> GetSales(SearchShopSaleRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("till")]
    [HasPermission(Example7Permissions.SalesSell)]
    public Task Till(CreateSaleItemRequest request)
    {
        return Mediator.Send(request);
    }
}

