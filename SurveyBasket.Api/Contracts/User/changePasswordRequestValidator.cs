namespace SurveyBasket.Api.Contracts.User;

public class changePasswordRequestValidator : AbstractValidator<changePasswordRequest>
{
    public changePasswordRequestValidator()
    {

        RuleFor(x => x.CurrentPassword)
            .NotEmpty();




        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegexPatterns.password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase , Nonalphanumeic and uppercase")
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("New Password cannot be same as the current Password");
    }
}
