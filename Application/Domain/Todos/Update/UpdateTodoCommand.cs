using Application.Abstractions.Messaging;

namespace Application.Domain.Todos.Update;

public sealed record UpdateTodoCommand(
    Guid TodoItemId,
    string Description) : ICommand;