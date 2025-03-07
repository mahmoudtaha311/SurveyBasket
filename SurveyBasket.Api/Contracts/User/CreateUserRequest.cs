namespace SurveyBasket.Api.Contracts.User;

public record CreateUserRequest
    (

    string FirstName,
    string LastName,
    string Email,
   string Password,
    IList<string> Roles
    );

