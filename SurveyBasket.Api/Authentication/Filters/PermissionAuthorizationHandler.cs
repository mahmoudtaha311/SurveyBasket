namespace SurveyBasket.Api.Authentication.Filters;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {

        if (context.User.Identity is not { IsAuthenticated: true } ||
           !context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type))
            return;
        context.Succeed(requirement);
        return;
    }
}
