using Application.Abstractions.Messaging;
using Application.Domain.CustomerProducts.Get;
using Application.Domain.CustomerProducts.Upsert;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class CustomerProductsController : VersionedApiController
{
    [HttpGet("{customerId:guid}")]
    public async Task<IActionResult> GetCustomerProductsAsync(
        Guid customerId,
        IQueryHandler<GetCustomerProductsQuery, List<CustomerProductResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetCustomerProductsQuery(customerId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{customerId:guid}/not-assigned")]
    public async Task<IActionResult> GetNotAssignedProductsAsync(
        Guid customerId,
        IQueryHandler<GetNotAssignedProductsQuery, List<NotAssignedProductResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetNotAssignedProductsQuery(customerId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{customerId:guid}/upsert")]
    public async Task<IActionResult> UpsertCustomerProductsAsync(
        Guid customerId,
        [FromBody] UpsertCustomerProductsCommand command,
        ICommandHandler<UpsertCustomerProductsCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }
}
