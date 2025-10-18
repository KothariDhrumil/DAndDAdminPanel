using Application.Abstractions.Messaging;

namespace Application.Domain.UserTypes.Create;

public sealed class CreateUserTypeCommand : ICommand<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
