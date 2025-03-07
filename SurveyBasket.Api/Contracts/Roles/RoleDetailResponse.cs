namespace SurveyBasket.Api.Contracts.Roles;

public record RoleDetailResponse(
    string Id,
    string Name,
    bool IsDeleted,
    IEnumerable<string> Permissions

    );
