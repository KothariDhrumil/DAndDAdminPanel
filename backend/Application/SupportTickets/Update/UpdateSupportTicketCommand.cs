using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.SupportTickets.Update;

public sealed class UpdateSupportTicketCommand : ICommand
{
    public required int Id { get; set; }
    public string? Notes { get; set; }
    public TicketStatus? TicketStatus { get; set; }
    public TicketPriority? Priority { get; set; }
    public string? Resolution { get; set; }

    internal sealed class Handler(AuthPermissionsDbContext db) : ICommandHandler<UpdateSupportTicketCommand>
    {
        private readonly AuthPermissionsDbContext _db = db;

        public async Task<Result> Handle(UpdateSupportTicketCommand command, CancellationToken ct)
        {
            var t = await _db.SupportTickets.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
            if (t is null)
                return Result.Failure(Error.NotFound("SupportTicket.NotFound", $"Ticket {command.Id} not found"));

            if (command.Notes is not null) t.Notes = command.Notes;
            if (command.TicketStatus is not null) t.TicketStatus = command.TicketStatus.Value;
            if (command.Priority is not null) t.Priority = command.Priority.Value;
            if (command.Resolution is not null) t.Resolution = command.Resolution;

            t.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}

public sealed class UpdateSupportTicketCommandValidator : AbstractValidator<UpdateSupportTicketCommand>
{
    public UpdateSupportTicketCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Notes).MaximumLength(4096).When(x => x.Notes != null);
        RuleFor(x => x.Resolution).MaximumLength(4096).When(x => x.Resolution != null);
    }
}
