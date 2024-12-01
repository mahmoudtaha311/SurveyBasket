using MapsterMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IpollService pollservice ) : ControllerBase
{
    private readonly IpollService _pollservice = pollservice;
  

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        var polls =  _pollservice.GetAll();

        var response = polls.Adapt<IEnumerable<PollResponse>>();    
        return Ok(response);
    }
    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute]int id)
    {
        var poll = _pollservice.GetById(id);   
        

        return poll is null? NotFound() : Ok(poll.Adapt<PollResponse>()); 

        
    }
    [HttpPost("")]
    public IActionResult Add([FromBody] CreatePollRequest request )
    {
        
        var newpoll = _pollservice.Add(request.Adapt<Poll>());
        return CreatedAtAction(nameof(GetById), new { id = newpoll.Id }, newpoll);
        
    }
    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id,[FromBody] CreatePollRequest request) 
    {
        var isUpdated = _pollservice.Update(id, request.Adapt<Poll>());
        if (!isUpdated)
        {
            return NotFound();
        }
        return NoContent();
       
    }
    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute]int id) 
    {
        var isDeleted = _pollservice.Delete(id);

        return isDeleted ? NoContent() : NotFound();
    }
}
