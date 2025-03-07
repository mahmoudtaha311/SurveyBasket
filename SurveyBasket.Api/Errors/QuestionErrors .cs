namespace SurveyBasket.Api.Errors;

public class QuestionErrors
{
    public static readonly Error QuestionNotFound =
       new Error("Question.Not.Found", "No Question was Found With The Given Id ", StatusCodes.Status404NotFound);
    public static readonly Error DuplicatedQuestionContent =
      new Error("Question.DuplicatedQuestionContent", "Another Question With The Same Content is already exists", StatusCodes.Status409Conflict);
}
