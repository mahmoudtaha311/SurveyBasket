
using Hangfire;

namespace SurveyBasket.Api.Services;

public class PollService(ApplicationDbContext context, INotificationService notificationService) : IPollService
{
    private readonly ApplicationDbContext _context = context;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<Result<PollResponse>> AddAsync(PollRequest request, CancellationToken cancellation = default)
    {
        var IsExistingTitle = await _context.polls.AnyAsync(x => x.Title == request.Title, cancellation);

        if (IsExistingTitle)
            return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

        await _context.polls.AddAsync(request.Adapt<Poll>(), cancellation);
        await _context.SaveChangesAsync(cancellation);
        return Result.Success(request.Adapt<PollResponse>());
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken cancellation = default)
    {
        var poll = await _context.polls.FindAsync(id, cancellation);
        if (poll is null)
            return Result.Failure(PollErrors.pollNotFound);
        _context.polls.Remove(poll);
        await _context.SaveChangesAsync(cancellation);
        return Result.Success();



    }

    public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellation = default) =>
        await _context.polls
        .AsNoTracking()
        .ProjectToType<PollResponse>()
        .ToListAsync(cancellation);

    public async Task<IEnumerable<PollResponse>> GetCurrentAsyncV1(CancellationToken cancellation = default) =>
                      await _context.polls
                     .Where(x => x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                     .AsNoTracking()
                     .ProjectToType<PollResponse>()
                      .ToListAsync(cancellation);


    public async Task<IEnumerable<PollResponseV2>> GetCurrentAsyncV2(CancellationToken cancellation = default) =>
                      await _context.polls
                     .Where(x => x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                     .AsNoTracking()
                     .ProjectToType<PollResponseV2>()
                      .ToListAsync(cancellation);
    public async Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellation = default)
    {
        var poll = await _context.polls.FindAsync(id, cancellation);

        return poll is not null ? Result.Success(poll.Adapt<PollResponse>())
            : Result.Failure<PollResponse>(PollErrors.pollNotFound);
    }

    public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellation = default)
    {
        var poll = await _context.polls.FindAsync(id, cancellation);
        if (poll is null)
            return Result.Failure(PollErrors.pollNotFound);

        poll.IsPublished = !poll.IsPublished;

        await _context.SaveChangesAsync(cancellation);

        if (poll.IsPublished && poll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
            BackgroundJob.Enqueue(() => _notificationService.SendNewPollsNotification(poll.Id));

        return Result.Success();

    }

    public async Task<Result> UpdateAsync(int id, PollRequest request, CancellationToken cancellation = default)
    {

        var IsExistingTitle = await _context.polls.AnyAsync(x => x.Title == request.Title && x.Id != id, cancellation);

        if (IsExistingTitle)
            return Result.Failure<PollResponse>(PollErrors.DuplicatedPollTitle);

        var currentPoll = await _context.polls.FindAsync(id, cancellation);
        if (currentPoll is null)
            return Result.Failure(PollErrors.pollNotFound);

        currentPoll = request.Adapt(currentPoll);
        await _context.SaveChangesAsync();

        return Result.Success();



    }
}
