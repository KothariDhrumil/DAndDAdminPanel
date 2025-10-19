using Application.Abstractions.Messaging;

namespace Application.Domain.Routes.Create;

public sealed class CreateRouteCommand : ICommand<string>
{
    public string Name { get; set; } = string.Empty;
    public string TenantUserId { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
