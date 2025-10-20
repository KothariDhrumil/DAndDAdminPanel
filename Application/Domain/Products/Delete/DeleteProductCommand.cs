using Application.Abstractions.Messaging;

namespace Application.Domain.Products.Delete;

public sealed class DeleteProductCommand : ICommand
{
    public int Id { get; set; }
}
