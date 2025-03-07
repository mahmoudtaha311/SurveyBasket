namespace SurveyBasket.Api.Contracts.Results;

public record VotesPerAnswerResponse(
    string Answer,
    int Count

    );