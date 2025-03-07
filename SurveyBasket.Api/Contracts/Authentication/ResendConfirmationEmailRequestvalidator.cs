namespace SurveyBasket.Api.Contracts.Authentication;

public class ResendConfirmationEmailRequestvalidator : AbstractValidator<ResendConfirmationEmailRequest>
{
    public ResendConfirmationEmailRequestvalidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty()
           .EmailAddress();
    }
}
