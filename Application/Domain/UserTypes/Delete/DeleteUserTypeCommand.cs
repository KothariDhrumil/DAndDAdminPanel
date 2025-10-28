using Application.Abstractions.Messaging;
using FluentValidation;

namespace Application.Domain.UserTypes.Delete;

public sealed class DeleteUserTypeCommand : ICommand
{
    public int UserTypeId { get; set; }
}

public class DeleteUserTypeCommandValidator : AbstractValidator<DeleteUserTypeCommand>
{
    public DeleteUserTypeCommandValidator()
    {
        RuleFor(x => x.UserTypeId).GreaterThan(0);
    }
}
