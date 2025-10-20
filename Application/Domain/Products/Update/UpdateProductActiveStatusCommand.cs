using Application.Abstractions.Messaging;

namespace Application.Domain.Products.Update;

public sealed class UpdateProductActiveStatusCommand : ICommand
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
}
