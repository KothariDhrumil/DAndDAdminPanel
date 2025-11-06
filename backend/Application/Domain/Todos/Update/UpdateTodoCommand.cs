using Application.Abstractions.Messaging;
using FluentValidation;

namespace Application.Domain.Todos.Update;

public sealed record UpdateTodoCommand(
    Guid TodoItemId,
    string Description) : ICommand;

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleFor(x => x.TodoItemId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
    }
}