using Application.Abstractions.Messaging;

namespace Application.Domain.Todos.Get;

public sealed record GetTodosQuery() : IQuery<List<TodoResponse>>;
