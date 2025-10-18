using Application.Abstractions.Messaging;
using Domain.Customers;

namespace Application.Domain.UserTypes.GetById;

public sealed record GetUserTypeByIdQuery(int UserTypeId) : IQuery<GetByIdUserTypeResponseDTO>;

public sealed class GetByIdUserTypeResponseDTO
{
    public int UserTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
