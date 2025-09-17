using FluentValidation;

namespace Application.Identity.Tokens;

public record TokenRequest(string? PhoneNumber, string? Password, string? Email);

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    public TokenRequestValidator()
    {
        RuleFor(p => p.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().When(x => string.IsNullOrEmpty(x.Email)).WithMessage("Phone number is required")
            .MinimumLength(10).When(x => string.IsNullOrEmpty(x.Email)).WithMessage("Invalid Phone number")
            .MaximumLength(10).When(x => string.IsNullOrEmpty(x.Email)).WithMessage("Invalid Phone number");

        RuleFor(p => p.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().When(x => string.IsNullOrEmpty(x.PhoneNumber))
            .EmailAddress().When(x => string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(p => p.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}

public record GenerateOTPRequest(string PhoneNumber);

public class GenerateOTPRequestValidator : AbstractValidator<GenerateOTPRequest>
{
    public GenerateOTPRequestValidator()
    {
        RuleFor(p => p.PhoneNumber).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(10).WithMessage("Invalid Phone number")
            .MaximumLength(10).WithMessage("Invalid Phone number");
    }
}
