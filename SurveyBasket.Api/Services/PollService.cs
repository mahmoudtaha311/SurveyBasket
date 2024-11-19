namespace SurveyBasket.Api.Services;

public class PollService : IpollService
{
    private readonly List<Poll> _polls = [

       new Poll{
            Description = "my first poll" ,
            Id = 1,
            Title = "poll 1"
        }


       ];

    public Poll Add(Poll poll)
    {
        poll.Id = _polls.Count + 1;
        _polls.Add( poll );
        return poll;
    }

    public bool Delete(int id)
    {
         var poll = GetById( id );
        if (poll is null) 
            return false;
        _polls.Remove( poll );
        return true;
        
         

    }

    public IEnumerable<Poll> GetAll() => _polls;
    

    public Poll? GetById(int id) => _polls.SingleOrDefault(pol => pol.Id == id);

    public bool Update(int id, Poll poll)
    {
        var currentpoll = GetById(id);
        if (currentpoll is  null) 
            return false;
        
        currentpoll.Description = poll.Description;
        currentpoll.Title = poll.Title;
        return true;
        


    }
}
