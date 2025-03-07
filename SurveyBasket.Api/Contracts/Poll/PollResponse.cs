namespace SurveyBasket.Api.Contracts.Poll;

public record PollResponse(
    int Id,
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
);
public record PollResponseV2(
    int Id,
    string Title,
    string Summary,
    DateOnly StartsAt,
    DateOnly EndsAt
);
