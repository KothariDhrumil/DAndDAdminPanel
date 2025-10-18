using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.Classes;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.SupportTickets.Update;

public sealed class SetSupportTicketStateCommand : ICommand
{
    public required int Id { get; set; }
    public required TicketStatus State { get; set; }

    internal sealed class Handler(AuthPermissionsDbContext db) : ICommandHandler<SetSupportTicketStateCommand>
    {
        private readonly AuthPermissionsDbContext _db = db;

        public async Task<Result> Handle(SetSupportTicketStateCommand command, CancellationToken ct)
        {
            var t = await _db.SupportTickets.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
            if (t is null)
                return Result.Failure(Error.NotFound("SupportTicket.NotFound", $"Ticket {command.Id} not found"));
            if (command.State == TicketStatus.Resolved)
                return Result.Failure(Error.Validation("SupportTicket.NotAllowed", "You can not allowed to mark it resolved using this"));
            
            t.TicketStatus = command.State;
            t.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

public sealed class SetSupportTicketStateCommandValidator : AbstractValidator<SetSupportTicketStateCommand>
{
    public SetSupportTicketStateCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.State).IsInEnum();
    }
}
