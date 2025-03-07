using SurveyBasket.Api.Contracts.Answers;
using System.Linq.Dynamic.Core;

namespace SurveyBasket.Api.Services;

public class QuestionService(ApplicationDbContext context, ICacheService cacheService, ILogger<QuestionService> logger) : IQuestionService
{
    private readonly ApplicationDbContext _context = context;

    private readonly ICacheService _cacheService = cacheService;

    private readonly ILogger<QuestionService> _logger = logger;

    private const string _cachePrefix = "availableQuestion";

    public async Task<Result<QuestionResponse>> GetAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Question
                          .Where(x => x.PollId == pollId && x.QuestionId == id)
                          .Include(x => x.Answers)
                          .ProjectToType<QuestionResponse>()
                          .AsNoTracking()
                          .SingleOrDefaultAsync(cancellationToken);
        if (question == null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);
        return Result.Success(question);
    }


    public async Task<Result<PaginatedList<QuestionResponse>>> GetAllAsync(int pollId, RequestFilters Filters, CancellationToken cancellationToken = default)
    {
        var pollIsExists = await _context.polls.AnyAsync(x => x.Id == pollId, cancellationToken);
        if (!pollIsExists)
            return Result.Failure<PaginatedList<QuestionResponse>>(PollErrors.pollNotFound);
        var query = _context.Question
                         .Where(x => x.PollId == pollId);
        if (!string.IsNullOrEmpty(Filters.SearchValue))
        {
            query = query.Where(x => x.Content.Contains(Filters.SearchValue));
        }

        if (!string.IsNullOrEmpty(Filters.SortColumn))
        {
            query = query.OrderBy($"{Filters.SortColumn} {Filters.SortDirection}");
        }


        var source = query
                         .Include(x => x.Answers)
                         .ProjectToType<QuestionResponse>()
                         .AsNoTracking();
        var questions = await PaginatedList<QuestionResponse>.CreateAsync(source, Filters.PageNumber, Filters.PageSize, cancellationToken);

        return Result.Success(questions);
    }


    public async Task<Result<QuestionResponse>> AddAsync(int pollId, QuestionRequest request, CancellationToken cancellation = default)
    {
        var pollIsExists = await _context.polls.AnyAsync(x => x.Id == pollId, cancellation);

        if (!pollIsExists)
            return Result.Failure<QuestionResponse>(PollErrors.pollNotFound);

        var questionIsExsists = await _context.Question.AnyAsync(x => x.Content == request.Content && x.PollId == pollId, cancellation);

        if (questionIsExsists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

        var question = request.Adapt<Question>();
        question.PollId = pollId;

        request.Answers.ForEach(answer => question.Answers.Add(new Answer { Content = answer }));

        await _context.AddAsync(question, cancellation);
        await _context.SaveChangesAsync(cancellation);

        await _cacheService.RemoveAsync($"{_cachePrefix}-{pollId}", cancellation);

        return Result.Success(question.Adapt<QuestionResponse>());



    }

    public async Task<Result> Updateasync(int pollId, int id, QuestionRequest request, CancellationToken cancellationToken = default)
    {
        var questionIsExsits = await _context.Question.AnyAsync(x => x.PollId == pollId
                                  && x.QuestionId != id
                                  && x.Content == request.Content, cancellationToken);

        if (questionIsExsits)
            return Result.Failure(QuestionErrors.DuplicatedQuestionContent);
        var question = await _context.Question
                                     .Include(x => x.Answers)
                                     .SingleOrDefaultAsync(x => x.QuestionId == id && x.PollId == pollId, cancellationToken);
        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.Content = request.Content;

        //current answer
        var currebrAnswers = question.Answers.Select(x => x.Content).ToList();
        // add new answer
        var newAnswers = request.Answers.Except(currebrAnswers).ToList();

        newAnswers.ForEach(answer =>
        {
            question.Answers.Add(new Answer { Content = answer });
        });

        question.Answers.ToList().ForEach(answer =>
        {
            answer.IsActive = request.Answers.Contains(answer.Content);
        });

        await _context.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);


        return Result.Success();



    }


    public async Task<Result> ToggleStatusAsync(int pollId, int id, CancellationToken cancellationToken = default)
    {
        var question = await _context.Question.SingleOrDefaultAsync(x => x.PollId == pollId && x.QuestionId == id, cancellationToken);
        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.IsActive = !question.IsActive;
        await _context.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"{_cachePrefix}-{pollId}", cancellationToken);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default)
    {
        

        var cashKey = $"{_cachePrefix}-{pollId}";

        var cacheQuestions = await _cacheService.GetAsync<IEnumerable<QuestionResponse>>(cashKey, cancellationToken);
        IEnumerable<QuestionResponse> Questions = [];
        if (cacheQuestions is null)
        {
            _logger.LogInformation("select questions from database");
            Questions = await _context.Question
                              .Where(x => x.PollId == pollId && x.IsActive)
                              .Include(x => x.Answers)
                                     .Select(q => new QuestionResponse(
                                          q.QuestionId,
                                          q.Content,
                                          q.Answers.Where(a => a.IsActive).Select(a => new AnswerResponse(a.AnswerId, a.Content))
                                     ))
                                     .AsNoTracking()
                                     .ToListAsync(cancellationToken);

            await _cacheService.SetAsync(cashKey, Questions, cancellationToken);

        }

        else
        {
            _logger.LogInformation("get question from cache");
            Questions = cacheQuestions;
        }



        return Result.Success<IEnumerable<QuestionResponse>>(Questions!);


    }
}
