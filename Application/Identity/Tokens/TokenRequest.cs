using FluentValidation;

namespace Application.Identity.Tokens;

public record TokenRequest(string PhoneNumber, string Password, bool OtpEnabled);

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    public TokenRequestValidator()
    {
        RuleFor(p => p.PhoneNumber).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(10).WithMessage("Invalid Phone number")
            .MaximumLength(10).WithMessage("Invalid Phone number");
        
        RuleFor(p => p.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().When(x => !x.OtpEnabled);
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
