namespace SurveyBasket.Api.Contracts.Results;

public record VotesPerQuestionResponse(
    string content,
    IEnumerable<VotesPerAnswerResponse> SelectedAnswers
    );
