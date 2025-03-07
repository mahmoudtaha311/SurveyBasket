namespace SurveyBasket.Api.Errors;

public record UserErrors
{
    public static readonly Error InvalidCredentials =
        new Error("User.InvalidCredentials", "InValid Email Or Password", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);


    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
    public static readonly Error DuplicatedEmail =

       new("User.DuplicatedEmail", "Another user with the same email is already Exists", StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed =
        new("User.EmailNotConfirmed", "Email Is Not confirmed", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidCode =
       new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
    public static readonly Error DuplicatedConfirmation =
       new("User.DuplicatedConfirmation", "Email already Confirmed ", StatusCodes.Status400BadRequest);
    public static readonly Error DisabledUser =
       new("User.DisabledUser", "Disabled user, please contact administrator ", StatusCodes.Status400BadRequest);
    public static readonly Error LockedUser =
      new("User.LockedUser", "Locked user, please contact administrator ", StatusCodes.Status400BadRequest);
    public static readonly Error UserNotFound =
      new("User.UserNotFound", "User is not found ", StatusCodes.Status404NotFound);

    public static readonly Error InvalidRoles =
        new("User.InvalidRoles", " Invalid Roles ", StatusCodes.Status400BadRequest);
}
