namespace SurveyBasket.Api;

public interface INotificationService
{

    Task SendNewPollsNotification(int? pollId = null);
}
