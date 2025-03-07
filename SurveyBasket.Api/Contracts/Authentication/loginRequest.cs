namespace SurveyBasket.Api.Contracts.Authentication;

public record loginRequest(
    string email,
    string Password
    );
