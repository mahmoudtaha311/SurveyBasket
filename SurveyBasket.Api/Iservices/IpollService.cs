namespace SurveyBasket.Api.Iservices;

public interface IpollService
{
    IEnumerable<Poll> GetAll();
    Poll GetById(int id);
    Poll Add (Poll poll);
    bool Update(int id, Poll poll);
    bool Delete(int id);
}
