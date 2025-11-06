using Application.Abstractions.Messaging;

namespace Application.Domain.Routes.Delete;

public sealed class DeleteRouteCommand : ICommand
{
    public int Id { get; set; }
}
