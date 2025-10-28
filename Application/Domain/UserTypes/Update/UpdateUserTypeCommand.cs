using Application.Abstractions.Messaging;
using FluentValidation;

namespace Application.Domain.UserTypes.Update;

public sealed class UpdateUserTypeCommand : ICommand
{
    public int UserTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateUserTypeCommandValidator : AbstractValidator<UpdateUserTypeCommand>
{
    public UpdateUserTypeCommandValidator()
    {
        RuleFor(x => x.UserTypeId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
