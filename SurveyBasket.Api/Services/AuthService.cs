
using Hangfire;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Helpers;

namespace SurveyBasket.Api.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
    IJwtProvider jwtProvider, SignInManager<ApplicationUser> signInManager,
    ILogger<AuthService> logger,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor,
    ApplicationDbContext context) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ApplicationDbContext _context = context;
    private readonly int _RefreshTokenExpiryDays = 14;



    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellation = default)
    {

        // check email
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.DisabledUser);


        //if(! user.EmailConfirmed)
        //    return Result.Failure<AuthResponse>(UserErrors.EmailNotconfirmed);

        var result = await _signInManager.PasswordSignInAsync(user, password, false, true);

        if (result.Succeeded)
        {


            var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellation);


            // generate jwt token 
            var (token, expiresin) = _jwtProvider.GenerateToken(user, userRoles, userPermissions);


            var refreshtoken = GenerateRefreshToken();
            var refreshtokenexpiration = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshtoken,
                ExpiresOn = refreshtokenexpiration
            });
            await _userManager.UpdateAsync(user);


            // return authresponse
            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresin, refreshtoken, refreshtokenexpiration);

            return Result.Success(response);

        }
        var error = result.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : result.IsLockedOut
            ? UserErrors.LockedUser
            : UserErrors.InvalidCredentials;

        return Result.Failure<AuthResponse>(error);


    }
    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellation = default)
    {
        var UserId = _jwtProvider.ValidateToken(token);

        if (UserId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(UserId);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

        if (user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.LockedUser);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);
        userRefreshToken.RevokOn = DateTime.UtcNow;

        var (userRoles, userPermissions) = await GetUserRolesAndPermissions(user, cancellation);


        var (newtoken, expiresin) = _jwtProvider.GenerateToken(user, userRoles, userPermissions);


        var newrefreshtoken = GenerateRefreshToken();
        var refreshtokenexpiration = DateTime.UtcNow.AddDays(_RefreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newrefreshtoken,
            ExpiresOn = refreshtokenexpiration
        });
        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newtoken, expiresin, newrefreshtoken, refreshtokenexpiration);

        return Result.Success(response);

    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellation = default)
    {
        var UserId = _jwtProvider.ValidateToken(token);
        if (UserId is null)
            return Result.Failure(UserErrors.InvalidJwtToken);
        var user = await _userManager.FindByIdAsync(UserId);
        if (user is null)
            return Result.Failure(UserErrors.InvalidJwtToken);
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);
        userRefreshToken.RevokOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);
        return Result.Success();



        throw new NotImplementedException();
    }

    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailIsExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var user = request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, request.PassWord);

        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Confirmation code : {code}", code);

            //BackgroundJob.Enqueue(() => SendConfirmationEmail(user, code))
            await SendConfirmationEmail(user, code);
            return Result.Success();
        }


        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);

        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, DefaultRoles.Member.Name);
            return Result.Success();
        }
        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result.Success();
        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Confirmation code : {code}", code);

        await SendConfirmationEmail(user, code);

        return Result.Success();
    }

    public async Task<Result> SendResetPasswordCodeAsync(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed with { StatusCode = StatusCodes.Status400BadRequest });

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Reset code : {code}", code);

        await SendResetPasswordEmail(user, code);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserErrors.InvalidCode);
        IdentityResult result;
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);

        }
        catch (FormatException)
        {

            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (result.Succeeded)
            return Result.Success();
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }





    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmtion",
            new Dictionary<string, string>
            {
                { "{{name}}" , user.FirstName},
                   { "{{action_url}}" ,$"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"}
           }
        );

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket :Email Confirmation :", emailBody));
        await Task.CompletedTask;
    }

    private async Task SendResetPasswordEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
            new Dictionary<string, string>
            {
                { "{{name}}" , user.FirstName},
                   { "{{action_url}}" ,$"{origin}/auth/ForgetPassord?email={user.Email}&code={code}"}
           }
        );

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket : Change Password :", emailBody));
        await Task.CompletedTask;
    }

    private async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissions(ApplicationUser user, CancellationToken cancellation)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        //var userPermissions = await _context.Roles
        //     .Join(_context.RoleClaims,
        //          role => role.Id,
        //          claim => claim.RoleId,
        //          (role, claim) => new { role, claim }
        //     )
        //     .Where(x => userRoles.Contains(x.role.Name!))
        //     .Select(x => x.claim.ClaimValue!)
        //     .Distinct()
        //     .ToListAsync(cancellation);
        var userPermissions = await (from r in _context.Roles
                                     join p in _context.RoleClaims
                                     on r.Id equals p.RoleId
                                     where userRoles.Contains(r.Name!)
                                     select p.ClaimValue!)
                                     .Distinct()
                                     .ToListAsync(cancellation);


        return (userRoles, userPermissions);
    }
}
