namespace SurveyBasket.Api.Abstractions;

public static class Permissions
{
    public static string Type { get; } = "permissions";

    public const string GetPolls = "Polls:read";
    public const string AddPolls = "Polls:add";
    public const string UpdatePolls = "Polls:update";
    public const string DeletePolls = "Polls:delete";



    public const string GetQuestions = "Questions:read";
    public const string AddQuestions = "Questions:add";
    public const string UpdateQuestions = "Questions:update";



    public const string GetUsers = "Users:read";
    public const string AddUsers = "Users:add";
    public const string UpdateUsers = "Users:update";


    public const string GetRoles = "Roles:read";
    public const string AddRoles = "Roles:add";
    public const string UpdateRoles = "Roles:update";



    public const string Results = "Results:read";


    public static IList<string?> GetAllPermissions() =>
        typeof(Permissions).GetFields().Select(f => f.GetValue(f) as string).ToList();


}
