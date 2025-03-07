namespace SurveyBasket.Api.Errors;

public class PollErrors
{
    public static readonly Error pollNotFound =
       new Error("Poll.Not.Found", "No Poll was Found With The Given Id ", StatusCodes.Status404NotFound);
    public static readonly Error DuplicatedPollTitle =
      new Error("Poll.DuplicatedPollTitle", "Another Poll With The Same title is already exists ", StatusCodes.Status409Conflict);
}
