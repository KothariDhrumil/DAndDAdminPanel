using Application.Abstractions.Messaging;

namespace Application.Domain.Todos.Delete;

public sealed record DeleteTodoCommand(Guid TodoItemId) : ICommand;
