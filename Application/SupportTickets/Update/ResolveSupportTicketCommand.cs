using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.SupportTickets.Update;

public sealed class ResolveSupportTicketCommand : ICommand
{
    public required int Id { get; set; }
    public required string Resolution { get; set; }

    internal sealed class Handler(AuthPermissionsDbContext db) : ICommandHandler<ResolveSupportTicketCommand>
    {
        private readonly AuthPermissionsDbContext _db = db;

        public async Task<Result> Handle(ResolveSupportTicketCommand command, CancellationToken ct)
        {
            var t = await _db.SupportTickets.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
            if (t is null)
                return Result.Failure(Error.NotFound("SupportTicket.NotFound", $"Ticket {command.Id} not found"));

            if (string.IsNullOrWhiteSpace(command.Resolution))
                return Result.Failure(Error.Validation("SupportTicket.Resolution.Required", "Resolution cannot be empty"));

            t.Resolution = command.Resolution;
            t.TicketStatus = TicketStatus.Resolved;
            t.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

public sealed class ResolveSupportTicketCommandValidator : AbstractValidator<ResolveSupportTicketCommand>
{
    public ResolveSupportTicketCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Resolution).NotEmpty().MaximumLength(4096);
    }
}
