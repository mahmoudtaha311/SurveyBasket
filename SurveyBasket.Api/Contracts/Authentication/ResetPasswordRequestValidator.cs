namespace SurveyBasket.Api.Contracts.Authentication;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Code)
            .NotEmpty();


        RuleFor(x => x.NewPassword)
            .NotEmpty()
           .Matches(RegexPatterns.password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase , Nonalphanumeic and uppercase");

    }
}
