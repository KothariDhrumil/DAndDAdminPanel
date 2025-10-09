using Application.Abstractions.Messaging;
using Application.Domain.Todos.Complete;
using Application.Domain.Todos.Create;
using Application.Domain.Todos.Get;
using Application.Domain.Todos.GetById;
using Application.Domain.Todos.Update;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace DealersAndDistributors.Server.Controllers.Domain;

public class TodosController : VersionedApiController
{

    /// <summary>
    /// Gets the list of todos.
    /// </summary>
    /// <param name="handler">The query handler for getting todos.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of todo responses wrapped in an <see cref="IResult"/>.</returns>
    [HttpGet]
    public async Task<IActionResult> GetTodosAsync(IQueryHandler<GetTodosQuery, List<Application.Domain.Todos.Get.TodoResponse>> handler, CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new GetTodosQuery(), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTodoByIdAsync(Guid id,
        IQueryHandler<GetTodoByIdQuery, Application.Domain.Todos.GetById.TodoResponse> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(new GetTodoByIdQuery(id), cancellationToken);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTodoAsync([FromBody] UpdateTodoCommand command,
        ICommandHandler<UpdateTodoCommand> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(command, cancellationToken));
    }


    /// <summary>
    /// Creates a new todo item.
    /// </summary>

    [HttpPost]
    public async Task<IActionResult> CreateTodoAsync([FromBody] CreateTodoCommand command,
        ICommandHandler<CreateTodoCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(command, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> CompleteTodoAsync(Guid id,
        ICommandHandler<CompleteTodoCommand> handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.Handle(new CompleteTodoCommand(id), cancellationToken));
    }
}

