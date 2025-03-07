namespace SurveyBasket.Api.Contracts.Roles;

public record RoleRequest(
    string Name,
    IList<string> Premissions
    );