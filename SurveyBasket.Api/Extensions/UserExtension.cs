using System.Security.Claims;

namespace SurveyBasket.Api.Extensions;

public static class UserExtension
{
    public static string? GetUserId(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier);
}
