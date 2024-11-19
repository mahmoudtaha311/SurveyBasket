namespace SurveyBasket.Api.Controllers;  

[Route("api/[controller]")]
[ApiController]
public class PollsController(IpollService pollservice) : ControllerBase
{
    private readonly IpollService _pollservice = pollservice;

    
    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {

        return Ok(_pollservice.GetAll());
    }
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var poll = _pollservice.GetById(id);       
        return poll is null? NotFound() : Ok(poll); 
    }
    [HttpPost("")]
    public IActionResult Add( Poll request)
    {
        var newpoll = _pollservice.Add(request);
        return CreatedAtAction(nameof(GetById), new {id = newpoll.Id} , newpoll);
    }
    [HttpPut("{id}")]
    public IActionResult Update(int id,Poll request) 
    {
        var isUpdated = _pollservice.Update(id, request);
        if (!isUpdated) 
        {
            return NotFound();
        }
        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) 
    {
        var isDeleted = _pollservice.Delete(id);

        return isDeleted ? NoContent() : NotFound();
    }
}
