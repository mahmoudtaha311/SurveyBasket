namespace SurveyBasket.Api.Errors;

public class voteErrors
{
    public static readonly Error IvalidQuestionId =
       new Error("vote.InvalidQuestionId", "Invalid Questions", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedVote =
      new Error("Poll.DuplicatedVote", "This user already voted before for this poll", StatusCodes.Status409Conflict);
}
