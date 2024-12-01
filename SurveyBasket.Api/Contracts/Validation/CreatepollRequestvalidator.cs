
namespace SurveyBasket.Api.Contracts.Validation;

public class CreatepollRequestvalidator : AbstractValidator<CreatePollRequest>
{
    public CreatepollRequestvalidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            //.WithMessage("please add a  {PropertyName}")
            .Length(3, 100);
            ////.WithMessage(" Title should be at least {MinLength} and Maximum {MaxLength},You Entered  {TotalLength}");

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(3, 1000);

    }

}
