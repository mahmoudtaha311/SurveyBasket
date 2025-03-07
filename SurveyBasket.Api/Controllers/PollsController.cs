using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;

namespace SurveyBasket.Api.Controllers;
[ApiVersion(1, Deprecated = true)]
[ApiVersion(2)]
[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollservice) : ControllerBase
{
    private readonly IPollService _pollservice = pollservice;
    [HttpGet("GetAll")]
    [HasPermission(Permissions.GetPolls)]
    public async Task<IActionResult> GetAll(CancellationToken cancellation)
    {

        return Ok(await _pollservice.GetAllAsync(cancellation));
    }
    [MapToApiVersion(1)]
    [HttpGet("current")]
    [Authorize(Roles = DefaultRoles.Member.Name)]
    [EnableRateLimiting(OptionsRateLimiting.UserLimitPolicy)]
    public async Task<IActionResult> GetCurrentV1(CancellationToken cancellation)
    {

        return Ok(await _pollservice.GetCurrentAsyncV1(cancellation));
    }
    [MapToApiVersion(2)]
    [HttpGet("current")]
    [Authorize(Roles = DefaultRoles.Member.Name)]
    [EnableRateLimiting(OptionsRateLimiting.UserLimitPolicy)]
    public async Task<IActionResult> GetCurrentV2(CancellationToken cancellation)
    {

        return Ok(await _pollservice.GetCurrentAsyncV2(cancellation));
    }


    [HttpGet("{id}")]
    [HasPermission(Permissions.GetPolls)]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellation)
    {
        var result = await _pollservice.GetByIdAsync(id, cancellation);
        var response = result.Value.Adapt<PollResponse>();

        return result.IsSuccess ? Ok(response) : result.ToProblem();


    }
    [HttpPost("")]
    [HasPermission(Permissions.AddPolls)]

    public async Task<IActionResult> Add([FromBody] PollRequest request, CancellationToken cancellation)
    {

        var result = await _pollservice.AddAsync(request, cancellation);
        return result.IsSuccess ?
            CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value) : result.ToProblem();


    }
    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdatePolls)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request, CancellationToken cancellation)
    {
        var result = await _pollservice.UpdateAsync(id, request, cancellation);


        return result.IsSuccess ? NoContent() : result.ToProblem();


    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeletePolls)]

    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellation)
    {
        var result = await _pollservice.DeleteAsync(id, cancellation);

        return result.IsSuccess ? NoContent() : result.ToProblem();


    }
    [HttpPut("{id}/toggle-published")]
    [HasPermission(Permissions.UpdatePolls)]
    public async Task<IActionResult> TogglePublishStatus(int id, CancellationToken cancellation)
    {
        var result = await _pollservice.TogglePublishStatusAsync(id, cancellation);

        return result.IsSuccess ? NoContent() : result.ToProblem();

    }
}
