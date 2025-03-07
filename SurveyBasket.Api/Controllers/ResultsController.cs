namespace SurveyBasket.Api.Controllers;
[Route("api/Polls/{pollId}/[controller]")]
[ApiController]
[HasPermission(Permissions.Results)]
public class ResultsController(IResultService resultService) : ControllerBase
{
    private readonly IResultService _resultService = resultService;

    [HttpGet("row-data")]
    public async Task<IActionResult> PollVotes([FromRoute] int pollid, CancellationToken cancellationToken)
    {
        var result = await _resultService.GetPollVotesAsync(pollid, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("vote-per-day")]
    public async Task<IActionResult> VotesPerDay([FromRoute] int pollid, CancellationToken cancellationToken)
    {
        var result = await _resultService.GetVotesPerDayAsync(pollid, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("vote-per-Question")]
    public async Task<IActionResult> VotesPerQuestion([FromRoute] int pollid, CancellationToken cancellationToken)
    {
        var result = await _resultService.GetVotesPerQuestionAsync(pollid, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
