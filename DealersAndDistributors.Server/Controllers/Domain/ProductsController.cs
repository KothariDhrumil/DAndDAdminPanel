using Application.Abstractions.Messaging;
using Application.Domain.Products.Get;
using Application.Domain.Products.GetById;
using Application.Domain.Products.Create;
using Application.Domain.Products.Update;
using Application.Domain.Products.Delete;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class ProductsController : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetProductsAsync(
        IQueryHandler<GetProductsQuery, List<GetProductsResponse>> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new GetProductsQuery(), cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductByIdAsync(int id,
        IQueryHandler<GetProductByIdQuery, ProductResponse> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new GetProductByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(
        [FromBody] CreateProductCommand command,
        ICommandHandler<CreateProductCommand, int> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(command, cancellationToken);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProductAsync(
        [FromBody] UpdateProductCommand command,
        ICommandHandler<UpdateProductCommand> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProductAsync(int id,
        ICommandHandler<DeleteProductCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new DeleteProductCommand { Id = id }, cancellationToken);
        return Ok(result);
    }
}
