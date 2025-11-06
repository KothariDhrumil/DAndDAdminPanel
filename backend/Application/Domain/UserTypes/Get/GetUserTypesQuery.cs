using Application.Abstractions.Messaging;
using Domain.Customers;

namespace Application.Domain.UserTypes.Get;

public sealed record GetUserTypesQuery : IQuery<List<GetUserTypeResponseDTO>>;

public sealed class GetUserTypeResponseDTO
{
    public int UserTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
