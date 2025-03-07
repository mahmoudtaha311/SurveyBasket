namespace SurveyBasket.Api.Contracts.Authentication;

public class LoginRequestValidator : AbstractValidator<loginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
