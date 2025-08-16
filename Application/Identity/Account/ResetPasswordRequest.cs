using Application.Identity.Tokens;
using FluentValidation;

namespace Application.Identity.Account;

public record ResetPasswordRequest(string PhoneNumber, string Code, string Password);


public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(p => p.PhoneNumber).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(10).WithMessage("Invalid Phone Number.")
            .MaximumLength(10).WithMessage("Invalid Phone Number.");

        RuleFor(p => p.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(p => p.Code)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}
