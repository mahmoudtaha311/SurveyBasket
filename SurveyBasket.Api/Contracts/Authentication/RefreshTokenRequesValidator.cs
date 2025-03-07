namespace SurveyBasket.Api.Contracts.Authentication;

public class RefreshTokenRequesValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequesValidator()
    {
        RuleFor(x => x.token)
            .NotEmpty();
        RuleFor(x => x.refreshToken)
            .NotEmpty();
    }
}

