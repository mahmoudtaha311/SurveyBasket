namespace SurveyBasket.Api.Contracts.User;

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Email,

    IList<string> Roles

    );
