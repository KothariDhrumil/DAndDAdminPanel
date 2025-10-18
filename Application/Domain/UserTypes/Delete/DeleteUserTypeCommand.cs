using Application.Abstractions.Messaging;

namespace Application.Domain.UserTypes.Delete;

public sealed class DeleteUserTypeCommand : ICommand
{
    public int UserTypeId { get; set; }
}
