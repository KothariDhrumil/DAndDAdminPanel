using Application.Abstractions.Messaging;

namespace Application.Domain.Todos.Complete;

public sealed record CompleteTodoCommand(Guid TodoItemId) : ICommand;
