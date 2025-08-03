using Application.Abstractions.Messaging;
using Application.Todos.Create;
using Application.Todos.Get;
using AuthPermissions.AspNetCore;
using DealersAndDistributors.Server.Extensions;
using DealersAndDistributors.Server.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Shared;
using SharedKernel;
using System.Threading;

namespace DealersAndDistributors.Server.Controllers;

public class TodosController : VersionedApiController
{

    /// <summary>
    /// Gets the list of todos.
    /// </summary>
    /// <param name="handler">The query handler for getting todos.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of todo responses wrapped in an <see cref="IResult"/>.</returns>
    [HttpGet]
    public async Task<IResult> GetTodosAsync(IQueryHandler<GetTodosQuery, List<TodoResponse>> handler, CancellationToken cancellationToken)
    {
        var query = new GetTodosQuery();

        Result<List<TodoResponse>> resonse = await handler.Handle(query, cancellationToken);

        return resonse.Match(Results.Ok, CustomResults.Problem);
    }

    /// <summary>
    /// Creates a new todo item.
    /// </summary>

    [HttpPost]
    public async Task<IResult> CreateTodoAsync([FromBody] CreateTodoCommand command,
        ICommandHandler<CreateTodoCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        Result<Guid> response = await handler.Handle(command, cancellationToken);
        return response.Match(Results.Created, CustomResults.Problem);
    }

}

