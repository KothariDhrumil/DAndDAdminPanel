using Application.Abstractions.Messaging;

namespace Application.Domain.UserTypes.Update;

public sealed class UpdateUserTypeCommand : ICommand
{
    public int UserTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
