namespace SurveyBasket.Api.Contracts.Votes;

public record VoteAnswerRequest
(
    int QuestionId,
    int AnswerId
    );
