namespace SurveyBasket.Api.Contracts.Poll;

public class pollRequestValidator : AbstractValidator<PollRequest>
{
    public pollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Summary)
            .NotEmpty()
            .Length(3, 1500);

        RuleFor(x => x.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.EndsAt)
            .NotEmpty();

        RuleFor(x => x)
            .Must(HasValiedDate)
            .WithName(nameof(PollRequest.EndsAt))
            .WithMessage("{PropertyName} should be Greater Than Or Equel start Date");


    }

    private bool HasValiedDate(PollRequest value)
    {
        return value.EndsAt >= value.StartsAt;
    }



}

