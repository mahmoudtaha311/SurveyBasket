using SurveyBasket.Api.Helpers;

namespace SurveyBasket.Api.Services;

public class NotificationService(ApplicationDbContext context, UserManager<ApplicationUser> userManager
    , IHttpContextAccessor httpContextAccessor,
    IEmailSender emailSender) : INotificationService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task SendNewPollsNotification(int? pollId = null)
    {
        IEnumerable<Poll> polls = [];
        if (pollId.HasValue)
        {
            var poll = await _context.polls.SingleOrDefaultAsync(x => x.Id == pollId && x.IsPublished);
            polls = [poll!];
        }
        else
        {
            polls = await _context.polls
                .Where(x => x.IsPublished && x.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
                .ToListAsync();
        }
        //todo:
        var users = await _userManager.GetUsersInRoleAsync(DefaultRoles.Member.Name);

        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        foreach (var poll in polls)
        {
            foreach (var user in users)
            {
                var placeholders = new Dictionary<string, string>
                {
                    {"{{name}}",user.FirstName },
                    {"{{pollTitle}}",poll.Title },
                    {"{{endDate}}",poll.EndsAt.ToString() },
                    {"{{url}}",$"{origin}/polls/start/{poll.Id}" }
                };
                var body = EmailBodyBuilder.GenerateEmailBody("PollNotification", placeholders);
                await _emailSender.SendEmailAsync(user.Email!, $"Survey Basket : New Poll - {poll.Title}", body);
            }
        }

    }
}
