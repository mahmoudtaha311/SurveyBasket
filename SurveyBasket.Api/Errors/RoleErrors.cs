namespace SurveyBasket.Api.Errors;

public class RoleErrors
{
    public static readonly Error RoleNotFound =
        new Error("Role.RoleNotFound", "Role is not found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidPermissions =
        new("Role.InvalidPermissions", "Invalid Permissions ", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedRole =

       new("Role.DuplicatedRole", "Another role with the same name is already Exists", StatusCodes.Status409Conflict);


}
