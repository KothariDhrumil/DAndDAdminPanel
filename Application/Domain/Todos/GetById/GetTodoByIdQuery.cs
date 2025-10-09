using Application.Abstractions.Messaging;

namespace Application.Domain.Todos.GetById;

public sealed record GetTodoByIdQuery(Guid TodoItemId) : IQuery<TodoResponse>;
