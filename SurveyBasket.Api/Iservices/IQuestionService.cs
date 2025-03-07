namespace SurveyBasket.Api.Iservices;

public interface IQuestionService
{
    Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilters filters, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default);
    Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default);
    Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellation = default);
    Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default);
    Task<Result> Updateasync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default);


}
