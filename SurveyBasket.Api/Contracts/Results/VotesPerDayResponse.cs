namespace SurveyBasket.Api.Contracts.Results;

public record VotesPerDayResponse(

    DateOnly Date,
    int NumberOfVotes

    );