using Application.Abstractions.Messaging;

namespace Application.Domain.Routes.Update;

public sealed class UpdateRouteCommand : ICommand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TenantUserId { get; set; } = string.Empty;
    public bool IsActive { get; set; }    
}
