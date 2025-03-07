using SurveyBasket.Api.Contracts.Results;

namespace SurveyBasket.Api.Services;

public class ResultService(ApplicationDbContext context) : IResultService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PollVotesResponse>> GetPollVotesAsync(int pollId, CancellationToken cancellationToken = default)
    {
        var pollVotes = await _context.polls
                  .Where(x => x.Id == pollId)
                  .Select(x => new PollVotesResponse(
                      x.Title,
                      x.Votes.Select(v => new VoteResponse(
                          $"{v.User.FirstName} {v.User.LastName}",
                          v.SubmittedOn,
                          v.VoteAnswers.Select(a => new QuestionAnswerResponse(
                              a.Question.Content,
                              a.Answer.Content


                          ))
                      ))
                  ))
                  .SingleOrDefaultAsync(cancellationToken);
        return pollVotes is null
            ? Result.Failure<PollVotesResponse>(PollErrors.pollNotFound)
            : Result.Success(pollVotes);
    }
    public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesPerDayAsync(int pollId, CancellationToken cancellationToken = default)
    {

        var pollIsExists = await _context.polls.AnyAsync(x => x.Id == pollId, cancellationToken);

        if (!pollIsExists)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.pollNotFound);

        var VotesPerDay = await _context.Votes
            .Where(x => x.Id == pollId)
            .GroupBy(x => new { Date = DateOnly.FromDateTime(x.SubmittedOn) })
            .Select(g => new VotesPerDayResponse(
                g.Key.Date,
                g.Count()
                ))
            .ToListAsync(cancellationToken);

        return Result.Success<IEnumerable<VotesPerDayResponse>>(VotesPerDay);

    }

    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
    {

        var pollIsExists = await _context.polls.AnyAsync(x => x.Id == pollId, cancellationToken);

        if (!pollIsExists)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.pollNotFound);

        var votesPerQuestion = await _context.VoteAnswers
              .Where(x => x.Vote.PollId == pollId)
              .Select(x => new VotesPerQuestionResponse(
                  x.Question.Content,
                  x.Question.VoteAnswers
                            .GroupBy(x => new { Answerid = x.Answer.AnswerId, AnswerContent = x.Answer.Content })
                            .Select(g => new VotesPerAnswerResponse(
                                g.Key.AnswerContent, g.Count()


                                ))
                  )).ToListAsync(cancellationToken);




        return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);

    }
}
