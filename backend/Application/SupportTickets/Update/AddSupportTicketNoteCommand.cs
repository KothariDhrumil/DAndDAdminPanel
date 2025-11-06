using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.SupportTickets.Update;

public sealed class AddSupportTicketNoteCommand : ICommand
{
    public required int Id { get; set; }
    public required string Note { get; set; }

    internal sealed class Handler(AuthPermissionsDbContext db) : ICommandHandler<AddSupportTicketNoteCommand>
    {
        private readonly AuthPermissionsDbContext _db = db;

        public async Task<Result> Handle(AddSupportTicketNoteCommand command, CancellationToken ct)
        {
            var t = await _db.SupportTickets.FirstOrDefaultAsync(x => x.Id == command.Id, ct);
            if (t is null)
                return Result.Failure(Error.NotFound("SupportTicket.NotFound", $"Ticket {command.Id} not found"));

            if (string.IsNullOrWhiteSpace(command.Note))
                return Result.Failure(Error.Validation("SupportTicket.Note.Required", "Note cannot be empty"));

            var now = DateTime.UtcNow.ToString("u");
            t.Notes = string.IsNullOrEmpty(t.Notes)
                ? $"[{now}] {command.Note}"
                : $"{t.Notes}\n---\n[{now}] {command.Note}";
            t.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}

public class AddSupportTicketNoteCommandValidator : AbstractValidator<AddSupportTicketNoteCommand>
{
    public AddSupportTicketNoteCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Note).NotEmpty().MaximumLength(4096);
    }
}
