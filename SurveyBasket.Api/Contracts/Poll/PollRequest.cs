namespace SurveyBasket.Api.Contracts.Poll;

public record PollRequest(
    string Title,
    string Summary,
    DateOnly StartsAt,
    DateOnly EndsAt
    );
