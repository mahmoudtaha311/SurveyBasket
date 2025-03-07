using Microsoft.AspNetCore.RateLimiting;

namespace SurveyBasket.Api.Controllers;
[Route("[controller]")]
[ApiController]
[EnableRateLimiting("ipLimit")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody] loginRequest request, CancellationToken cancellation)
    {
        _logger.LogInformation("Logging with email:{email} and password:{password}", request.email, request.Password);

        var AuthResult = await _authService.GetTokenAsync(request.email, request.Password, cancellation);

        return AuthResult.IsSuccess ? Ok(AuthResult.Value) : AuthResult.ToProblem();
    }



    [HttpPost("refresh")]
    public async Task<IActionResult> Refrsh([FromBody] RefreshTokenRequest request, CancellationToken cancellation)
    {
        var AuthResult = await _authService.GetRefreshTokenAsync(request.token, request.refreshToken, cancellation);

        return AuthResult.IsSuccess ? Ok(AuthResult.Value) : AuthResult.ToProblem();
    }

    [HttpPost("Revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefrshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellation)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.token, request.refreshToken, cancellation);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellation)
    {
        var result = await _authService.RegisterAsync(request, cancellation);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellation)
    {
        var result = await _authService.ConfirmEmailAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }


    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmailAsync([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellation)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request.Email);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }

}