namespace SurveyBasket.Api;

public interface IPollService
{
    Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellation = default);
    Task<IEnumerable<PollResponse>> GetCurrentAsyncV1(CancellationToken cancellation = default);
    Task<IEnumerable<PollResponseV2>> GetCurrentAsyncV2(CancellationToken cancellation = default);
    Task<Result<PollResponse>> GetByIdAsync(int id, CancellationToken cancellation = default);
    Task<Result<PollResponse>> AddAsync(PollRequest poll, CancellationToken cancellation = default);
    Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellation = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellation = default);
    Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellation = default);
}
